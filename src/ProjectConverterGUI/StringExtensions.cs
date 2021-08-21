namespace KsWare.ProjectConverterGUI {

	public static class StringExtensions {
		public static string LeadingSpace(this string s) {
			return string.IsNullOrWhiteSpace(s) ? s?.Trim() : " " + s.Trim();
		}

		public static string Quotes(this string s) {
			if (string.IsNullOrWhiteSpace(s)) return s;
			return s.Trim().Contains(" ") ? $"\"{s}\"" : s.Trim();
		}
	}

}