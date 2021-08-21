using System.Text;
using System.Text.RegularExpressions;

namespace KsWare.ProjectConverterGUI {

	public class ParameterBuilder {

		private StringBuilder _sb;

		public ParameterBuilder(StringBuilder sb = null) {
			_sb = sb ?? new StringBuilder();
		}

		public ParameterBuilder(string rawString) {
			_sb = new StringBuilder(rawString);
		}

		public void Append(params string[] parameters) {
			foreach (var p in parameters) {
				var parameter = p;
				if (string.IsNullOrWhiteSpace(parameter)) continue;
				parameter = parameter.Trim();
				if (parameter.Contains(" ")) {
					parameter = $"\"{parameter}\"";
				}

				if (_sb.Length > 0) _sb.Append(' ');
				_sb.Append(parameter);
			}
		}

		public void AppendRaw(string parameters) {
			if (string.IsNullOrWhiteSpace(parameters)) return;
			parameters = parameters.Trim();
			if (_sb.Length > 0) _sb.Append(" ");
			_sb.Append(parameters);
		}

		public void AppendPair(string parameterPair) {
			if (string.IsNullOrWhiteSpace(parameterPair)) return;
			parameterPair = parameterPair.Trim();
			var parameters = parameterPair.Split(new[] { ' ' }, 2);
			Append(parameters);
		}

		public void Append(ParameterBuilder builder) => AppendRaw(builder.ToString());

		public void Replace(string variable, string replacement) {
			//TODO use _sb.Replace()
			variable = Regex.Escape(variable);
			replacement = replacement.Contains(" ") ? $"\"{replacement}\"" : replacement;
			_sb = new StringBuilder(Regex.Replace(_sb.ToString(), @"\s*" + variable, replacement.Quotes().LeadingSpace()));
		}

		public void Replace(string variable, ParameterBuilder replacement) {
			//TODO use _sb.Replace()
			variable = Regex.Escape(variable);
			_sb = new StringBuilder(Regex.Replace(_sb.ToString(), @"\s*" + variable, replacement.ToString().LeadingSpace()));
		}

		public void ReplaceRaw(string variable, string replacement) {
			//TODO use _sb.Replace()
			variable = Regex.Escape(variable);
			_sb = new StringBuilder(Regex.Replace(_sb.ToString(), @"\s*" + variable, replacement.LeadingSpace()));
		}

		public override string ToString() {
			return _sb.ToString().Trim();
		}

	}

}