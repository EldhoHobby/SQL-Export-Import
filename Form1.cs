using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

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
            int newlyAdded = 0;
            int updated = 0;
            int noChange = 0;
            List<DataRow> summaryData = new List<DataRow>();

            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV files|*.csv", Title = "Select a CSV file" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string csvFilePath = ofd.FileName;
                    DataTable dataTable = new DataTable();
                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                    {
                        using (var dr = new CsvDataReader(csv))
                        {
                            dataTable.Load(dr);
                        }
                    }

                    // Update this with your actual connection string.
                    string connectionString = "Server=localhost\\SQLEXPRESS;Database=FOSTER_DB;User Id=FOSTER;Password=Foster1;";

                    using (SqlConnection sqlConn = new SqlConnection(connectionString))
                    {
                        sqlConn.Open();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string itemNum = row["ItemNum"].ToString();

                            if (ItemExists(sqlConn, itemNum))
                            {
                                if (UpdateItem(sqlConn, row))
                                {
                                    updated++;
                                    summaryData.Add(CreateSummaryDataRow(row, "Updated"));
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
                            }
                        }
                    }

                    MessageBox.Show("Data imported successfully!");

                    // Show the summary window
                    ImportSummaryForm summaryForm = new ImportSummaryForm(newlyAdded, updated, noChange, summaryData);
                    summaryForm.ShowDialog();
                }
            }
        }

        private bool ItemExists(SqlConnection sqlConn, string itemNum)
        {
            string query = "SELECT COUNT(*) FROM dbo.products WHERE ItemNum = @ItemNum";
            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                cmd.Parameters.AddWithValue("@ItemNum", itemNum);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool UpdateItem(SqlConnection sqlConn, DataRow row)
        {
            string query = @"UPDATE dbo.products SET 
                        ItemName = @ItemName,
                        Price = @Price,
                        NeverPrintInKitchen = @NeverPrintInKitchen,
                        ItemName_Extra = @ItemName_Extra,
                        Dept_ID = @Dept_ID,
                        Cost = @Cost,
                        Retail_Price = @Retail_Price,
                        In_Stock = @In_Stock,
                        Tax_1 = @Tax_1,
                        Tax_2 = @Tax_2,
                        Tax_3 = @Tax_3,
                        Tax_4 = @Tax_4,
                        Tax_5 = @Tax_5,
                        Tax_6 = @Tax_6,
                        Vendor_Number = @Vendor_Number,
                        VendorName = @VendorName,
                        Vendor_Part_Num = @Vendor_Part_Num,
                        AltSku = @AltSku,
                        Location = @Location,
                        AutoWeigh = @AutoWeigh,
                        FoodStampable = @FoodStampable,
                        Check_ID = @Check_ID,
                        Prompt_Price = @Prompt_Price,
                        Prompt_Quantity = @Prompt_Quantity,
                        Allow_BuyBack = @Allow_BuyBack,
                        Unit_Type = @Unit_Type,
                        Unit_Size = @Unit_Size,
                        Prompt_Description = @Prompt_Description,
                        Check_ID2 = @Check_ID2,
                        Count_This_Item = @Count_This_Item,
                        Print_On_Receipt = @Print_On_Receipt,
                        AllowReturns = @AllowReturns,
                        Liability = @Liability,
                        AllowOnDepositInvoices = @AllowOnDepositInvoices,
                        AllowOnFleetCard = @AllowOnFleetCard,
                        DisplayTaxInPrice = @DisplayTaxInPrice
                    WHERE ItemNum = @ItemNum";

            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);
                }
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }

        private void InsertItem(SqlConnection sqlConn, DataRow row)
        {
            string query = @"INSERT INTO dbo.products (
                        ItemNum, ItemName, Price, NeverPrintInKitchen, ItemName_Extra, 
                        Dept_ID, Cost, Retail_Price, In_Stock, Tax_1, Tax_2, Tax_3, 
                        Tax_4, Tax_5, Tax_6, Vendor_Number, VendorName, Vendor_Part_Num, 
                        AltSku, Location, AutoWeigh, FoodStampable, Check_ID, Prompt_Price, 
                        Prompt_Quantity, Allow_BuyBack, Unit_Type, Unit_Size, 
                        Prompt_Description, Check_ID2, Count_This_Item, Print_On_Receipt, 
                        AllowReturns, Liability, AllowOnDepositInvoices, AllowOnFleetCard, 
                        DisplayTaxInPrice)
                    VALUES (
                        @ItemNum, @ItemName, @Price, @NeverPrintInKitchen, @ItemName_Extra, 
                        @Dept_ID, @Cost, @Retail_Price, @In_Stock, @Tax_1, @Tax_2, @Tax_3, 
                        @Tax_4, @Tax_5, @Tax_6, @Vendor_Number, @VendorName, @Vendor_Part_Num, 
                        @AltSku, @Location, @AutoWeigh, @FoodStampable, @Check_ID, @Prompt_Price, 
                        @Prompt_Quantity, @Allow_BuyBack, @Unit_Type, @Unit_Size, 
                        @Prompt_Description, @Check_ID2, @Count_This_Item, @Print_On_Receipt, 
                        @AllowReturns, @Liability, @AllowOnDepositInvoices, @AllowOnFleetCard, 
                        @DisplayTaxInPrice)";

            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);
                }
                cmd.ExecuteNonQuery();
            }
        }

        private DataRow CreateSummaryDataRow(DataRow row, string status)
        {
            DataRow summaryRow = row.Table.NewRow();
            summaryRow["ItemNum"] = row["ItemNum"];
            summaryRow["ItemName"] = row["ItemName"];
            summaryRow["ItemName_Extra"] = row["ItemName_Extra"];
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
                    string query = "SELECT * FROM dbo.products"; // Replace 'products' with your actual table name

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
                                // Write header
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    csvWriter.WriteField(reader.GetName(i));
                                }
                                csvWriter.NextRecord();

                                // Write data
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
            string query = "SELECT * FROM dbo.products"; // Replace 'products' with your actual table name

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
