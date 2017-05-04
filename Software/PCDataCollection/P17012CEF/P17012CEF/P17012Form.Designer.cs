namespace P17012CEF
{
    partial class P17012Form
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
            this.BrowserContainer = new System.Windows.Forms.Panel();
            this.ControlsContainer = new System.Windows.Forms.Panel();
            this.MinPanel = new System.Windows.Forms.Panel();
            this.MinNumeric = new System.Windows.Forms.NumericUpDown();
            this.MinLabel = new System.Windows.Forms.Label();
            this.MaxPanel = new System.Windows.Forms.Panel();
            this.MaxNumeric = new System.Windows.Forms.NumericUpDown();
            this.MaxLabel = new System.Windows.Forms.Label();
            this.OrientationPanel = new System.Windows.Forms.Panel();
            this.OrieNumeric = new System.Windows.Forms.NumericUpDown();
            this.OrientationLabel = new System.Windows.Forms.Label();
            this.FlexionPanel = new System.Windows.Forms.Panel();
            this.FlexNumeric = new System.Windows.Forms.NumericUpDown();
            this.FlexionLabel = new System.Windows.Forms.Label();
            this.ForcePanel = new System.Windows.Forms.Panel();
            this.ForceLabel = new System.Windows.Forms.Label();
            this.ForceNumeric = new System.Windows.Forms.NumericUpDown();
            this.UpdateBrowser = new System.Windows.Forms.Button();
            this.ControlsContainer.SuspendLayout();
            this.MinPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinNumeric)).BeginInit();
            this.MaxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxNumeric)).BeginInit();
            this.OrientationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrieNumeric)).BeginInit();
            this.FlexionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlexNumeric)).BeginInit();
            this.ForcePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ForceNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowserContainer
            // 
            this.BrowserContainer.Location = new System.Drawing.Point(0, 0);
            this.BrowserContainer.Name = "BrowserContainer";
            this.BrowserContainer.Size = new System.Drawing.Size(311, 126);
            this.BrowserContainer.TabIndex = 0;
            // 
            // ControlsContainer
            // 
            this.ControlsContainer.Controls.Add(this.MinPanel);
            this.ControlsContainer.Controls.Add(this.MaxPanel);
            this.ControlsContainer.Controls.Add(this.OrientationPanel);
            this.ControlsContainer.Controls.Add(this.FlexionPanel);
            this.ControlsContainer.Controls.Add(this.ForcePanel);
            this.ControlsContainer.Controls.Add(this.UpdateBrowser);
            this.ControlsContainer.Location = new System.Drawing.Point(12, 147);
            this.ControlsContainer.Name = "ControlsContainer";
            this.ControlsContainer.Size = new System.Drawing.Size(449, 310);
            this.ControlsContainer.TabIndex = 2;
            // 
            // MinPanel
            // 
            this.MinPanel.Controls.Add(this.MinNumeric);
            this.MinPanel.Controls.Add(this.MinLabel);
            this.MinPanel.Location = new System.Drawing.Point(25, 61);
            this.MinPanel.Name = "MinPanel";
            this.MinPanel.Size = new System.Drawing.Size(181, 71);
            this.MinPanel.TabIndex = 10;
            this.MinPanel.Tag = "0";
            // 
            // MinNumeric
            // 
            this.MinNumeric.Location = new System.Drawing.Point(3, 30);
            this.MinNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.MinNumeric.Name = "MinNumeric";
            this.MinNumeric.Size = new System.Drawing.Size(120, 20);
            this.MinNumeric.TabIndex = 6;
            // 
            // MinLabel
            // 
            this.MinLabel.AutoSize = true;
            this.MinLabel.Location = new System.Drawing.Point(16, 14);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Size = new System.Drawing.Size(54, 13);
            this.MinLabel.TabIndex = 8;
            this.MinLabel.Text = "Min Force";
            // 
            // MaxPanel
            // 
            this.MaxPanel.Controls.Add(this.MaxNumeric);
            this.MaxPanel.Controls.Add(this.MaxLabel);
            this.MaxPanel.Location = new System.Drawing.Point(224, 47);
            this.MaxPanel.Name = "MaxPanel";
            this.MaxPanel.Size = new System.Drawing.Size(181, 71);
            this.MaxPanel.TabIndex = 9;
            this.MaxPanel.Tag = "1";
            // 
            // MaxNumeric
            // 
            this.MaxNumeric.Location = new System.Drawing.Point(3, 30);
            this.MaxNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.MaxNumeric.Name = "MaxNumeric";
            this.MaxNumeric.Size = new System.Drawing.Size(120, 20);
            this.MaxNumeric.TabIndex = 6;
            // 
            // MaxLabel
            // 
            this.MaxLabel.AutoSize = true;
            this.MaxLabel.Location = new System.Drawing.Point(16, 14);
            this.MaxLabel.Name = "MaxLabel";
            this.MaxLabel.Size = new System.Drawing.Size(57, 13);
            this.MaxLabel.TabIndex = 8;
            this.MaxLabel.Text = "Max Force";
            // 
            // OrientationPanel
            // 
            this.OrientationPanel.Controls.Add(this.OrieNumeric);
            this.OrientationPanel.Controls.Add(this.OrientationLabel);
            this.OrientationPanel.Location = new System.Drawing.Point(70, 234);
            this.OrientationPanel.Name = "OrientationPanel";
            this.OrientationPanel.Size = new System.Drawing.Size(184, 53);
            this.OrientationPanel.TabIndex = 9;
            this.OrientationPanel.Tag = "4";
            // 
            // OrieNumeric
            // 
            this.OrieNumeric.Location = new System.Drawing.Point(16, 21);
            this.OrieNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.OrieNumeric.Name = "OrieNumeric";
            this.OrieNumeric.Size = new System.Drawing.Size(120, 20);
            this.OrieNumeric.TabIndex = 5;
            // 
            // OrientationLabel
            // 
            this.OrientationLabel.AutoSize = true;
            this.OrientationLabel.Location = new System.Drawing.Point(31, 5);
            this.OrientationLabel.Name = "OrientationLabel";
            this.OrientationLabel.Size = new System.Drawing.Size(58, 13);
            this.OrientationLabel.TabIndex = 8;
            this.OrientationLabel.Text = "Orientation";
            // 
            // FlexionPanel
            // 
            this.FlexionPanel.Controls.Add(this.FlexNumeric);
            this.FlexionPanel.Controls.Add(this.FlexionLabel);
            this.FlexionPanel.Location = new System.Drawing.Point(224, 143);
            this.FlexionPanel.Name = "FlexionPanel";
            this.FlexionPanel.Size = new System.Drawing.Size(181, 71);
            this.FlexionPanel.TabIndex = 8;
            this.FlexionPanel.Tag = "3";
            // 
            // FlexNumeric
            // 
            this.FlexNumeric.Location = new System.Drawing.Point(3, 30);
            this.FlexNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.FlexNumeric.Name = "FlexNumeric";
            this.FlexNumeric.Size = new System.Drawing.Size(120, 20);
            this.FlexNumeric.TabIndex = 6;
            // 
            // FlexionLabel
            // 
            this.FlexionLabel.AutoSize = true;
            this.FlexionLabel.Location = new System.Drawing.Point(16, 14);
            this.FlexionLabel.Name = "FlexionLabel";
            this.FlexionLabel.Size = new System.Drawing.Size(40, 13);
            this.FlexionLabel.TabIndex = 8;
            this.FlexionLabel.Text = "Flexion";
            // 
            // ForcePanel
            // 
            this.ForcePanel.Controls.Add(this.ForceLabel);
            this.ForcePanel.Controls.Add(this.ForceNumeric);
            this.ForcePanel.Location = new System.Drawing.Point(21, 143);
            this.ForcePanel.Name = "ForcePanel";
            this.ForcePanel.Size = new System.Drawing.Size(154, 71);
            this.ForcePanel.TabIndex = 9;
            this.ForcePanel.Tag = "2";
            // 
            // ForceLabel
            // 
            this.ForceLabel.AutoSize = true;
            this.ForceLabel.Location = new System.Drawing.Point(15, 14);
            this.ForceLabel.Name = "ForceLabel";
            this.ForceLabel.Size = new System.Drawing.Size(74, 13);
            this.ForceLabel.TabIndex = 7;
            this.ForceLabel.Text = "Force Percent";
            // 
            // ForceNumeric
            // 
            this.ForceNumeric.Location = new System.Drawing.Point(18, 30);
            this.ForceNumeric.Name = "ForceNumeric";
            this.ForceNumeric.Size = new System.Drawing.Size(120, 20);
            this.ForceNumeric.TabIndex = 4;
            // 
            // UpdateBrowser
            // 
            this.UpdateBrowser.Location = new System.Drawing.Point(19, 23);
            this.UpdateBrowser.Name = "UpdateBrowser";
            this.UpdateBrowser.Size = new System.Drawing.Size(75, 23);
            this.UpdateBrowser.TabIndex = 0;
            this.UpdateBrowser.Tag = "5";
            this.UpdateBrowser.Text = "Update";
            this.UpdateBrowser.UseVisualStyleBackColor = true;
            this.UpdateBrowser.Click += new System.EventHandler(this.UpdateBrowser_Click);
            // 
            // P17012Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 588);
            this.Controls.Add(this.ControlsContainer);
            this.Controls.Add(this.BrowserContainer);
            this.Name = "P17012Form";
            this.Text = "Knee Position";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P17012Form_FormClosing);
            this.Load += new System.EventHandler(this.P17012Form_Load);
            this.Resize += new System.EventHandler(this.P17012Form_Resize);
            this.ControlsContainer.ResumeLayout(false);
            this.MinPanel.ResumeLayout(false);
            this.MinPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinNumeric)).EndInit();
            this.MaxPanel.ResumeLayout(false);
            this.MaxPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxNumeric)).EndInit();
            this.OrientationPanel.ResumeLayout(false);
            this.OrientationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrieNumeric)).EndInit();
            this.FlexionPanel.ResumeLayout(false);
            this.FlexionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlexNumeric)).EndInit();
            this.ForcePanel.ResumeLayout(false);
            this.ForcePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ForceNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BrowserContainer;
        private System.Windows.Forms.Panel ControlsContainer;
        private System.Windows.Forms.Button UpdateBrowser;
        private System.Windows.Forms.NumericUpDown FlexNumeric;
        private System.Windows.Forms.NumericUpDown OrieNumeric;
        private System.Windows.Forms.NumericUpDown ForceNumeric;
        private System.Windows.Forms.Label OrientationLabel;
        private System.Windows.Forms.Label FlexionLabel;
        private System.Windows.Forms.Label ForceLabel;
        private System.Windows.Forms.Panel OrientationPanel;
        private System.Windows.Forms.Panel FlexionPanel;
        private System.Windows.Forms.Panel ForcePanel;
        private System.Windows.Forms.Panel MinPanel;
        private System.Windows.Forms.NumericUpDown MinNumeric;
        private System.Windows.Forms.Label MinLabel;
        private System.Windows.Forms.Panel MaxPanel;
        private System.Windows.Forms.NumericUpDown MaxNumeric;
        private System.Windows.Forms.Label MaxLabel;
    }
}

