using System;
using System.IO;

namespace BootstrapAI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			BootStrap _bootStrap = new BootStrap();
			_bootStrap.init();
		}
	}

class BootStrap {
		public static String _homeDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString() + "/";

		public void init ()
		{

			if (!File.Exists(_homeDir + "BootStrap.conf")) {
				var _confWrite = File.CreateText(_homeDir + "BootStrap.conf");
				_confWrite.Write("Current Version: 1");
				copyDirectory(_homeDir + "Git/Bootstrap-AI/0.1/Bootstrap-AI", _homeDir + "BootStrap/1");
				_confWrite.Close();
			}

			if (!Directory.Exists(_homeDir + "BootStrap")) {
				Directory.CreateDirectory(_homeDir + "BootStrap");
			}
			Console.Write(getCurrentVersion ());
			revertLastVersion ();
			Console.Write(getCurrentVersion ());
		}

		public static int getCurrentVersion ()
		{
			string[] _lines = System.IO.File.ReadAllLines (_homeDir + "BootStrap.conf");
			string _version = "";
			foreach (string _line in _lines) {
				if (_line.StartsWith("Current Version: ")) {
					_version = _line.Replace("Current Version: ", "").Trim();
					break;
				}
			}
			return Convert.ToInt32(_version);
		}

		private void createNextVersion() 
		{
			int _oldVersion = getCurrentVersion();
			int _newVersion = _oldVersion + 1;
			copyDirectory(_homeDir + "BootStrap/" + _oldVersion, _homeDir + "BootStrap/" + _newVersion);
			int _versionLine = 0;

			string[] _lines = System.IO.File.ReadAllLines (_homeDir + "BootStrap.conf");
			string _version = "";
			int _lineId = 0;
			foreach (string _line in _lines) {
				if (_line.StartsWith("Current Version: ")) {
					_versionLine = _lineId;
					break;
				}
				_lineId++;
			}
			_lines[_versionLine] = "Current Version: " + _newVersion;
			File.WriteAllLines(_homeDir + "BootStrap.conf", _lines);
		}

		private void revertLastVersion ()
		{
			string[] _lines = System.IO.File.ReadAllLines (_homeDir + "BootStrap.conf");
			int _version = 0;
			int _lineId = 0;
			foreach (string _line in _lines) {
				if (_line.StartsWith("Current Version: ")) {
					_version = Convert.ToInt32(_line.Replace("Current Version: ", ""));
					break;
				}
				_lineId++;
			}
			_version = _version - 1;
			_lines[_lineId] = "Current Version: " + _version;
			File.WriteAllLines(_homeDir + "BootStrap.conf", _lines);
		}

		private void copyDirectory(string sourceDir, string targetDir)
		{
   			Directory.CreateDirectory(targetDir);

    		foreach(var file in Directory.GetFiles(sourceDir))
      			File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
			foreach(var directory in Directory.GetDirectories(sourceDir))
        		copyDirectory(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
		}
	}
}