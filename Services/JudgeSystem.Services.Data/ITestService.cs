﻿namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Test;

	public interface ITestService
	{
		Task<Test> Add(TestInputModel model);

		IEnumerable<TestViewModel> TestsByProblem(int problemId);

		Task<Test> GetById(int id);

		Task Delete(Test test);

		Task Update(Test test);

		IEnumerable<TestDataDto> GetTestsByProblemId(int problemId);
	}
}
