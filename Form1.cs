using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;

namespace SQL_Export_Import
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ImportSQLBtn_Click(object sender, EventArgs e)
        {
            int newlyAdded = 0, updated = 0, noChange = 0;
            List<DataRow> summaryData = new List<DataRow>();
            StringBuilder logBuilder = new StringBuilder();

            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV files|*.csv", Title = "Select a CSV file" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string csvFilePath = ofd.FileName;
                    DataTable dataTable = new DataTable();

                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                    using (var dr = new CsvDataReader(csv))
                    {
                        dataTable.Load(dr);
                    }

                    string connectionString = "Server=localhost\\SQLEXPRESS;Database=FOSTER_DB;User Id=FOSTER;Password=Foster1;";

                    using (SqlConnection sqlConn = new SqlConnection(connectionString))
                    {
                        sqlConn.Open();
                        int rowNumber = 1;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string itemNum = row["ItemNum"].ToString().Trim();

                            if (ItemExists(sqlConn, itemNum))
                            {
                                if (UpdateItem(sqlConn, row, out Dictionary<string, string> differences))
                                {
                                    updated++;
                                    summaryData.Add(CreateSummaryDataRow(row, "Updated"));
                                    logBuilder.AppendLine($"Row {rowNumber} - ItemNum: {itemNum}, ItemName: {row["ItemName"]}, ItemName_Extra: {row["ItemName_Extra"]} UPDATED");



                                    foreach (var diff in differences)
                                    {
                                        logBuilder.AppendLine($"  {diff.Key}: {diff.Value}");
                                    }
                                }
                                else
                                {
                                    noChange++;
                                    summaryData.Add(CreateSummaryDataRow(row, "No Change"));
                                }
                            }
                            else
                            {
                                InsertItem(sqlConn, row);
                                newlyAdded++;
                                summaryData.Add(CreateSummaryDataRow(row, "Newly Added"));
                                logBuilder.AppendLine($"Row {rowNumber} - ItemNum: {itemNum}, ItemName: {row["ItemName"]}, ItemName_Extra: {row["ItemName_Extra"]} ADDED");

                            }
                            rowNumber++;
                        }
                    }

                    string logDirectory = @"C:\\Users\\Public\\Documents\\DreamsLive\\Log";
                    string logFilePath = Path.Combine(logDirectory, "UpdateLog.txt");

                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    File.WriteAllText(logFilePath, logBuilder.ToString());
                    MessageBox.Show($"Log saved at: {logFilePath}");

                    ImportSummaryForm summaryForm = new ImportSummaryForm(newlyAdded, updated, noChange, summaryData);
                    summaryForm.ShowDialog();
                }
            }
        }

        private bool UpdateItem(SqlConnection sqlConn, DataRow newRow, out Dictionary<string, string> differences)
        {
            differences = new Dictionary<string, string>();

            string selectQuery = "SELECT * FROM dbo.products WHERE ItemNum = @ItemNum";
            using (SqlCommand selectCmd = new SqlCommand(selectQuery, sqlConn))
            {
                selectCmd.Parameters.AddWithValue("@ItemNum", newRow["ItemNum"].ToString().Trim());
                SqlDataAdapter adapter = new SqlDataAdapter(selectCmd);
                DataTable existingData = new DataTable();
                adapter.Fill(existingData);

                if (existingData.Rows.Count == 0)
                    return false;

                DataRow existingRow = existingData.Rows[0];
                bool isDifferent = false;

                foreach (DataColumn col in newRow.Table.Columns)
                {
                    string existingValue = existingRow[col.ColumnName]?.ToString().Trim() ?? "";
                    string newValue = newRow[col.ColumnName]?.ToString().Trim() ?? "";

                    if (decimal.TryParse(existingValue, out decimal oldNum) && decimal.TryParse(newValue, out decimal newNum))
                    {
                        if (Math.Abs(oldNum - newNum) > 0.0001M)
                        {
                            isDifferent = true;
                            differences[col.ColumnName] = $"Old: {oldNum}, New: {newNum}";
                        }
                    }
                    else if (!string.Equals(existingValue, newValue, StringComparison.OrdinalIgnoreCase))
                    {
                        isDifferent = true;
                        differences[col.ColumnName] = $"Old: {existingValue}, New: {newValue}";
                    }
                }

                if (!isDifferent)
                    return false;

                string updateQuery = "UPDATE dbo.products SET " +
                    string.Join(", ", newRow.Table.Columns.Cast<DataColumn>().Select(c => $"{c.ColumnName} = @{c.ColumnName}")) +
                    " WHERE ItemNum = @ItemNum";

                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConn))
                {
                    foreach (DataColumn col in newRow.Table.Columns)
                    {
                        cmd.Parameters.AddWithValue($"@{col.ColumnName}", newRow[col] ?? DBNull.Value);
                    }
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private bool ItemExists(SqlConnection sqlConn, string itemNum)
        {
            string query = "SELECT COUNT(*) FROM dbo.products WHERE ItemNum = @ItemNum";
            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                cmd.Parameters.AddWithValue("@ItemNum", itemNum);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

       
        private void InsertItem(SqlConnection sqlConn, DataRow row)
        {
            StringBuilder insertQuery = new StringBuilder("INSERT INTO dbo.products (");
            StringBuilder valuesQuery = new StringBuilder("VALUES (");
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (DataColumn col in row.Table.Columns)
            {
                insertQuery.Append(col.ColumnName + ", ");
                valuesQuery.Append($"@{col.ColumnName}, ");
                parameters.Add(new SqlParameter($"@{col.ColumnName}", row[col.ColumnName] ?? DBNull.Value));
            }

            insertQuery.Length -= 2;
            valuesQuery.Length -= 2;
            insertQuery.Append(") ");
            valuesQuery.Append(")");

            using (SqlCommand cmd = new SqlCommand(insertQuery.ToString() + valuesQuery.ToString(), sqlConn))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }
                private DataRow CreateSummaryDataRow(DataRow row, string status)
        {
            DataTable table = row.Table.Clone();
            table.Columns.Add("Status", typeof(string));

            DataRow summaryRow = table.NewRow();
            summaryRow["ItemNum"] = row["ItemNum"];
            summaryRow["ItemName"] = row["ItemName"]; // Ensure column exists
            summaryRow["ItemName_Extra"] = row["ItemName_Extra"]; // Ensure column exists
            summaryRow["Status"] = status;
            return summaryRow;
        }

        private void ExportSQLBtn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV files|*.csv", Title = "Save as CSV file" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string csvFilePath = sfd.FileName;
                    string connectionString = "Server=localhost\\SQLEXPRESS;Database=FOSTER_DB;User Id=FOSTER;Password=Foster1;";
                    string query = "SELECT * FROM dbo.products";

                    try
                    {
                        using (SqlConnection sqlConn = new SqlConnection(connectionString))
                        {
                            sqlConn.Open();
                            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            using (StreamWriter writer = new StreamWriter(csvFilePath))
                            using (CsvWriter csvWriter = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    csvWriter.WriteField(reader.GetName(i));
                                }
                                csvWriter.NextRecord();

                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        csvWriter.WriteField(reader[i]);
                                    }
                                    csvWriter.NextRecord();
                                }
                            }
                        }
                        MessageBox.Show("Data exported successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void LoadSQLBtn_Click(object sender, EventArgs e)
        {
            LoadDataFromSQL();
        }

        private void LoadDataFromSQL()
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FOSTER_DB;User Id=FOSTER;Password=Foster1;";
            string query = "SELECT * FROM dbo.products";

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConn))
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }
    }
}
