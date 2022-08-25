using Microsoft.AspNetCore.Mvc;
using APISkripsi.Model;
using APISkripsi.Function;
using Microsoft.Data.SqlClient;

namespace APISkripsi.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        public string ConnectionString = "Data Source=RIJALELFIKRI;Initial Catalog=SKRIPSI;Integrated Security=True;" + "MultipleActiveResultSets = True";

        // UserModel user = new UserModel();
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string DbConnection()
        {
            var dbAccess = new DatabaseAccessLayer();
            string dbString = dbAccess.ConnectionString;
            return dbString;
        }

        // Get User for ????
        [HttpGet]
        [Route("api/[controller]/GetUser")]
        public List<UserModel> GetUser()
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "SELECT * FROM Mst_Mhs";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new UserModel();
                        data.Id = Convert.ToInt32(reader["id"]);
                        data.NIM = Convert.ToInt32(reader["NIM"]);
                        data.NoTelp = Convert.ToInt32(reader["noTelp"]);
                        data.Angkatan = Convert.ToInt32(reader["angkatan"]);
                        data.UserName = reader["username"].ToString();
                        data.Name = reader["name"].ToString();
                        data.Email = reader["email"].ToString();
                        data.Semester = reader["semester"].ToString();
                        data.Fakultas = reader["fakultas"].ToString();
                        data.Jurusan = reader["jurusan"].ToString();
                        data.Profile = reader["image"].ToString();
                        users.Add(data);
                    }

                    conn.Close();
                    return users;
                }

                else
                {
                    conn.Close();
                    return users;
                }
            }
        }

        [HttpPost]
        [Route("api/[controller]/RgstrUsr")]
        public JsonResult RegisterUser(string name, int NIM, string email, string password)
        {
            var hashpassword = new Authentication();
            UserModel user = new UserModel();
            user.Name = name;
            user.NIM = NIM;
            user.Email = email;

            string passwordhash = hashpassword.MD5Hash(password);
            int Excute;
            string query = "INSERT INTO [dbo].[Mst_Mhs] ([username],[name],[NIM],[email],[password])" +
                "VALUES (@UserName, @Name, @NIM, @Email, '" + passwordhash + "' )";
            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@NIM", user.NIM);
                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    conn.Open();
                    Excute = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return Json(Excute);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[controller]/LgnUsr")]
        
        public IActionResult LoginUser(UserModel user)
        {
            var hashpassword = new Authentication();

            if (ModelState.IsValid)
            {
                List<UserModel> userInfo = new List<UserModel>();

                using (SqlConnection conn = new SqlConnection(DbConnection()))
                {
                    string passwordHash = hashpassword.MD5Hash(user.Password);
                    string query = "SELECT * FROM Mst_Mhs WHERE NIM = '" + user.NIM + "'" + " AND password = '" + hashpassword + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        ViewData["Message"] = "HAS DATA";
                        while (reader.Read())
                        {
                            var data = new UserModel();
                            data.Id = Convert.ToInt32(reader["id"]);
                            data.NIM = Convert.ToInt32(reader["NIM"]);
                            data.NoTelp = Convert.ToInt32(reader["noTelp"]);
                            data.Angkatan = Convert.ToInt32(reader["angkatan"]);
                            data.UserName = reader["username"].ToString();
                            data.Name = reader["name"].ToString();
                            data.Email = reader["email"].ToString();
                            data.Semester = reader["semester"].ToString();
                            data.Fakultas = reader["fakultas"].ToString();
                            data.Jurusan = reader["jurusan"].ToString();
                            data.Profile = reader["image"].ToString();

                            userInfo.Add(data);
                            
                            HttpContext.Session.SetInt32("id_user", data.Id);
                            HttpContext.Session.SetString("name", data.Name);
                            HttpContext.Session.SetInt32("NIM", data.NIM);

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

        /* [HttpPost]
           [Route("api/[controller]/LgnUsr")]
         public List<UserModel> LoginUser(int NIM, string password)
         {
             var hashpassword = new Authentication();
             List<UserModel> users = new List<UserModel>();

             UserModel user = new UserModel();
             user.NIM = NIM;
             string passwordhash = hashpassword.MD5Hash(password);
             using (SqlConnection conn = new SqlConnection(ConnectionString))
             {

                 string query = "SELECT * FROM Mst_Mhs WHERE NIM = '" + user.NIM + "'" + " AND password = '" + passwordhash + "'";
                 Console.WriteLine(query);
                 SqlCommand cmd = new SqlCommand(query, conn);
                 conn.Open();
                 SqlDataReader reader = cmd.ExecuteReader();
                 if (reader.HasRows)
                 {
                     while (reader.Read())
                     {

                         var data = new UserModel();
                         data.Id = Convert.ToInt32(reader["id"]);
                         data.NIM = Convert.ToInt32(reader["NIM"]);
                         data.NoTelp = Convert.ToInt32(reader["noTelp"]);
                         data.Angkatan = Convert.ToInt32(reader["angkatan"]);
                         data.UserName = reader["username"].ToString();
                         data.Name = reader["name"].ToString();
                         data.Email = reader["email"].ToString();
                         data.Semester = reader["semester"].ToString();
                         data.Fakultas = reader["fakultas"].ToString();
                         data.Jurusan = reader["jurusan"].ToString();
                         data.Profile = reader["image"].ToString();
                         users.Add(data);

                            HttpContext.Session.SetInt32("id_user", data.Id);
                            HttpContext.Session.SetString("name", data.Name);
                            HttpContext.Session.SetInt32("NIM", data.NIM);        
                     }
                     conn.Close();
                     return users;
                 }

                 else
                 {
                     conn.Close();
                     return users;
                 }

             }
         }*/


        [HttpGet]
        [Route("api/[controller]/AllAbsen")]
        public List<AbsenModel> GetAllAbsen()
        {
            List<AbsenModel> absens = new List<AbsenModel>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "SELECT * FROM absen";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new AbsenModel();
                        data.Id = Convert.ToInt32(reader["id"]);
                        data.Kd_Mk = Convert.ToInt32(reader["kd_Mk"]);
                        data.Absen = reader["absen"].ToString();
                        data.Pukul = reader["pukul"].ToString();
                        data.Id_user = Convert.ToInt32(reader["id_user"]);

                        absens.Add(data);
                    }

                    conn.Close();
                    return absens;
                }

                else
                {
                    conn.Close();
                    return absens;
                }
            }
        }



        [HttpGet]
        [Route("api/[controller]/Absen/{id}")]
        public List<AbsenModel> GetAbsen(string id)
        {
            List<AbsenModel> absens = new List<AbsenModel>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "SELECT COUNT absen WHERE id = '"+id+"' FROM absen";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new AbsenModel();
                        data.Id = Convert.ToInt32(reader["id"]);
                        data.Kd_Mk = Convert.ToInt32(reader["kd_Mk"]);
                        data.Absen = reader["absen"].ToString();
                        data.Pukul = reader["pukul"].ToString();
                        data.Id_user = Convert.ToInt32(reader["id_user"]);

                        absens.Add(data);
                    }

                    conn.Close();
                    return absens;
                }

                else
                {
                    conn.Close();
                    return absens;
                }
            }
        }


        [HttpPost]
        [Route("api/[controller]/AbsenUsr/{id_user}")]
        public JsonResult AbsenUser(string keterangan, string absen, string kode_Mk, int id_user)
        {
            //AbsenModel absenuser = new AbsenModel();

            int Excute;
            string query = "INSERT INTO [dbo].[absen] ([absen],[keterangan],[kd_Mk],[pukul],[id_user])" +
                "VALUES (@keterangan, @absen, @kd_Mk, @pukul, @id_user)";
            using (SqlConnection conn = new SqlConnection(DbConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keterangan", keterangan);
                    cmd.Parameters.AddWithValue("@absen", absen);
                    cmd.Parameters.AddWithValue("@kd_Mk", kode_Mk);
                    cmd.Parameters.AddWithValue("@pukul", DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@id_user", id_user);

                    conn.Open();
                    Excute = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return Json(Excute);

        }
        /*   private string CreateToken(UserModel user)
           {
               List<Claim> claims = new List<Claim>
               {
                   new Claim(ClaimTypes.Name, user.UserName),
                   new Claim(ClaimTypes.Role, "Admin")
               };

               var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                   _configuration.GetSection("AppSettings:Token").Value));

               var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

               var token = new JwtSecurityToken(
                   claims: claims,
                   expires: DateTime.Now.AddDays(1),
                   signingCredentials: creds);

               var jwt = new JwtSecurityTokenHandler().WriteToken(token);

               return jwt;
           }*/
    }
}
