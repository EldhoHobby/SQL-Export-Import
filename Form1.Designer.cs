namespace SQL_Export_Import
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ImportSQLBtn = new System.Windows.Forms.Button();
            this.ExportSQLBtn = new System.Windows.Forms.Button();
            this.LoadSQLBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1069, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // ImportSQLBtn
            // 
            this.ImportSQLBtn.Location = new System.Drawing.Point(93, 240);
            this.ImportSQLBtn.Name = "ImportSQLBtn";
            this.ImportSQLBtn.Size = new System.Drawing.Size(75, 23);
            this.ImportSQLBtn.TabIndex = 1;
            this.ImportSQLBtn.Text = "Import";
            this.ImportSQLBtn.UseVisualStyleBackColor = true;
            this.ImportSQLBtn.Click += new System.EventHandler(this.ImportSQLBtn_Click);
            // 
            // ExportSQLBtn
            // 
            this.ExportSQLBtn.Location = new System.Drawing.Point(313, 240);
            this.ExportSQLBtn.Name = "ExportSQLBtn";
            this.ExportSQLBtn.Size = new System.Drawing.Size(75, 23);
            this.ExportSQLBtn.TabIndex = 2;
            this.ExportSQLBtn.Text = "Export";
            this.ExportSQLBtn.UseVisualStyleBackColor = true;
            this.ExportSQLBtn.Click += new System.EventHandler(this.ExportSQLBtn_Click);
            // 
            // LoadSQLBtn
            // 
            this.LoadSQLBtn.Location = new System.Drawing.Point(561, 239);
            this.LoadSQLBtn.Name = "LoadSQLBtn";
            this.LoadSQLBtn.Size = new System.Drawing.Size(75, 23);
            this.LoadSQLBtn.TabIndex = 3;
            this.LoadSQLBtn.Text = "Load";
            this.LoadSQLBtn.UseVisualStyleBackColor = true;
            this.LoadSQLBtn.Click += new System.EventHandler(this.LoadSQLBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 508);
            this.Controls.Add(this.LoadSQLBtn);
            this.Controls.Add(this.ExportSQLBtn);
            this.Controls.Add(this.ImportSQLBtn);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button ImportSQLBtn;
        private System.Windows.Forms.Button ExportSQLBtn;
        private System.Windows.Forms.Button LoadSQLBtn;
    }
}

