using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SQL_Export_Import
{
    public partial class ImportSummaryForm : Form
    {
        public ImportSummaryForm(int newlyAdded, int updated, int noChange, List<DataRow> summaryData)
        {
            InitializeComponent();
            labelSummaryForm.Text = $"Newly Added: {newlyAdded}\nUpdated: {updated}\nNo Change: {noChange}";

            DataTable summaryTable = new DataTable();
            summaryTable.Columns.Add("ItemNum", typeof(string));
            summaryTable.Columns.Add("ItemName", typeof(string));
            summaryTable.Columns.Add("ItemName_Extra", typeof(string));
            summaryTable.Columns.Add("Status", typeof(string)); // Ensure the Status column is added

            foreach (DataRow row in summaryData)
            {
                DataRow newRow = summaryTable.NewRow();
                newRow["ItemNum"] = row["ItemNum"];
                newRow["ItemName"] = row["ItemName"];
                newRow["ItemName_Extra"] = row["ItemName_Extra"];
                newRow["Status"] = row["Status"];
                summaryTable.Rows.Add(newRow);
            }

            dataGridViewSummary.DataSource = summaryTable;
        }
    }
}
