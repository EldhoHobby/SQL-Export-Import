namespace SQL_Export_Import
{
    partial class ImportSummaryForm
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
            this.labelSummaryForm = new System.Windows.Forms.Label();
            this.dataGridViewSummary = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSummaryForm
            // 
            this.labelSummaryForm.AutoSize = true;
            this.labelSummaryForm.Location = new System.Drawing.Point(143, 69);
            this.labelSummaryForm.Name = "labelSummaryForm";
            this.labelSummaryForm.Size = new System.Drawing.Size(35, 13);
            this.labelSummaryForm.TabIndex = 0;
            this.labelSummaryForm.Text = "label1";
            // 
            // dataGridViewSummary
            // 
            this.dataGridViewSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSummary.Location = new System.Drawing.Point(27, 168);
            this.dataGridViewSummary.Name = "dataGridViewSummary";
            this.dataGridViewSummary.Size = new System.Drawing.Size(737, 150);
            this.dataGridViewSummary.TabIndex = 1;
            // 
            // ImportSummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewSummary);
            this.Controls.Add(this.labelSummaryForm);
            this.Name = "ImportSummaryForm";
            this.Text = "ImportSummaryForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSummaryForm;
        private System.Windows.Forms.DataGridView dataGridViewSummary;
    }
}