﻿using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Submission;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class SubmissionServiceTests : TransientDbContextProvider
    {
        private readonly IEstimator estimator = new Estimator();

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var service = await CreateSubmissionService(new List<Submission>());
            string code = "using System;";
            string userId = "use_test_id";
            var submission = new SubmissionInputModel
            {
                SubmissionContent = Encoding.UTF8.GetBytes(code),
                ContestId = null,
                ProblemId = 1,
            };

            var actualSubmission = await service.Create(submission, userId);

            Assert.Equal(Encoding.UTF8.GetString(actualSubmission.Code), code);
            Assert.Null(actualSubmission.CompilationErrors);
            Assert.Equal(actualSubmission.ProblemId, submission.ProblemId);
            Assert.Contains(this.context.Submissions, x => x.Id == actualSubmission.Id);
        }

        [Fact]
        public async Task GetSubmissionResult_WithValidId_ShouldReturnCorrectData()
        {
            var testData = GetDetailedTestData();
            var service = await CreateSubmissionService(testData);

            var actualResult = service.GetSubmissionResult(3);
            var expectedResult = testData.First(x => x.Id == 3);

            Assert.Equal(expectedResult.ActualPoints, actualResult.ActualPoints);
            Assert.True(actualResult.IsCompiledSuccessfully);
            Assert.Equal(actualResult.ExecutedTests.Count, expectedResult.ExecutedTests.Count);
            foreach (var executedTest in actualResult.ExecutedTests)
            {
                Assert.Contains(expectedResult.ExecutedTests, x => x.IsCorrect == executedTest.IsCorrect 
                && x.ExecutionResultType.ToString() == executedTest.ExecutionResultType);
            }
            Assert.Equal(expectedResult.Problem.MaxPoints, actualResult.MaxPoints);
            Assert.Equal(expectedResult.SubmisionDate.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture), 
                actualResult.SubmissionDate);
        }

        [Fact]
        public async Task GetSubmissionResult_WithInValidId_ShouldReturnNull()
        {
            var testData = GetDetailedTestData();
            var service = await CreateSubmissionService(testData);

            var actualResult = service.GetSubmissionResult(33);

            Assert.Null(actualResult);
        }

        [Theory]
        [InlineData(3, "me", 1, 10, "5, 7")]
        [InlineData(2, "me", 1, 2, "8, 1")]
        [InlineData(2, "me", 2, 2, "2, 4")]
        [InlineData(2, "me", 2, 3, "4")]
        [InlineData(2, "me", 2, 5, "")]
        public async Task GetUserSubmissionsByProblemIdAndContestId_WithValidDataAndDifferentPages_ShouldReturnCorrectData(
            int problemId, string userId, int page, int submissionsPerPage, string expectedIds)
        {
            var testData = PaginationTestData();
            var service = await CreateSubmissionService(testData);

            var actualSubmissions = service.GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);

            Assert.Equal(expectedIds, string.Join(", ", actualSubmissions.Select(x => x.Id)));
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetDetailedTestData();
            var service = await CreateSubmissionService(testData);
            var submission = this.context.Submissions.Find(2);
            submission.ContestId = 3;

            await service.Update(submission);
            var expecctedSubmission = this.context.Submissions.Find(2);

            Assert.Equal(expecctedSubmission.ContestId, submission.ContestId);
        }

        private async Task<SubmissionService> CreateSubmissionService(List<Submission> testData)
        {
            await this.context.Submissions.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IRepository<Submission> repository = new EfRepository<Submission>(this.context);
            var service = new SubmissionService(repository, this.estimator);
            return service;
        }

        private SubmissionService CreateSubmissionServiceWithMockedRepository(IQueryable<Submission> testData)
        {
            var reposotiryMock = new Mock<IRepository<Submission>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new SubmissionService(reposotiryMock.Object, this.estimator);
        }

        private List<Submission> GetDetailedTestData()
        {
            return new List<Submission>
            {
                new Submission
                {
                    Id = 1,
                    ActualPoints = 100,
                    CompilationErrors = null,
                    ExecutedTests = new List<ExecutedTest>
                    {
                        new ExecutedTest { IsCorrect = true, TimeUsed = 5, MemoryUsed = 10, ExecutionResultType = TestExecutionResultType.Success },
                        new ExecutedTest { IsCorrect = true, TimeUsed = 20, MemoryUsed = 20, ExecutionResultType = TestExecutionResultType.Success },
                        new ExecutedTest { IsCorrect = true, TimeUsed = 5, MemoryUsed = 10, ExecutionResultType = TestExecutionResultType.Success }
                    },
                    Problem = new Problem { MaxPoints = 100 },
                    SubmisionDate = new DateTime(2019, 5, 2),
                    ProblemId = 2,
                    UserId = "test_id",
                },
                new Submission
                {
                    Id = 2,
                    ActualPoints = 0,
                    CompilationErrors = Encoding.UTF8.GetBytes("invalid operation"),
                    Problem = new Problem { MaxPoints = 100 },
                    SubmisionDate = new DateTime(2019, 5, 2),
                    ProblemId = 1,
                    UserId = "test_id",
                },
                new Submission
                {
                    Id = 3,
                    ActualPoints = 50,
                    CompilationErrors = null,
                    ExecutedTests = new List<ExecutedTest>
                    {
                        new ExecutedTest { IsCorrect = true, TimeUsed = 5, MemoryUsed = 10, ExecutionResultType = TestExecutionResultType.Success },
                        new ExecutedTest { IsCorrect = false, TimeUsed = 50, MemoryUsed = 60, ExecutionResultType = TestExecutionResultType.MemoryLimit },
                        new ExecutedTest { IsCorrect = true, TimeUsed = 5, MemoryUsed = 10, ExecutionResultType = TestExecutionResultType.Success },
                        new ExecutedTest { IsCorrect = false, TimeUsed = 51, MemoryUsed = 14, ExecutionResultType = TestExecutionResultType.RunTimeError }
                    },
                    Problem = new Problem { MaxPoints = 100 },
                    ProblemId = 1,
                    UserId = "test_id",
                    SubmisionDate = new DateTime(2019, 6, 2),
                 }
            };
        }

        private List<Submission> PaginationTestData()
        {
            var firstProblem = new Problem { Id = 1 };
            var secondProblem = new Problem { Id = 2 };
            var thirdProblem = new Problem { Id = 3 };

            return new List<Submission>
            {
                new Submission { Id = 1, ProblemId = 2, UserId = "me", SubmisionDate = new DateTime(2019, 7, 2), Problem = secondProblem },
                new Submission { Id = 2, ProblemId = 2, UserId = "me", SubmisionDate = new DateTime(2019, 6, 2), Problem = secondProblem },
                new Submission { Id = 3, ProblemId = 1, UserId = "me", SubmisionDate = new DateTime(2019, 8, 2), Problem = firstProblem },
                new Submission { Id = 4, ProblemId = 2, UserId = "me", SubmisionDate = new DateTime(2018, 7, 2), Problem = secondProblem },
                new Submission { Id = 5, ProblemId = 3, UserId = "me", SubmisionDate = new DateTime(2019, 9, 2), Problem = thirdProblem },
                new Submission { Id = 6, ProblemId = 3, UserId = "me123", SubmisionDate = new DateTime(2019, 2, 2), Problem = thirdProblem },
                new Submission { Id = 7, ProblemId = 3, UserId = "me", SubmisionDate = new DateTime(2019, 4, 2), Problem = thirdProblem },
                new Submission { Id = 8, ProblemId = 2, UserId = "me", SubmisionDate = new DateTime(2019, 8, 2), Problem = secondProblem },
            };
        }
    }
}
