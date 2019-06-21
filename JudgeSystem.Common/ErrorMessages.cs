﻿using System;

namespace JudgeSystem.Common
{
	public static class ErrorMessages
	{
		public const string NotFoundEntityMessage = "The required {0} was not found!";

		public const string DiffrentLessonPasswords = "Invalid old password.";

		public const string InvalidLessonPassword = "Invalid password.";

		public const string TooShorPasswordMessage = "The lesson password must be at least five symbols.";

		public const string LockedLesson = "The lesson already has password!";

		public const string NotValidEmail = "The provided email is not valid";

		public const string InvalidActivationKey = "Your activation key is invalid.";

		public const string ActivatedStudentProfile = "Student profile with this activation key is already activated.";

		public const string InvalidStudentProfile = "Your student profile is not activated! Connect to administrators for more info!";
	}
}
