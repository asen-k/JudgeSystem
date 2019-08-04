﻿using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.ViewModels.Test;
using JudgeSystem.Web.InputModels.Test;

namespace JudgeSystem.Services.Data
{
    public interface ITestService
	{
		Task<TestDto> Add(TestInputModel model);

		IEnumerable<TestViewModel> GetTestsByProblemIdOrderedByIsTrialDescending(int problemId);

		Task<TDestination> GetById<TDestination>(int id);

		Task Delete(int id);

		Task Update(TestEditInputModel test);

		IEnumerable<TestDataDto> GetTestsByProblemId(int problemId);
	}
}
