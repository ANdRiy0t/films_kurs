namespace kino_ku.Settings
{
	public static class RegexPatterns
	{
		public static string PasswordRegexPattern = @"^(?=.*[a-zA-Z])(?=.*\d).{8,}$";
		public static string NameRegexPattern = "^[a-zA-Z]{2,13}$";
		public static string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";


		public static string AddMessage(bool isValidate, string message)
		{
			if (!isValidate)
				return message;
			return "";
		}
	}
}
