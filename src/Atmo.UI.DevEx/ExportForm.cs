using System;
using System.Windows.Forms;
using Atmo.Data;
using System.Linq;
using System.IO;

namespace Atmo.UI.DevEx {
    public partial class ExportForm : DevExpress.XtraEditors.XtraForm {

		private enum ExportType {
			TenMinute = 0,
			Raw,
		}

        private readonly IDataStore _dataStore;

        public ExportForm(IDataStore dataStore) {
            _dataStore = dataStore;
            InitializeComponent();
            var now = DateTime.Now;
            dateTimeRangePicker.From = now;
            dateTimeRangePicker.To = now;
        }

        private void ExportForm_Load(object sender, EventArgs e) {
            var dbSensorInfos = _dataStore.GetAllSensorInfos().ToList();
            var dbSensorNamesArray = dbSensorInfos.Select(sim => sim.Name).ToArray();
            comboChooseSensor.Properties.Items.Clear();
            comboChooseSensor.Properties.Items.AddRange(dbSensorNamesArray);
			comboBoxEditExportType.Properties.Items.Clear();
			comboBoxEditExportType.Properties.Items.AddRange(
				Enum.GetValues(typeof(ExportType)));
        	comboBoxEditExportType.SelectedItem = default(ExportType);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnExecute_Click(object sender, EventArgs e) {
            var saveDialog = new SaveFileDialog {
				CheckPathExists = true,
				DefaultExt = "csv",
				Filter = "csv files (*.csv)|*.csv",
				AddExtension = true
			};
        	if (saveDialog.ShowDialog() == DialogResult.OK) {
                if (!ExecuteExport(saveDialog.FileName)) {
                    MessageBox.Show("No records saved", "No data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

		private static void WriteQuoted(StreamWriter writer, string text) {
			writer.Write('\"');
			writer.Write(text);
			writer.Write('\"');
		}

		private bool ExecuteExport(string csvFileName) {
			switch((ExportType)comboBoxEditExportType.SelectedItem)
			{
			case ExportType.Raw:
				return ExecuteExportRaw(csvFileName);
			case ExportType.TenMinute:
				return ExecuteExportTenMinute(csvFileName);
			default:
				return false;
			}
		}

		private bool ExecuteExportTenMinute(string csvFileName) {
			csvFileName = Path.ChangeExtension(csvFileName, "csv");
			var iniFileName = Path.ChangeExtension(csvFileName, "ini");
			using (var iniStream = File.Open(iniFileName, FileMode.Create, FileAccess.ReadWrite)) {
				using (var iniWriter = new StreamWriter(iniStream)) {
					iniWriter.Write('[');
					iniWriter.Write(Path.GetFileName(csvFileName));
					iniWriter.WriteLine(']');
					iniWriter.WriteLine("ColnameHeader=True");
					iniWriter.WriteLine("Format=CSVDelimited");
					iniWriter.WriteLine("DateTimeFormat=yyyy/MM/dd HH:nn:ss");
					iniWriter.WriteLine("Col1=Stamp DateTime");
					iniWriter.WriteLine("Col2=Temperature Double");
					iniWriter.WriteLine("Col3=TempSTDV Double");
					iniWriter.WriteLine("Col4=Pressure Double");
					iniWriter.WriteLine("Col5=PresSTDV Double");
					iniWriter.WriteLine("Col6=Humidity Double");
					iniWriter.WriteLine("Col7=HumiSTDV Double");
					iniWriter.WriteLine("Col8=Speed Double");
					iniWriter.WriteLine("Col9=SpdSTDV Double");
					iniWriter.WriteLine("Col10=Direction Double");
					iniWriter.WriteLine("Col11=DirSTDV Double");
					iniWriter.Flush();
				}
			}
			var readings = _dataStore.GetReadingSummaries(
				comboChooseSensor.SelectedText,
				dateTimeRangePicker.Min,
				dateTimeRangePicker.Max.Subtract(dateTimeRangePicker.Min),
				new TimeSpan(0,0,10,0)
			);
			int c = 0;
			using (var csvStream = File.Open(csvFileName, FileMode.Create, FileAccess.ReadWrite)) {
				using (var csvWriter = new StreamWriter(csvStream)) {

					WriteQuoted(csvWriter, "Stamp");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Temperature");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Temperature (stdv)");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Pressure");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Pressure (stdv)");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Humidity");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Humidity (stdv)");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Speed");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Speed (stdv)");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Direction");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Direction (stdv)");
					csvWriter.WriteLine();

					foreach (var reading in readings) {
						c++;
						WriteQuoted(csvWriter, reading.BeginStamp.ToString("yyyy/MM/dd HH:mm:ss"));
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.Mean.Temperature.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.SampleStandardDeviation.Temperature.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.Mean.Pressure.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.SampleStandardDeviation.Pressure.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.Mean.Humidity.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.SampleStandardDeviation.Humidity.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.Mean.WindSpeed.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.SampleStandardDeviation.WindSpeed.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.Mean.WindDirection.ToString());
						csvWriter.Write(',');
						WriteQuoted(csvWriter, reading.SampleStandardDeviation.WindDirection.ToString());
						csvWriter.WriteLine();
					}
					csvWriter.Flush();
				}
			}
			return c > 0;
		}

