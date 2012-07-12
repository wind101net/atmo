using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Atmo;
using Atmo.Data;
using System.IO;
using System.Linq;
using Atmo.UI.DevEx.Controls;
using Atmo.Units;
using log4net;


namespace Atmo.UI.DevEx {
    public partial class ImportDataForm : DevExpress.XtraEditors.XtraForm {

		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static IEnumerable<FileInfo> GetAnemFiles(DirectoryInfo folder) {
			try {
				return folder.GetFiles()
					.Where(fi => DaqDataFileInfo.AnemFileNameRegex.IsMatch(fi.Name))
					.OrderBy(fi => fi.Name)
					;
			}
			catch {
				return Enumerable.Empty<FileInfo>();
			}
        }

		private static IEnumerable<PackedReading> GetPackedReadings(DaqDataFileInfo fileInfo) {
			using (var file = fileInfo.CreateReader()) {
                while (file.MoveNext()) {
                    yield return file.Current;
                }
            }
        }




		private static IEnumerable<DaqDataFileInfo> GetAnemFileInfos(IEnumerable<FileInfo> fileInfos) {
			return fileInfos.Select(DaqDataFileInfo.Create);
        }


        private IDataStore _dataStore;
		private IDaqConnection _device;
		private Dictionary<int, List<DaqDataFileInfo>> _fileInfosLookup;
		public bool AutoImport { get; set; }
    	private ImportAnemMap[] _importAnemMaps;
    	private FolderBrowserDialog _fbd;

		public string DataFolderPath { get; set; }

		public PersistentState PersistentState  { get; set; }


		public ImportDataForm(IDataStore dataStore, IDaqConnection device) {
			AutoImport = false;
            _dataStore = dataStore;
            _device = device;
            _fileInfosLookup = null;
            InitializeComponent();
			_importAnemMaps = new[] {
				importAnemMap0,
				importAnemMap1,
				importAnemMap2,
				importAnemMap3
			};
			
			_fbd = new FolderBrowserDialog();
		}

        private void buttonCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void buttonSelectDataFolder_Click(object sender, EventArgs e) {
            
            _fbd.Description = "Select a folder containing the data files";
            _fbd.ShowNewFolderButton = false;
        	_fbd.RootFolder = Environment.SpecialFolder.MyComputer;
			if(null != PersistentState && !String.IsNullOrEmpty(PersistentState.LastDaqFileLoadPath)) {
				_fbd.SelectedPath = PersistentState.LastDaqFileLoadPath;
			}

            DialogResult result = _fbd.ShowDialog();
            if (result == DialogResult.OK) {
            	HandleFolderSelected(_fbd.SelectedPath);
            }
            else {
                ; // user cancel
            }
        }

		private void HandleFolderSelected(string dataFolderPath) {
			DataFolderPath = dataFolderPath;
			textEditFolderPath.Text = DataFolderPath;
			var fileInfos = new List<DaqDataFileInfo>(
				GetAnemFileInfos(GetAnemFiles(new DirectoryInfo(DataFolderPath))).Where(afi => null != afi)
			);

			SetAnemFileList(fileInfos);

			_fileInfosLookup = fileInfos
				.GroupBy(afi => afi.Nid)
				.ToDictionary(
					g => g.Key,
					g => g.ToList()
				);

			ActivateAnemImportMaps(_fileInfosLookup, _dataStore);

			buttonImport.Enabled = true;
			daqCheckTimer_Tick(null, null);
		}

		private void ActivateAnemImportMaps(Dictionary<int, List<DaqDataFileInfo>> fileInfos, IDataStore dataStore) {
            List<ISensorInfo> dbSensorInfos = dataStore.GetAllSensorInfos().ToList();
            string[] dbSensorNamesArray = dbSensorInfos.Select(sim => sim.Name).ToArray();
			for (int i = 0; i < _importAnemMaps.Length; i++) {
				var anemMapControl = _importAnemMaps[i];
				List<DaqDataFileInfo> afiCollection = null;
                anemMapControl.AnemId = ((char)((byte)('A') + i)).ToString();
                if (fileInfos.TryGetValue(i, out afiCollection) && null != afiCollection && afiCollection.Count > 0) {
                    var minFileStamp = afiCollection.Min(afi => afi.FirstStamp);
                    anemMapControl.StartStamp = minFileStamp;
                    anemMapControl.DatabaseSensorId = dataStore.GetLatestSensorNameForHardwareId(anemMapControl.AnemId) ?? String.Empty;
                    anemMapControl.SetNameSuggestions(dbSensorNamesArray);
                    anemMapControl.Enabled = true;
                }
                else {
                    anemMapControl.Enabled = false;
                }
            }
        }

		private void SetAnemFileList(IEnumerable<DaqDataFileInfo> fileInfos) {
            listBoxAnemFiles.Items.Clear();
			foreach (DaqDataFileInfo fi in fileInfos) {
                listBoxAnemFiles.Items.Add(fi.ToString());
            }
        }

