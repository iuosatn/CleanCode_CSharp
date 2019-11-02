namespace CleanCode
{
    partial class CleanCode
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
            this.ButtonRun = new System.Windows.Forms.Button();
            this.FilePath = new System.Windows.Forms.TextBox();
            this.GetFile = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonRun
            // 
            this.ButtonRun.Location = new System.Drawing.Point(280, 78);
            this.ButtonRun.Name = "ButtonRun";
            this.ButtonRun.Size = new System.Drawing.Size(227, 29);
            this.ButtonRun.TabIndex = 0;
            this.ButtonRun.Text = "Run";
            this.ButtonRun.UseVisualStyleBackColor = true;
            this.ButtonRun.Click += new System.EventHandler(this.Run);
            // 
            // FilePath
            // 
            this.FilePath.Location = new System.Drawing.Point(31, 12);
            this.FilePath.Name = "FilePath";
            this.FilePath.Size = new System.Drawing.Size(624, 20);
            this.FilePath.TabIndex = 1;
            // 
            // GetFile
            // 
            this.GetFile.Location = new System.Drawing.Point(674, 9);
            this.GetFile.Name = "GetFile";
            this.GetFile.Size = new System.Drawing.Size(75, 23);
            this.GetFile.TabIndex = 2;
            this.GetFile.Text = "GetFile";
            this.GetFile.UseVisualStyleBackColor = true;
            this.GetFile.Click += new System.EventHandler(this.Get);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 125);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(765, 347);
            this.dataGridView.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(31, 39);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(718, 33);
            this.progressBar.TabIndex = 4;
            // 
            // CleanCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(789, 484);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.GetFile);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.ButtonRun);
            this.Name = "CleanCode";
            this.Text = "CleanCode";
            this.Load += new System.EventHandler(this.lade);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonRun;
        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Button GetFile;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