    	private bool ExecuteExportRaw(string csvFileName) {
			int c = 0;
			csvFileName = Path.ChangeExtension(csvFileName, "csv");
            var iniFileName = Path.ChangeExtension(csvFileName, "ini");
			using (FileStream
				iniStream = File.Open(iniFileName, FileMode.Create, FileAccess.ReadWrite),
				csvStream = File.Open(csvFileName, FileMode.Create, FileAccess.ReadWrite)
			) {
				using (StreamWriter
					iniWriter = new StreamWriter(iniStream),
					csvWriter = new StreamWriter(csvStream)
				) {
					var dataWriter = new CsvDataFileWriter(
						csvWriter, iniWriter,
						Path.GetFileName(csvFileName));

					var readings = _dataStore.GetReadings(
						comboChooseSensor.SelectedText,
						dateTimeRangePicker.Min,
						dateTimeRangePicker.Max.Subtract(dateTimeRangePicker.Min));
					
					foreach (var reading in readings) {
						dataWriter.Write(reading);
						c++;
					}

					iniWriter.Flush();
					csvWriter.Flush();
				}
			}
    		return c > 0;
    		/*
            csvFileName = Path.ChangeExtension(csvFileName, "csv");
            var iniFileName = Path.ChangeExtension(csvFileName, "ini");
            using (var iniStream = File.Open(iniFileName, FileMode.Create, FileAccess.ReadWrite)) {
                using (var iniWriter = new StreamWriter(iniStream)) {
                    iniWriter.Write('[');
                    iniWriter.Write(Path.GetFileName(csvFileName));
                    iniWriter.WriteLine(']');
                    iniWriter.WriteLine("ColnameHeader=True");
                    iniWriter.WriteLine("Format=CSVDelimited");
                    iniWriter.WriteLine("DateTimeFormat=yyyy/MM/dd HH:nn:ss");
                    iniWriter.WriteLine("Col1=Stamp DateTime");
                    iniWriter.WriteLine("Col2=Temperature Double");
                    iniWriter.WriteLine("Col3=Pressure Double");
                    iniWriter.WriteLine("Col4=Humidity Double");
                    iniWriter.WriteLine("Col5=Speed Double");
                    iniWriter.WriteLine("Col6=Direction Double");
                    iniWriter.Flush();
                }
            }
            var readings = _dataStore.GetReadings(
                comboChooseSensor.SelectedText,
                dateTimeRangePicker.Min,
                dateTimeRangePicker.Max.Subtract(dateTimeRangePicker.Min)
            );
            int c = 0;
            using (var csvStream = File.Open(csvFileName, FileMode.Create, FileAccess.ReadWrite)) {
                using (var csvWriter = new StreamWriter(csvStream)) {

					WriteQuoted(csvWriter, "Stamp");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Temperature");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Pressure");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Humidity");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Speed");
					csvWriter.Write(',');
					WriteQuoted(csvWriter, "Direction");
					csvWriter.WriteLine();

                    foreach (var reading in readings) {
                        c++;
						WriteQuoted(csvWriter,reading.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss"));
                        csvWriter.Write(',');
						WriteQuoted(csvWriter,reading.Temperature.ToString());
                        csvWriter.Write(',');
						WriteQuoted(csvWriter,reading.Pressure.ToString());
                        csvWriter.Write(',');
						WriteQuoted(csvWriter,reading.Humidity.ToString());
                        csvWriter.Write(',');
						WriteQuoted(csvWriter,reading.WindSpeed.ToString());
                        csvWriter.Write(',');
						WriteQuoted(csvWriter,reading.WindDirection.ToString());
						csvWriter.WriteLine();
                    }
                    csvWriter.Flush();
                }
            }
            return c > 0;
			*/
    	}
    }
}