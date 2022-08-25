using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEMV_MRO_Tracking.Function;
using SEMV_MRO_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;

namespace SEMV_MRO_Tracking.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Getmaterialhistory(string Part_Number)
        {
            var db = new DatabaseAccessLayer();
            List<TransactionModel> TransactionData = db.GetTransactionData(Part_Number);
            var dataModel = new ViewModel()
            {
                TransactionDetails = TransactionData
            };

            return PartialView("_HistoryPartnumber", dataModel);
        }


        [HttpPost]
        public JsonResult SubmitMaterial(string part_number, float quantity, string request_by)
        {
            TransactionModel data = new TransactionModel();

            data.Part_Number = part_number;
            data.Quantity_Req = quantity;
            data.Request_By = request_by;

            string query = @"INSERT INTO tbl_scanout_header (Part_Number,Quantity_Req,Request_By) VALUES(@part_number, @quantity, @request_by)
                UPDATE mst_material SET Stock = Stock - @quantity WHERE Part_Number = @part_number
                INSERT INTO [dbo].[tbl_transaction_history] ([Part_Number] ,[Sesa_user] ,[Transaction_typ] ,[Qty]) VALUES (@part_number ,@request_by ,'SCAN_OUT' ,@quantity)";
            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@part_number", data.Part_Number);
                    cmd.Parameters.AddWithValue("@quantity", data.Quantity_Req);
                    cmd.Parameters.AddWithValue("@request_by", data.Request_By);

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetMaterialDetail(string part_number)
        {
            MaterialModel data = new MaterialModel();

            data.Part_Number = part_number;

            string query = "SELECT TOP 1 Unit,Material_Name,Material_Desc,Stock,Price,Supplier_Name FROM mst_material WHERE Part_Number = '" + part_number + "' ";
            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Unit = reader["Unit"].ToString();
                            data.Material_Name = reader["Material_Name"].ToString();
                            data.Material_Desc = reader["Material_Desc"].ToString();
                            data.Stock = Convert.ToSingle(reader["Stock"]);
                            data.Price = Convert.ToString(reader["Price"]);
                            data.Supplier_Name = reader["Supplier_Name"].ToString();
                        }
                    }
                    conn.Close();
                }
            }

            return Json(data);
        }

        public IActionResult ScanIn()
        {
            if (HttpContext.Session.GetString("level") == "admin")
            {
                ViewBag.Sesa_id = HttpContext.Session.GetString("sesa_id");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("level") == "admin")
            {
                var db = new DatabaseAccessLayer();
                List<MaterialModel> MaterialData = db.GetMaterialData();
                var dataModel = new ViewModel()
                {
                    MaterialDetails = MaterialData
                };
                ViewBag.Sesa_id = HttpContext.Session.GetString("sesa_id");
                return View(dataModel);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }
        public FileContentResult Export_mat_data()
        {
            var db = new DatabaseAccessLayer();
            List<MaterialModel> MaterialData = db.GetMaterialData();
            var dataModel = new ViewModel()
            {
                MaterialDetails = MaterialData
            };
            var csv = "Part_Number,Barcode,Unit,Material_Name,Material_Desc,Leadtime,Stock,Safety_Stock,Max_Stock,Total_Purchase,Price,Supplier_Name,Supplier_Email,Timestamp\n";
            foreach(var item in dataModel.MaterialDetails)
            {
                csv += item.Part_Number + ",";
                csv += item.Barcode + ",";
                csv += item.Unit + ",";
                csv += item.Material_Name + ",";
                csv += item.Material_Desc.Replace(",","") + ",";
                csv += item.Leadtime + ",";
                csv += item.Stock + ",";
                csv += item.Safety_Stock + ",";
                csv += item.Max_Stock + ",";
                csv += item.Total_Purchase + ",";
                csv += item.Price + ",";
                csv += item.Supplier_Name + ",";
                csv += item.Supplier_Email + ",";
                csv += item.Timestamp + "\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "mst_material.csv");
        }

        [HttpPost]
        public IActionResult ImportDataMaterial(IFormFile importFile, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });

            string path = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.GetFileName(importFile.FileName);

            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                importFile.CopyTo(stream);
                //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }

            var option = Request.Form["option"].FirstOrDefault();

            var fileNames = $"{hostingEnvironment.ContentRootPath}\\Upload\\{importFile.FileName}";

            var MaterialCSV = new CSVService();

            var Result = MaterialCSV.ReadCSVFile(fileNames);

            DataTable dt = MaterialCSV.ToDataTable(Result);

            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                SqlCommand truncatecmd = new SqlCommand();
                truncatecmd.Connection = conn;
                SqlCommand insertcmd = new SqlCommand();
                insertcmd.Connection = conn;
                truncatecmd.CommandText = "TRUNCATE TABLE [dbo].[mst_material_tmp]";
                try
                {
                    conn.Open();
                    truncatecmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                using (SqlBulkCopy s = new SqlBulkCopy(DbConnection()))
                {
                    s.DestinationTableName = "[dbo].[mst_material_tmp]";
                    s.ColumnMappings.Add("Part_Number", "Part_Number");
                    s.ColumnMappings.Add("Barcode", "Barcode");
                    s.ColumnMappings.Add("Unit", "Unit");
                    s.ColumnMappings.Add("Material_Name", "Material_Name");
                    s.ColumnMappings.Add("Material_Desc", "Material_Desc");
                    s.ColumnMappings.Add("Leadtime", "Leadtime");
                    s.ColumnMappings.Add("Stock", "Stock");
                    s.ColumnMappings.Add("Safety_Stock", "Safety_Stock");
                    s.ColumnMappings.Add("Max_Stock", "Max_Stock");
                    s.ColumnMappings.Add("Total_Purchase", "Total_Purchase");
                    s.ColumnMappings.Add("Price", "Price");
                    s.ColumnMappings.Add("Supplier_Name", "Supplier_Name");
                    s.ColumnMappings.Add("Supplier_Email", "Supplier_Email");
                    try
                    {
                        conn.Open();
                        s.WriteToServer(dt);
                        conn.Close();
                        if (option == "truncate")
                        {
                            truncatecmd.CommandText = "TRUNCATE TABLE [dbo].[mst_material]";
                            try
                            {
                                conn.Open();
                                truncatecmd.ExecuteNonQuery();
                                conn.Close();
                            }
                            catch (Exception e)
                            {
                                throw new Exception(e.Message);
                            }
                        }
                        insertcmd.CommandText = "EXEC INSERT_MST_MATERIAL";
                        try
                        {
                            conn.Open();
                            insertcmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }

                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }

            return RedirectToAction("Index", "Admin");
        }


        [HttpPost]
        public IActionResult Editmstmaterial(int id, string Part_Number, string Barcode, string Unit, string Material_Name, string Material_Desc, float Stock, float Safety_Stock, float Max_Stock, string Leadtime, string Price, string Supplier_Name, string Supplier_Email, float Total_Purchase,string Edit_typ, string Remark, string sesa_id)
        {
            TransactionModel transaction = new TransactionModel();
            transaction.Order_ID = "EDITDATA";
            transaction.Part_Number = Part_Number;
            transaction.Quantity_Req = 0;
            transaction.Request_By = sesa_id;
            transaction.Request_Type = Edit_typ;
            transaction.Remark = Remark;

            MaterialModel material = new MaterialModel();
            material.Id = id;
            material.Part_Number = Part_Number;
            material.Barcode = Barcode;
            material.Unit = Unit;
            material.Material_Name = Material_Name;
            material.Material_Desc = Material_Desc;
            material.Stock = Stock;
            material.Safety_Stock = Safety_Stock;
            material.Max_Stock = Max_Stock;
            material.Leadtime = Leadtime;
            material.Price = Price;
            material.Supplier_Name = Supplier_Name;
            material.Supplier_Email = Supplier_Email;
            material.Total_Purchase = Total_Purchase;

            string query = @"UPDATE [dbo].[mst_material] SET [Part_Number] = @Part_Number_mt
                                                            ,[Barcode] = @Barcode_mt
                                                            ,[Unit] = @Unit_mt
                                                            ,[Material_Name] = @Material_Name_mt
                                                            ,[Material_Desc] = @Material_Desc_mt
                                                            ,[Leadtime] = @Leadtime_mt
                                                            ,[Stock] = @Stock_mt
                                                            ,[Safety_Stock] = @Safety_Stock_mt
                                                            ,[Max_Stock] = @Max_Stock_mt
                                                            ,[Total_Purchase] = @Total_Purchase_mt
                                                            ,[Price] = @Price_mt
                                                            ,[Supplier_Name] = @Supplier_Name_mt
                                                            ,[Supplier_Email] = @Supplier_Email_mt
                                                            ,[Timestamp] = GETDATE() WHERE Id = @Id_mt
                            INSERT INTO [dbo].[tbl_transaction_history] ([Part_Number],[Purchase_Number],[Sesa_user] ,[Transaction_typ] ,[Qty],[Remark])
                            VALUES (@part_number_hist,@po_number_hist, @request_by_hist, @request_typ_hist, @quantity_hist,@remark_hist)";

            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Part_Number_mt", material.Part_Number ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Barcode_mt", material.Barcode ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Unit_mt", material.Unit ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Material_Name_mt", material.Material_Name ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Material_Desc_mt", material.Material_Desc ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Leadtime_mt", material.Leadtime ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Stock_mt", material.Stock);
                    cmd.Parameters.AddWithValue("@Safety_Stock_mt", material.Safety_Stock);
                    cmd.Parameters.AddWithValue("@Max_Stock_mt", material.Max_Stock);
                    cmd.Parameters.AddWithValue("@Total_Purchase_mt", material.Total_Purchase);
                    cmd.Parameters.AddWithValue("@Price_mt", material.Price);
                    cmd.Parameters.AddWithValue("@Supplier_Name_mt", material.Supplier_Name ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Supplier_Email_mt", material.Supplier_Email ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@Id_mt", material.Id);

                    cmd.Parameters.AddWithValue("@part_number_hist", transaction.Part_Number ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@po_number_hist", transaction.Order_ID ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@request_by_hist", transaction.Request_By ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@request_typ_hist", transaction.Request_Type ?? Convert.DBNull);
                    cmd.Parameters.AddWithValue("@quantity_hist", transaction.Quantity_Req);
                    cmd.Parameters.AddWithValue("@remark_hist", transaction.Remark ?? Convert.DBNull);
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return Json(transaction);
        }

        [HttpPost]
        public IActionResult ScanIn(string part_number, float request_qty, string sesa_id, string po_number)
        {
            TransactionModel data = new TransactionModel();

            data.Order_ID = po_number;
            data.Part_Number = part_number;
            data.Quantity_Req = request_qty;
            data.Request_By = sesa_id;
            data.Request_Type = "SCAN_IN";

            string query = @"INSERT INTO [dbo].[tbl_scanin_header] (Order_ID,Part_Number,Quantity_Req,Request_By) VALUES(@po_number,@part_number, @quantity, @request_by)
                UPDATE [dbo].[mst_material] SET Total_Purchase = Total_Purchase - @quantity, Stock = Stock + @quantity WHERE Part_Number = @part_number
                INSERT INTO [dbo].[tbl_transaction_history] ([Part_Number] ,[Purchase_Number],[Sesa_user] ,[Transaction_typ] ,[Qty]) VALUES (@part_number,@po_number, @request_by, @request_typ, @quantity)";

            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@part_number", data.Part_Number);
                    cmd.Parameters.AddWithValue("@quantity", data.Quantity_Req);
                    cmd.Parameters.AddWithValue("@request_by", data.Request_By);
                    cmd.Parameters.AddWithValue("@request_typ", data.Request_Type);
                    cmd.Parameters.AddWithValue("@po_number", data.Order_ID);

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return Json(data);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string part_number, float request_qty, string sesa_id)
        {
            TransactionModel data = new TransactionModel();

            data.Part_Number = part_number;
            data.Quantity_Req = request_qty;
            data.Request_By = sesa_id;
            data.Request_Type = "ORDER_SUPPLIER";

            string query = @"INSERT INTO [dbo].[tbl_order_header] (Part_Number,Quantity_Req,Request_By) VALUES(@part_number, @quantity, @request_by)
                UPDATE [dbo].[mst_material] SET Total_Purchase = Total_Purchase + @quantity WHERE Part_Number = @part_number
                INSERT INTO [dbo].[tbl_transaction_history] ([Part_Number] ,[Sesa_user] ,[Transaction_typ] ,[Qty]) VALUES (@part_number, @request_by, @request_typ, @quantity)";

            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@part_number", data.Part_Number);
                    cmd.Parameters.AddWithValue("@quantity", data.Quantity_Req);
                    cmd.Parameters.AddWithValue("@request_by", data.Request_By);
                    cmd.Parameters.AddWithValue("@request_typ", data.Request_Type);

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return Json(data);
        }

        private string DbConnection()
        {
            var dbAccess = new DatabaseAccessLayer();
            string dbString = dbAccess.ConnectionString;
            return dbString;
        }

        public IActionResult TransactionHistory()
        {

            if (HttpContext.Session.GetString("level") == "admin")
            {
                var db = new DatabaseAccessLayer();
                List<TransactionModel> TransactionData = db.GetTransactionData();
                var dataModel = new ViewModel()
                {
                    TransactionDetails = TransactionData
                };

                return View(dataModel);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }            
        }

        public IActionResult MaterialStock()
        {
            if (HttpContext.Session.GetString("level") == "admin")
            {
                var db = new DatabaseAccessLayer();
                List<MaterialModel> MaterialData = db.GetMaterialStock();
                var dataModel = new ViewModel()
                {
                    MaterialDetails = MaterialData
                };

                ViewBag.Sesa_id = HttpContext.Session.GetString("sesa_id");
                return View(dataModel);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

    }
}
