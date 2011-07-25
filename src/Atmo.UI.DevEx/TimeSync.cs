using System;
using System.Windows.Forms;
using Atmo.Data;

namespace Atmo.UI.DevEx {
    public partial class TimeSync : DevExpress.XtraEditors.XtraForm {

        private readonly IDaqConnection _device;
        private readonly IDataStore _store;

		public TimeSync(IDaqConnection device, IDataStore store) {
            _device = device;
            _store = store;
            InitializeComponent();
        }

        private void syncButton_Click(object sender, EventArgs e) {
            DateTime cpuTime = DateTime.Now;
            bool clockOk = _device.SetClock(cpuTime);
            /*bool noData = false;
            if (clockOk && chkAdjust.Checked) {
                DateTime maxSyncStamp = _store.GetMaxSyncStamp().AddSeconds(1.0);
                if (default(DateTime).Equals(maxSyncStamp)) {
                    noData = true;
                }else{
                    DateTime daqTime = _device.QueryClock();
                    foreach (ISensorInfo si in _store.GetAllSensorInfos()) {
                        _store.AdjustTimeStamps(si.Name, new TimeRange(maxSyncStamp, daqTime), new TimeRange(maxSyncStamp, cpuTime));
                    }
                    _store.PushSyncStamp(cpuTime);
                }
            }*/

			if (clockOk) {
				MessageBox.Show(
					"DAQ clock syncronization was successful.",
					"Success",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			else {
				MessageBox.Show(
					"DAQ clock failed to synchronize.",
					"Notice",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}

            /*if (!clockOk) {
                MessageBox.Show(
                    "DAQ clock failed to synchronize.",
                    "Notice",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else if (noData) {
                MessageBox.Show(
                    "No data was found to be adjusted but DAQ clock syncronization was successful.",
                    "Notice",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else {
                MessageBox.Show(
                    "Data adjustment and DAQ clock syncronization was successful.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }*/
            
        }

        private void TimeSync_Load(object sender, EventArgs e) {
            /*if (
                DialogResult.OK != MessageBox.Show(
                    "It is recomended that you synchronize the time through the import tool unless this is the first time this device has been synchronized after being powered on.",
                    "Continue?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                )
            ) {
                Close();
            }*/
        }
    }
}