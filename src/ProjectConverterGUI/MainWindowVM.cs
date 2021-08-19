using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsWare.Presentation.Lite;

namespace ProjectConverterGUI {

	public class MainWindowVM : NotifyPropertyChangedBase {

		private string _inputPath;

		public string InputPath { get => _inputPath; set => Set(ref _inputPath, value); }
	}
}
