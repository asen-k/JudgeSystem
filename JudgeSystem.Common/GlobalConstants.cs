﻿namespace JudgeSystem.Common
{
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Administrator";

        public const string StudentRoleName = "Student";

		public const string AdministrationArea = "Administration";

		public const string InfoKey = "info";

		public const string ErrorKey = "error";

		public const string FileStorePath = "F:\\JudgeSystemStore\\";

		public const string WordFileExtension = ".docx";

		public const string PowerPointFileExtension = ".pptx";

		public const string VideoFileExtension = ".mp4";

		public const string ZipFileExtension = ".zip";

		public const int NameMinLength = 3;

		public const int ProblemMinPoints = 1;

		public const int ProblemMaxPoints = 100;

		public const int PasswordMinLength = 5;

		public const string ResourceTypesKey = "resourceTypes";

		public const int SessionIdleTimeout = 2;

		public const string StandardDateFormat = "dd/MM/yyyy HH:mm:ss";

		public const string StudentProfileActivationEmailSubject = "Student profile activation";

		public const string TemplatesFolder = "Templates";

		public const string EmailTemplatesFolder = "EmailTemplates";

		public const int PaginationOffset = 5;

		public const int SearchKeywordMinLength = 3;

		public const int MinClassNumber = 8;

		public const int MaxClassNumber = 12;

		public const int MinStudentsInClass = 1;

		public const int MaxStudentsInClass = 26;

        public const int SubmissionFileMaxSizeInKb = 16;

        public const int MaxSubmissionCodeLength = 15000;

        public const string OctetStreanMimeType = "application/octet-stream";

        public const string Location = "Location";

        public const string InvalidPracticeId = "Practice id must be specified";

        public const int SubmissionPerPage = 5;
    }
}
