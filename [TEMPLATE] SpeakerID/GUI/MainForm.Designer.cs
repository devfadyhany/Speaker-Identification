namespace Recorder
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Signals Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrain1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.chart = new Accord.Controls.Wavechart();
            this.lbPosition = new System.Windows.Forms.Label();
            this.lbLength = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnIdentify = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.togglePruningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.Label_pruning = new System.Windows.Forms.Label();
            this.clearMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(404, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.clearMemoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrain1ToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.togglePruningToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(49, 26);
            this.optionsToolStripMenuItem.Text = "Edit";
            // 
            // loadTrain1ToolStripMenuItem
            // 
            this.loadTrain1ToolStripMenuItem.Name = "loadTrain1ToolStripMenuItem";
            this.loadTrain1ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.loadTrain1ToolStripMenuItem.Text = "Load Train1";
            this.loadTrain1ToolStripMenuItem.Click += new System.EventHandler(this.loadTrain1ToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.loadToolStripMenuItem.Text = "Load Sample Train";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Wave files (*.wav)|*.wav";
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnStop.Location = new System.Drawing.Point(168, 134);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(69, 38);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "<";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecord.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnRecord.Location = new System.Drawing.Point(320, 134);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(69, 38);
            this.btnRecord.TabIndex = 4;
            this.btnRecord.Text = "=";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Black;
            this.chart.ForeColor = System.Drawing.Color.DarkGreen;
            this.chart.Location = new System.Drawing.Point(112, 34);
            this.chart.Margin = new System.Windows.Forms.Padding(4);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.DoubleRange)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.DoubleRange)(resources.GetObject("chart.RangeY")));
            this.chart.SimpleMode = false;
            this.chart.Size = new System.Drawing.Size(179, 51);
            this.chart.TabIndex = 6;
            this.chart.Text = "chart1";
            // 
            // lbPosition
            // 
            this.lbPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPosition.Location = new System.Drawing.Point(15, 34);
            this.lbPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(90, 51);
            this.lbPosition.TabIndex = 7;
            this.lbPosition.Text = "Position: 00.00 sec.";
            this.lbPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLength
            // 
            this.lbLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbLength.Location = new System.Drawing.Point(299, 34);
            this.lbLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(90, 51);
            this.lbLength.TabIndex = 7;
            this.lbLength.Text = "Length: 00.00 sec.";
            this.lbLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnAdd.Location = new System.Drawing.Point(15, 134);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(69, 38);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "a";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPlay.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlay.Location = new System.Drawing.Point(244, 134);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(69, 38);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "4";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnIdentify
            // 
            this.btnIdentify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIdentify.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnIdentify.Location = new System.Drawing.Point(91, 134);
            this.btnIdentify.Margin = new System.Windows.Forms.Padding(4);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(69, 38);
            this.btnIdentify.TabIndex = 4;
            this.btnIdentify.Text = "s";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(15, 92);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(374, 56);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "wav";
            this.saveFileDialog1.FileName = "file.wav";
            this.saveFileDialog1.Filter = "Wave files|*.wav|All files|*.*";
            this.saveFileDialog1.Title = "Save wave file";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // togglePruningToolStripMenuItem
            // 
            this.togglePruningToolStripMenuItem.Name = "togglePruningToolStripMenuItem";
            this.togglePruningToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.togglePruningToolStripMenuItem.Text = "Toggle Pruning";
            this.togglePruningToolStripMenuItem.Click += new System.EventHandler(this.togglePruningToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Use Pruning :";
            // 
            // Label_pruning
            // 
            this.Label_pruning.AutoSize = true;
            this.Label_pruning.ForeColor = System.Drawing.Color.Green;
            this.Label_pruning.Location = new System.Drawing.Point(99, 182);
            this.Label_pruning.Name = "Label_pruning";
            this.Label_pruning.Size = new System.Drawing.Size(38, 17);
            this.Label_pruning.TabIndex = 10;
            this.Label_pruning.Text = "True";
            this.Label_pruning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clearMemoryToolStripMenuItem
            // 
            this.clearMemoryToolStripMenuItem.Name = "clearMemoryToolStripMenuItem";
            this.clearMemoryToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.clearMemoryToolStripMenuItem.Text = "Clear Memory";
            this.clearMemoryToolStripMenuItem.Click += new System.EventHandler(this.clearMemoryToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(404, 207);
            this.Controls.Add(this.Label_pruning);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnIdentify);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.trackBar1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Speaker Identification";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private Accord.Controls.Wavechart chart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label lbPosition;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.ToolStripMenuItem loadTrain1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem togglePruningToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Label_pruning;
        private System.Windows.Forms.ToolStripMenuItem clearMemoryToolStripMenuItem;
    }
}
