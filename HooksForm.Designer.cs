namespace WindowsHooks
{
    partial class HooksForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.textBoxLocalEvents = new System.Windows.Forms.TextBox();
            this.labelPosLocal = new System.Windows.Forms.Label();
            this.buttonStopLocal = new System.Windows.Forms.Button();
            this.buttonStartLocal = new System.Windows.Forms.Button();
            this.groupBoxGlobal = new System.Windows.Forms.GroupBox();
            this.textBoxGlobalEvents = new System.Windows.Forms.TextBox();
            this.labelPosGlobal = new System.Windows.Forms.Label();
            this.buttonStopGlobal = new System.Windows.Forms.Button();
            this.buttonStartGlobal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxLocal.SuspendLayout();
            this.groupBoxGlobal.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxLocal);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxGlobal);
            this.splitContainer1.Size = new System.Drawing.Size(721, 580);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 0;
            //
            // groupBoxLocal
            //
            this.groupBoxLocal.Controls.Add(this.textBoxLocalEvents);
            this.groupBoxLocal.Controls.Add(this.labelPosLocal);
            this.groupBoxLocal.Controls.Add(this.buttonStopLocal);
            this.groupBoxLocal.Controls.Add(this.buttonStartLocal);
            this.groupBoxLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLocal.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(352, 580);
            this.groupBoxLocal.TabIndex = 0;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local Hooks";
            //
            // textBoxLocalEvents
            //
            this.textBoxLocalEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocalEvents.Enabled = false;
            this.textBoxLocalEvents.Location = new System.Drawing.Point(6, 61);
            this.textBoxLocalEvents.Multiline = true;
            this.textBoxLocalEvents.Name = "textBoxLocalEvents";
            this.textBoxLocalEvents.Size = new System.Drawing.Size(343, 513);
            this.textBoxLocalEvents.TabIndex = 5;
            //
            // labelPosLocal
            //
            this.labelPosLocal.AutoSize = true;
            this.labelPosLocal.Location = new System.Drawing.Point(3, 45);
            this.labelPosLocal.Name = "labelPosLocal";
            this.labelPosLocal.Size = new System.Drawing.Size(108, 13);
            this.labelPosLocal.TabIndex = 3;
            this.labelPosLocal.Text = "Mouse Local Position";
            //
            // buttonStopLocal
            //
            this.buttonStopLocal.Location = new System.Drawing.Point(84, 19);
            this.buttonStopLocal.Name = "buttonStopLocal";
            this.buttonStopLocal.Size = new System.Drawing.Size(75, 23);
            this.buttonStopLocal.TabIndex = 1;
            this.buttonStopLocal.Text = "Stop";
            this.buttonStopLocal.UseVisualStyleBackColor = true;
            this.buttonStopLocal.Click += new System.EventHandler(this.ButtonStopLocal_Click);
            //
            // buttonStartLocal
            //
            this.buttonStartLocal.Location = new System.Drawing.Point(3, 19);
            this.buttonStartLocal.Name = "buttonStartLocal";
            this.buttonStartLocal.Size = new System.Drawing.Size(75, 23);
            this.buttonStartLocal.TabIndex = 0;
            this.buttonStartLocal.Text = "Start";
            this.buttonStartLocal.UseVisualStyleBackColor = true;
            this.buttonStartLocal.Click += new System.EventHandler(this.ButtonStartLocal_Click);
            //
            // groupBoxGlobal
            //
            this.groupBoxGlobal.Controls.Add(this.textBoxGlobalEvents);
            this.groupBoxGlobal.Controls.Add(this.labelPosGlobal);
            this.groupBoxGlobal.Controls.Add(this.buttonStopGlobal);
            this.groupBoxGlobal.Controls.Add(this.buttonStartGlobal);
            this.groupBoxGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxGlobal.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGlobal.Name = "groupBoxGlobal";
            this.groupBoxGlobal.Size = new System.Drawing.Size(365, 580);
            this.groupBoxGlobal.TabIndex = 0;
            this.groupBoxGlobal.TabStop = false;
            this.groupBoxGlobal.Text = "Global Hooks";
            //
            // textBoxGlobalEvents
            //
            this.textBoxGlobalEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGlobalEvents.Enabled = false;
            this.textBoxGlobalEvents.Location = new System.Drawing.Point(3, 61);
            this.textBoxGlobalEvents.Multiline = true;
            this.textBoxGlobalEvents.Name = "textBoxGlobalEvents";
            this.textBoxGlobalEvents.Size = new System.Drawing.Size(356, 513);
            this.textBoxGlobalEvents.TabIndex = 6;
            //
            // labelPosGlobal
            //
            this.labelPosGlobal.AutoSize = true;
            this.labelPosGlobal.Location = new System.Drawing.Point(0, 45);
            this.labelPosGlobal.Name = "labelPosGlobal";
            this.labelPosGlobal.Size = new System.Drawing.Size(112, 13);
            this.labelPosGlobal.TabIndex = 4;
            this.labelPosGlobal.Text = "Mouse Global Position";
            //
            // buttonStopGlobal
            //
            this.buttonStopGlobal.Location = new System.Drawing.Point(84, 19);
            this.buttonStopGlobal.Name = "buttonStopGlobal";
            this.buttonStopGlobal.Size = new System.Drawing.Size(75, 23);
            this.buttonStopGlobal.TabIndex = 2;
            this.buttonStopGlobal.Text = "Stop";
            this.buttonStopGlobal.UseVisualStyleBackColor = true;
            this.buttonStopGlobal.Click += new System.EventHandler(this.ButtonStopGlobal_Click);
            //
            // buttonStartGlobal
            //
            this.buttonStartGlobal.Location = new System.Drawing.Point(3, 19);
            this.buttonStartGlobal.Name = "buttonStartGlobal";
            this.buttonStartGlobal.Size = new System.Drawing.Size(75, 23);
            this.buttonStartGlobal.TabIndex = 1;
            this.buttonStartGlobal.Text = "Start";
            this.buttonStartGlobal.UseVisualStyleBackColor = true;
            this.buttonStartGlobal.Click += new System.EventHandler(this.ButtonStartGlobal_Click);
            //
            // HooksForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 580);
            this.Controls.Add(this.splitContainer1);
            this.Name = "HooksForm";
            this.Text = "HooksForm";
            this.Load += new System.EventHandler(this.HooksForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.groupBoxGlobal.ResumeLayout(false);
            this.groupBoxGlobal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.GroupBox groupBoxGlobal;
        private System.Windows.Forms.Button buttonStopLocal;
        private System.Windows.Forms.Button buttonStartLocal;
        private System.Windows.Forms.Button buttonStopGlobal;
        private System.Windows.Forms.Button buttonStartGlobal;
        private System.Windows.Forms.TextBox textBoxLocalEvents;
        private System.Windows.Forms.Label labelPosLocal;
        private System.Windows.Forms.TextBox textBoxGlobalEvents;
        private System.Windows.Forms.Label labelPosGlobal;
    }
}

