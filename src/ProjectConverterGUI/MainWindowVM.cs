using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using KsWare.Presentation.Lite;
using Microsoft.Xaml.Behaviors.Core;

namespace KsWare.ProjectConverterGUI {

	public class MainWindowVM : NotifyPropertyChangedBase {

		private string _inputPath;
		private ConverterTool _tool;
		private bool _createBackup;
		private string _parameter;

		public string InputPath { get => _inputPath; set => Set(ref _inputPath, value); }

		public bool CreateBackup { get => _createBackup; set => Set(ref _createBackup, value); }
		public string Parameter { get => _parameter; set => Set(ref _parameter, value); }
		public ObservableCollection<string> ParameterMru { get; } = new ObservableCollection<string>();

		public ConverterTool[] AvailableTools { get; } = {
			new ConverterTool {
				Name = "upgrade-assistant",
				ProjectUrl = "https://github.com/dotnet/upgrade-assistant",
				WizardCommand="upgrade-assistant upgrade $(Options) $(InputPath)",
				CreateBackup="",
				SkipBackup="--skip-backup",
				InstallCommand = "dotnet tool install -g upgrade-assistant",
				UpdateCommand = "dotnet tool update -g upgrade-assistant"
			},
			new ConverterTool {
				Name = "migrate-2019",
				ProjectUrl="https://github.com/hvanbakel/CsprojToVs2017",
				WizardCommand="dotnet migrate-2019 wizard $(Options) $(InputPath)",
				CreateBackup="",
				SkipBackup="--no-backup",
				InstallCommand = "dotnet tool install --global Project2015To2017.Migrate2019.Tool",
				UpdateCommand = "dotnet tool update --global Project2015To2017.Migrate2019.Tool" //??
			}
		};

		public ConverterTool Tool { get => _tool; set => Set(ref _tool, value); }

		public ICommand StartCommand { get; }

		/// <inheritdoc />
		public MainWindowVM() {
			StartCommand = new ActionCommand(StartAction);

			CreateBackup = false;
			InputPath = @"D:\Develop\Extern\GitHub.KsWare\KsWare.IO.NamedPipes\src\KsWare.IO.NamedPipes.Tests\Tests.csproj";
			ParameterMru.Add("-t net45 -t netcoreapp3.1 -t net5.0");
			ParameterMru.Add("-t net45 -t netstandard2.0");
		}

		private void StartAction() {
			var command = new ParameterBuilder(Tool.WizardCommand);
			var options = new ParameterBuilder();
			options.Append(CreateBackup ? Tool.CreateBackup : Tool.SkipBackup);
			options.AppendRaw(Parameter);
			command.Replace("$(InputPath)", InputPath);
			command.Replace("$(Options)", options);

			var psi = new ProcessStartInfo("cmd.exe", $"/K {command}") {
				UseShellExecute = true
			};
			Debug.WriteLine($"Execute {psi.FileName} {psi.Arguments}");
			var p = Process.Start(psi);
			p?.WaitForExit();
		}
	}

}
