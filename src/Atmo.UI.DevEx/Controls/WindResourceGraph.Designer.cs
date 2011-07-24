namespace Atmo.UI.DevEx.Controls {
	partial class WindResourceGraph {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
			DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.SplineAreaSeriesView splineAreaSeriesView1 = new DevExpress.XtraCharts.SplineAreaSeriesView();
			DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
			DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
			DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.SplineSeriesView splineSeriesView1 = new DevExpress.XtraCharts.SplineSeriesView();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.SplineAreaSeriesView splineAreaSeriesView2 = new DevExpress.XtraCharts.SplineAreaSeriesView();
			DevExpress.XtraCharts.PolarDiagram polarDiagram1 = new DevExpress.XtraCharts.PolarDiagram();
			DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.RadarPointSeriesLabel radarPointSeriesLabel1 = new DevExpress.XtraCharts.RadarPointSeriesLabel();
			DevExpress.XtraCharts.PolarAreaSeriesView polarAreaSeriesView1 = new DevExpress.XtraCharts.PolarAreaSeriesView();
			DevExpress.XtraCharts.Series series5 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.RadarPointSeriesLabel radarPointSeriesLabel2 = new DevExpress.XtraCharts.RadarPointSeriesLabel();
			DevExpress.XtraCharts.PolarAreaSeriesView polarAreaSeriesView2 = new DevExpress.XtraCharts.PolarAreaSeriesView();
			DevExpress.XtraCharts.RadarPointSeriesLabel radarPointSeriesLabel3 = new DevExpress.XtraCharts.RadarPointSeriesLabel();
			DevExpress.XtraCharts.PolarAreaSeriesView polarAreaSeriesView3 = new DevExpress.XtraCharts.PolarAreaSeriesView();
			this.bindingSourceWindSpeedFreq = new System.Windows.Forms.BindingSource(this.components);
			this.chartControlWindSpeedFreq = new DevExpress.XtraCharts.ChartControl();
			this.chartControlWindDir = new DevExpress.XtraCharts.ChartControl();
			this.bindingSourceWindDir = new System.Windows.Forms.BindingSource(this.components);
			this.groupControl = new DevExpress.XtraEditors.GroupControl();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceWindSpeedFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControlWindSpeedFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(splineAreaSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(splineSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(splineAreaSeriesView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControlWindDir)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(polarDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceWindDir)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
			this.groupControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// bindingSourceWindSpeedFreq
			// 
			this.bindingSourceWindSpeedFreq.DataSource = typeof(Atmo.Stats.WindSpeedFrequency);
			// 
			// chartControlWindSpeedFreq
			// 
			this.chartControlWindSpeedFreq.AppearanceName = "The Trees";
			this.chartControlWindSpeedFreq.DataSource = this.bindingSourceWindSpeedFreq;
			xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
			xyDiagram1.AxisX.ScaleBreakOptions.Style = DevExpress.XtraCharts.ScaleBreakStyle.Straight;
			xyDiagram1.AxisX.Tickmarks.MinorVisible = false;
			xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = false;
			xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
			xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
			secondaryAxisY1.AxisID = 0;
			secondaryAxisY1.Name = "Secondary AxisY 1";
			secondaryAxisY1.Range.ScrollingRange.SideMarginsEnabled = true;
			secondaryAxisY1.Range.SideMarginsEnabled = true;
			secondaryAxisY1.VisibleInPanesSerializable = "-1";
			xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
			this.chartControlWindSpeedFreq.Diagram = xyDiagram1;
			this.chartControlWindSpeedFreq.Dock = System.Windows.Forms.DockStyle.Top;
			this.chartControlWindSpeedFreq.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Right;
			this.chartControlWindSpeedFreq.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
			this.chartControlWindSpeedFreq.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
			this.chartControlWindSpeedFreq.Legend.EquallySpacedItems = false;
			this.chartControlWindSpeedFreq.Legend.Shadow.Visible = true;
			this.chartControlWindSpeedFreq.Location = new System.Drawing.Point(2, 22);
			this.chartControlWindSpeedFreq.Name = "chartControlWindSpeedFreq";
			series1.ArgumentDataMember = "SpeedPropertty";
			series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			pointSeriesLabel1.LineVisible = true;
			pointSeriesLabel1.Visible = false;
			series1.Label = pointSeriesLabel1;
			series1.Name = "Wind Power";
			series1.ValueDataMembersSerializable = "Power";
			splineAreaSeriesView1.AxisYName = "Secondary AxisY 1";
			splineAreaSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(64)))));
			splineAreaSeriesView1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
			splineAreaSeriesView1.MarkerOptions.Visible = false;
			series1.View = splineAreaSeriesView1;
			series2.ArgumentDataMember = "SpeedPropertty";
			series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			series2.DataFilters.ClearAndAddRange(new DevExpress.XtraCharts.DataFilter[] {
            new DevExpress.XtraCharts.DataFilter("SpeedPropertty", "System.Double", DevExpress.XtraCharts.DataFilterCondition.NotEqual, 0D)});
			sideBySideBarSeriesLabel1.LineVisible = true;
			sideBySideBarSeriesLabel1.Visible = false;
			series2.Label = sideBySideBarSeriesLabel1;
			series2.Name = "Wind Speed Frequency";
			series2.ValueDataMembersSerializable = "FrequencyProperty";
			sideBySideBarSeriesView1.BarWidth = 1D;
			sideBySideBarSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(96)))), ((int)(((byte)(128)))), ((int)(((byte)(228)))));
			sideBySideBarSeriesView1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
			sideBySideBarSeriesView1.Transparency = ((byte)(135));
			series2.View = sideBySideBarSeriesView1;
			series3.ArgumentDataMember = "SpeedPropertty";
			series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			pointSeriesLabel2.LineVisible = true;
			pointSeriesLabel2.Visible = false;
			series3.Label = pointSeriesLabel2;
			series3.LegendText = "Beta: NaN Theta: NaN";
			series3.Name = "Weibull";
			series3.ValueDataMembersSerializable = "WeibullProperty";
			splineSeriesView1.Color = System.Drawing.Color.Red;
			splineSeriesView1.LineMarkerOptions.Visible = false;
			splineSeriesView1.LineStyle.Thickness = 1;
			series3.View = splineSeriesView1;
			this.chartControlWindSpeedFreq.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
			pointSeriesLabel3.LineVisible = true;
			this.chartControlWindSpeedFreq.SeriesTemplate.Label = pointSeriesLabel3;
			splineAreaSeriesView2.Transparency = ((byte)(0));
			this.chartControlWindSpeedFreq.SeriesTemplate.View = splineAreaSeriesView2;
			this.chartControlWindSpeedFreq.SideBySideBarDistanceFixed = 0;
			this.chartControlWindSpeedFreq.SideBySideEqualBarWidth = true;
			this.chartControlWindSpeedFreq.Size = new System.Drawing.Size(894, 203);
			this.chartControlWindSpeedFreq.TabIndex = 0;
			// 
			// chartControlWindDir
			// 
			polarDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
			polarDiagram1.AxisY.Range.SideMarginsEnabled = true;
			polarDiagram1.RotationDirection = DevExpress.XtraCharts.RadarDiagramRotationDirection.Clockwise;
			this.chartControlWindDir.Diagram = polarDiagram1;
			this.chartControlWindDir.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControlWindDir.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Left;
			this.chartControlWindDir.Legend.HorizontalIndent = 2;
			this.chartControlWindDir.Legend.Padding.Bottom = 1;
			this.chartControlWindDir.Legend.Padding.Left = 1;
			this.chartControlWindDir.Legend.Padding.Right = 1;
			this.chartControlWindDir.Legend.Padding.Top = 1;
			this.chartControlWindDir.Legend.Shadow.Visible = true;
			this.chartControlWindDir.Legend.TextOffset = 1;
			this.chartControlWindDir.Legend.VerticalIndent = 1;
			this.chartControlWindDir.Location = new System.Drawing.Point(2, 225);
			this.chartControlWindDir.Name = "chartControlWindDir";
			series4.ArgumentDataMember = "DirectionProperty";
			series4.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			radarPointSeriesLabel1.LineVisible = true;
			radarPointSeriesLabel1.Visible = false;
			series4.Label = radarPointSeriesLabel1;
			series4.Name = "Power From Direction";
			series4.ValueDataMembersSerializable = "PowerProperty";
			polarAreaSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(64)))));
			polarAreaSeriesView1.MarkerOptions.Visible = false;
			series4.View = polarAreaSeriesView1;
			series5.ArgumentDataMember = "DirectionProperty";
			series5.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			radarPointSeriesLabel2.LineVisible = true;
			radarPointSeriesLabel2.Visible = false;
			series5.Label = radarPointSeriesLabel2;
			series5.Name = "Wind From Direction";
			series5.ValueDataMembersSerializable = "FrequencyProperty";
			polarAreaSeriesView2.Border.Visible = false;
			polarAreaSeriesView2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(96)))), ((int)(((byte)(128)))), ((int)(((byte)(228)))));
			polarAreaSeriesView2.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
			polarAreaSeriesView2.MarkerOptions.Visible = false;
			series5.View = polarAreaSeriesView2;
			this.chartControlWindDir.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series4,
        series5};
			this.chartControlWindDir.SeriesTemplate.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
			radarPointSeriesLabel3.LineVisible = true;
			this.chartControlWindDir.SeriesTemplate.Label = radarPointSeriesLabel3;
			polarAreaSeriesView3.Transparency = ((byte)(0));
			this.chartControlWindDir.SeriesTemplate.View = polarAreaSeriesView3;
			this.chartControlWindDir.Size = new System.Drawing.Size(894, 652);
			this.chartControlWindDir.TabIndex = 1;
			// 
			// bindingSourceWindDir
			// 
			this.bindingSourceWindDir.DataSource = typeof(Atmo.Stats.WindDirectionEnergy);
			// 
			// groupControl
			// 
			this.groupControl.Controls.Add(this.chartControlWindDir);
			this.groupControl.Controls.Add(this.chartControlWindSpeedFreq);
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupControl.Location = new System.Drawing.Point(0, 0);
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(898, 879);
			this.groupControl.TabIndex = 2;
			this.groupControl.Text = "Wind Resource Analysis";
			// 
			// WindResourceGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl);
			this.Name = "WindResourceGraph";
			this.Size = new System.Drawing.Size(898, 879);
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceWindSpeedFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(splineAreaSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(splineSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(splineAreaSeriesView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControlWindSpeedFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(polarDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(radarPointSeriesLabel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(polarAreaSeriesView3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControlWindDir)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSourceWindDir)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
			this.groupControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraCharts.ChartControl chartControlWindSpeedFreq;
		private System.Windows.Forms.BindingSource bindingSourceWindSpeedFreq;
		private DevExpress.XtraCharts.ChartControl chartControlWindDir;
		private System.Windows.Forms.BindingSource bindingSourceWindDir;
		private DevExpress.XtraEditors.GroupControl groupControl;
	}
}
