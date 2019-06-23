﻿namespace JudgeSystem.Data.Seeding
{
	using System;
	using System.Threading.Tasks;
    using JudgeSystem.Common;
    using JudgeSystem.Data.Models;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class AdminSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
		{
			IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
			UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			ApplicationUser userFromDb = await userManager.FindByNameAsync(configuration["Admin:Name"]);
			if (userFromDb != null)
			{
				return;
			}

			ApplicationUser user = new ApplicationUser
			{
				Name = configuration["Admin:Name"],
				Email = configuration["Admin:Email"],
				UserName = configuration["Admin:Username"],
				Surname = configuration["Admin:Surname"],
			};

			await userManager.CreateAsync(user, configuration["Admin:Password"]);
			await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
		}
	}
}