		private DialogResult WarnDialog(string text, string caption) {
			return MessageBox.Show(
				text, caption,
				MessageBoxButtons.OK, MessageBoxIcon.Warning
			);
		}

		private ISensorInfo GetOrAddSensor(ImportAnemMap map) {
			var sensor = _dataStore
				.GetAllSensorInfos()
				.FirstOrDefault(si => String.Equals(si.Name, map.DatabaseSensorId, StringComparison.OrdinalIgnoreCase));
			if (null == sensor) {
				sensor = new SensorInfo(map.DatabaseSensorId, SpeedUnit.MetersPerSec, TemperatureUnit.Celsius, PressureUnit.InchOfMercury);
				_dataStore.AddSensor(sensor);
			}
			return sensor;
		}

		private class ImportDataSet {

			public ImportDataSet() {
				MovedToLocation = new Dictionary<DaqDataFileInfo, FileInfo>();
			}

			public ISensorInfo Sensor { get; set; }
			public List<DaqDataFileInfo> Files { get; set; }
			public Dictionary<DaqDataFileInfo, FileInfo> MovedToLocation { get; set; }
		}

		private class ImportDataProcesser {

			public ImportDataProcesser(IEnumerable<ImportDataSet> sets, bool delSource, bool recordOverwrite, IDataStore dataStore) {
				_moveSetQueue = new Queue<ImportDataSet>(sets);
				_deleteSource = delSource;
				_importSetQueue = new Queue<ImportDataSet>();
				MaxQueue = _moveSetQueue.Count;
				MoveProgress = 0.0;
				ImportProgress = 0.0;
				_recordOverwrite = recordOverwrite;
				_dataStore = dataStore;
				_deleteSource = delSource;
			}

			public int MaxQueue { get; private set; }
			public double MoveProgress { get; private set; }
			public double ImportProgress { get; private set; }

			public double Progress {
				get { return (MoveProgress + ImportProgress)/2.0; }
			}

			private readonly bool _recordOverwrite;
			private readonly bool _deleteSource;
			private readonly object _queueLock = new object();
			private Queue<ImportDataSet> _moveSetQueue;
			private Queue<ImportDataSet> _importSetQueue;
			private readonly IDataStore _dataStore;
			private string _message;

			public void DoWork(object sender, DoWorkEventArgs e) {
				/*
				var moveThread = new Thread(MoveData) {
					IsBackground = true
				};
				var importThread = new Thread(ImportData) {
					IsBackground = true
				};
				moveThread.Start(sender);
				importThread.Start(sender);
				moveThread.Join();
				Thread.Sleep(500);
				importThread.Join();*/

				try {
					MoveData(sender);
					ImportData(sender);
					e.Result = _message;
				}
				catch(Exception ex) {
					Log.Error("Import failure", ex);
					e.Result = "Import failed. See the log file for details.";
				}
			}

			private void MoveData(object o) {

				var atmoFolder =
					new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Atmo"));
				if (!atmoFolder.Exists)
					atmoFolder.Create();
				var atmoImportsFolder =
					new DirectoryInfo(Path.Combine(atmoFolder.FullName, "Imports"));
				if (!atmoImportsFolder.Exists)
					atmoImportsFolder.Create();

				Action<int> reportProgress = null;
				if(o is BackgroundWorker)
					reportProgress = (o as BackgroundWorker).ReportProgress;
				else
					reportProgress = x => {};

				while(true) {
					ImportDataSet currentSet;
					lock (_queueLock) {
						if(_moveSetQueue.Count == 0) {
							_moveSetQueue = null;
							break;
						}
						currentSet = _moveSetQueue.Dequeue();
					}

					foreach (var file in currentSet.Files) {
						var importTarget = new FileInfo(
							Path.Combine(
								Path.Combine(atmoImportsFolder.FullName, file.Nid.ToString()),
								file.Path.Name
							)
						);

						if (file.Path != importTarget && file.Path.Exists) {
							if (!importTarget.Directory.Exists)
								importTarget.Directory.Create();
							try {
								file.Path.CopyTo(importTarget.FullName, true);
								currentSet.MovedToLocation[file] = importTarget;
								if (_deleteSource) {
									file.Path.Delete();
								}
							}
							catch {
								;
							}
						}
					}

					lock(_queueLock) {
						MoveProgress = (MaxQueue - _moveSetQueue.Count) / (double)MaxQueue;
						reportProgress((int)(Progress * 100));
						_importSetQueue.Enqueue(currentSet);
					}
				}
				lock(_queueLock) {
					MoveProgress = 1.0;
					reportProgress((int)(Progress * 100));
				}
			}

