using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEMV_MRO_Tracking.Function;
using SEMV_MRO_Tracking.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SEMV_MRO_Tracking.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("level") == "user")
            {
                ViewBag.Sesa_id = HttpContext.Session.GetString("sesa_id");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult MasterData()
        {
            if (HttpContext.Session.GetString("level") == "user")
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

        private string DbConnection()
        {
            var dbAccess = new DatabaseAccessLayer();
            string dbString = dbAccess.ConnectionString;
            return dbString;
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
    }
}
