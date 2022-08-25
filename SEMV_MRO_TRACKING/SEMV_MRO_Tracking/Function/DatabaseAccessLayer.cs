using SEMV_MRO_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SEMV_MRO_Tracking.Function
{
    public class DatabaseAccessLayer
    {
        public string ConnectionString = "Data Source=10.155.128.23;Initial Catalog=SEMV_MRO;Persist Security Info=True;User ID=semv;Password=Semv@123;" + "MultipleActiveResultSets=True";

        public List<MaterialModel> GetMaterialData()
        {

            List<MaterialModel> materialList = new List<MaterialModel>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT [Id],[Part_Number],[Barcode],[Unit],[Material_Name],[Material_Desc],[Leadtime],[Stock],[Safety_Stock],[Max_Stock],[Total_Purchase],Format([Price],'#,##0') as Price, Format(Stock*Price,'#,##0') as TotalValue ,[Supplier_Name],[Supplier_Email],[Timestamp] FROM mst_material";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new MaterialModel();
                        data.Part_Number = reader["Part_Number"].ToString();
                        data.Barcode = reader["Barcode"].ToString();
                        data.Unit = reader["Unit"].ToString();
                        data.Material_Name = reader["Material_Name"].ToString();
                        data.Material_Desc = reader["Material_Desc"].ToString();
                        data.Stock = Convert.ToSingle(reader["Stock"]);
                        data.Leadtime = reader["Leadtime"].ToString();
                        data.Price = Convert.ToString(reader["Price"]);
                        data.TotalValue = Convert.ToString(reader["TotalValue"]);
                        data.Supplier_Name = reader["Supplier_Name"].ToString();
                        data.Max_Stock = Convert.ToSingle(reader["Max_Stock"]);
                        data.Safety_Stock = Convert.ToSingle(reader["Safety_Stock"]);
                        data.Total_Purchase = Convert.ToSingle(reader["Total_Purchase"]);
                        data.Timestamp = reader["Timestamp"].ToString();
                        data.Supplier_Email = reader["Supplier_Email"].ToString();
                        data.Id = Convert.ToInt32(reader["Id"]);
                        materialList.Add(data);

                    }

                    conn.Close();
                    return materialList;
                }

                else
                {
                    conn.Close();
                    return null;
                }

            }

        }

        public List<TransactionModel> GetTransactionData(string Part_number = null)
        {

            List<TransactionModel> transactionList = new List<TransactionModel>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Purchase_Number,Part_Number,Qty,Sesa_user,Transaction_typ,Timestamp FROM tbl_transaction_history";
                if (Part_number != null)
                {
                    query += " WHERE Part_Number = @part_number";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                if (Part_number != null)
                {
                    cmd.Parameters.AddWithValue("@part_number", Part_number);
                }
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new TransactionModel();
                        data.Order_ID = reader["Purchase_Number"].ToString();
                        data.Part_Number = reader["Part_Number"].ToString();
                        data.Quantity_Req = Convert.ToSingle(reader["Qty"]);
                        data.Request_By = reader["Sesa_user"].ToString();
                        data.Request_Type = reader["Transaction_typ"].ToString();
                        data.Timestamp = reader["Timestamp"].ToString();                      

                        transactionList.Add(data);

                    }

                    conn.Close();
                    return transactionList;
                }

                else
                {
                    conn.Close();
                    return null;
                }

            }

        }

        public List<MaterialModel> GetMaterialStock()
        {

            List<MaterialModel> materialList = new List<MaterialModel>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Part_Number, Material_Name, Stock, Safety_Stock, Max_Stock, Total_Purchase, Supplier_Name, Stock_Status, Unit, format(Price, 'N0') as Price,Supplier_Email FROM V_MATERIAL_STOCK";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new MaterialModel();
                        data.Part_Number = reader["Part_Number"].ToString();                      
                        data.Material_Name = reader["Material_Name"].ToString();
                        data.Unit = reader["Unit"].ToString();
                        data.Stock = Convert.ToSingle(reader["Stock"]);
                        data.Safety_Stock = Convert.ToSingle(reader["Safety_Stock"]);
                        data.Max_Stock = Convert.ToSingle(reader["Max_Stock"]);
                        data.Total_Purchase = Convert.ToSingle(reader["Total_Purchase"]);
                        data.Supplier_Name = reader["Supplier_Name"].ToString();
                        data.Supplier_Email = reader["Supplier_Email"].ToString();
                        data.Stock_Status = reader["Stock_Status"].ToString();
                        data.Price = Convert.ToString(reader["Price"]);

                        materialList.Add(data);

                    }

                    conn.Close();
                    return materialList;
                }

                else
                {
                    conn.Close();
                    return null;
                }

            }

        }
    }
}
