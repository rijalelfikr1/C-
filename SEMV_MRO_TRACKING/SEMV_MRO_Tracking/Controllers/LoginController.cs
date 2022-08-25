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
    public class LoginController : Controller
    {
        public IActionResult SignOut()
        {
            if (HttpContext.Session != null)
            {
                HttpContext.Session.Clear();
            }

            return View("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        private string DbConnection()
        {
            var dbAccess = new DatabaseAccessLayer();

            string dbString = dbAccess.ConnectionString;

            return dbString;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel user)
        {
            var hashpassword = new Authentication();

            if (ModelState.IsValid)
            {
                List<LoginModel> userInfo = new List<LoginModel>();
                using (SqlConnection conn = new SqlConnection(DbConnection()))
                {
                    string passwordHash = hashpassword.MD5Hash(user.password);
                    string query = "SELECT * FROM mst_users WHERE sesa_id = '" + user.sesa_id + "' AND password = '" + passwordHash + "' ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        ViewData["Message"] = "HAS DATA";
                        while (reader.Read())
                        {
                            var loginUser = new LoginModel();
                            loginUser.id = Convert.ToInt32(reader["id_user"]);
                            loginUser.name = reader["name"].ToString();
                            loginUser.sesa_id = reader["sesa_id"].ToString();
                            loginUser.level = reader["level"].ToString();
                            userInfo.Add(loginUser);
                            HttpContext.Session.SetString("sesa_id", loginUser.sesa_id);
                            HttpContext.Session.SetString("name", loginUser.name);
                            HttpContext.Session.SetString("level", loginUser.level);
                        }

                        if (HttpContext.Session.GetString("level") == "user")
                        {
                            return RedirectToAction("Index", "User");

                        } else if (HttpContext.Session.GetString("level") == "admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "User and Password not Registered !";
                    }
                    conn.Close();

                }
            }

            return View("Index");
        }
    }
}
