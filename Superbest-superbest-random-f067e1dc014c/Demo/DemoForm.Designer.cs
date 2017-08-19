namespace Demo
{
    partial class DemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_execute = new System.Windows.Forms.Button();
            this.demo_picker = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
            this.SuspendLayout();
            // 
            // graph
            // 
            this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea5.Name = "ChartArea1";
            this.graph.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.graph.Legends.Add(legend5);
            this.graph.Location = new System.Drawing.Point(12, 42);
            this.graph.Name = "graph";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.graph.Series.Add(series5);
            this.graph.Size = new System.Drawing.Size(729, 501);
            this.graph.TabIndex = 0;
            this.graph.Text = "chart1";
            // 
            // btn_execute
            // 
            this.btn_execute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_execute.Location = new System.Drawing.Point(666, 13);
            this.btn_execute.Name = "btn_execute";
            this.btn_execute.Size = new System.Drawing.Size(75, 23);
            this.btn_execute.TabIndex = 2;
            this.btn_execute.Text = "Do it!";
            this.btn_execute.UseVisualStyleBackColor = true;
            this.btn_execute.Click += new System.EventHandler(this.DemoRequested);
            // 
            // demo_picker
            // 
            this.demo_picker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.demo_picker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.demo_picker.FormattingEnabled = true;
            this.demo_picker.Items.AddRange(new object[] {
            "Gaussian - distribution",
            "Gaussian - performance (not yet implemented)",
            "Triangular - distribution",
            "Triangular - performance (not yet implemented)",
            "Boolean - distribution",
            "Boolean - performance (not yet implemented)",
            "Shuffle list - performance (not yet implemented)",
            "Random permutations - performance"});
            this.demo_picker.Location = new System.Drawing.Point(13, 13);
            this.demo_picker.Name = "demo_picker";
            this.demo_picker.Size = new System.Drawing.Size(647, 21);
            this.demo_picker.TabIndex = 1;
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 555);
            this.Controls.Add(this.btn_execute);
            this.Controls.Add(this.demo_picker);
            this.Controls.Add(this.graph);
            this.Name = "DemoForm";
            this.Text = "Superbest random demo";
            ((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart graph;
        private System.Windows.Forms.Button btn_execute;
        private System.Windows.Forms.ComboBox demo_picker;
    }
}

