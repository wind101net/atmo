using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Atmo.Data;
using System.Linq;
using System.IO;

namespace Atmo.UI.DevEx {
    public partial class ExportForm : DevExpress.XtraEditors.XtraForm {

        private IDataStore _dataStore;

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
        }
    }
}