			private void ImportData(object o) {
				int doneCount = 0;
				int failedCount = 0;
				int successCount = 0;

				Action<int> reportProgress = null;
				if(o is BackgroundWorker)
					reportProgress = (o as BackgroundWorker).ReportProgress;
				else
					reportProgress = x => { };

				while(true) {
					ImportDataSet currentSet = null;
					lock(_queueLock) {
						if(_importSetQueue.Count == 0) {
							if(null == _moveSetQueue) {
								_importSetQueue = null;
								break;
							}
							Thread.Sleep(100);
						}
						else {
							currentSet = _importSetQueue.Dequeue();
						}
					}
					if (null != currentSet) {
						bool savedData = false;
						int lastAnemId = -1;
						foreach (var file in currentSet.Files) {
							FileInfo fileLocation;
							if (!currentSet.MovedToLocation.TryGetValue(file, out fileLocation))
								fileLocation = file.Path;

							if (null != fileLocation) {

								var currentFile = file.Path == fileLocation ? file : DaqDataFileInfo.Create(fileLocation);
								try {
									var pushOk = _dataStore.Push(
										currentSet.Sensor.Name, GetPackedReadings(currentFile), _recordOverwrite);

									if (pushOk) {
										successCount++;
										savedData = true;
										lastAnemId = file.Nid;
									}
									else {
										failedCount++;
									}
								}
								catch {
									failedCount++;
								}
							}

						}
						if (savedData) {
							_dataStore.SetLatestSensorNameForHardwareId(
								currentSet.Sensor.Name,
								((char) (lastAnemId + (byte) ('A'))).ToString()
							);
						}

						System.Diagnostics.Debug.WriteLine("Importing..." + currentSet.Files.Count);
						doneCount++;
					}
					lock(_queueLock) {
						ImportProgress = doneCount / (double)MaxQueue;
						reportProgress((int)(Progress * 100));
					}
				}

				_message = null;
				if (failedCount > 0 || successCount == 0)
				{
					_message = "Data import failed.";
				}
				else {

					_message = "Data import completed successfully.";
				}

				lock (_queueLock) {
					ImportProgress = 1.0;
					reportProgress((int)(Progress * 100));
				}

			}


		}

		private void backgroundWorkerImport_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			progressBarControl1.Position = e.ProgressPercentage;
		}

		private void backgroundWorkerImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			System.Diagnostics.Debug.WriteLine("DONE!!!");
			buttonImport.Enabled = true;
			progressBarControl1.Position = 100;
			if(e.Result != null) {
				MessageBox.Show(e.Result.ToString(), "Result", MessageBoxButtons.OK);
			}
		}

		private void backgroundWorkerImport_DoWork(object sender, DoWorkEventArgs e) {
			((ImportDataProcesser)e.Argument).DoWork(sender, e);
		}

		private void buttonImport_Click(object sender, EventArgs e) {
			var validMaps = new List<ImportAnemMap>();
			var invalidMaps = new List<ImportAnemMap>();
			foreach (
				var anemMapControl in _importAnemMaps
				.Where(c => c.Enabled && c.Checked)
			) {
				(anemMapControl.IsValid ? validMaps : invalidMaps)
					.Add(anemMapControl);
			}
			if (invalidMaps.Count > 0) {
				WarnDialog(
					"All anemometers selected for import require a database name to be specified.",
					"Invalid Import Request"
				);
				return;
			}

			var setProcessor = new ImportDataProcesser(validMaps.Select(map => new ImportDataSet {
				Sensor = GetOrAddSensor(map),
				Files = _fileInfosLookup[(byte)(Char.Parse(map.AnemId.ToUpper())) - (byte)('A')]
					.OrderBy(afi => afi.FirstStamp).ToList()
			}), checkEditDelSource.Checked, chkOverwrite.Checked, _dataStore);

			progressBarControl1.Position = 0;
			buttonImport.Enabled = false;
			backgroundWorkerImport.RunWorkerAsync(setProcessor);

		}


        private void daqCheckTimer_Tick(object sender, EventArgs e) {
            syncChk.Enabled = null != _device
                &&  null != _fileInfosLookup
                && _fileInfosLookup.Any()
                && _device.IsConnected
            ;
        }

        private void ImportDataForm_Load(object sender, EventArgs e) {
            ThreadPool.QueueUserWorkItem(ReconnectSD, _device);
			if (null != PersistentState && !String.IsNullOrEmpty(PersistentState.LastDaqFileLoadPath)) {
				HandleFolderSelected(PersistentState.LastDaqFileLoadPath);
			}
        }

        public void ReconnectSD(object state)
        {
			var device = (state as IDaqConnection);
            if (null != device)
            {
                device.ReconnectMedia();
            }
        }

		private void textEditFolderPath_Leave(object sender, EventArgs e) {
			HandleFolderSelected(textEditFolderPath.Text);
		}

		private void ImportDataForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (backgroundWorkerImport.IsBusy) {
				e.Cancel = true;
				MessageBox.Show("The import is still processing.", "Please wait...");
			}
		}

		

		
    }
}