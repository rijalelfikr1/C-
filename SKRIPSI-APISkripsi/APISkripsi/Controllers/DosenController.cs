using Microsoft.AspNetCore.Mvc;
using APISkripsi.Model;
using APISkripsi.Function;
using Microsoft.Data.SqlClient;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace APISkripsi.Controllers
{
    [ApiController]
    public class DosenController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        public DosenController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }


        public string ConnectionString = "Data Source=RIJALELFIKRI;Initial Catalog=SKRIPSI;Integrated Security=True;" + "MultipleActiveResultSets = True";

        // UserModel user = new UserModel();
        private readonly IConfiguration _configuration;

        public DosenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private string DbConnection()
        {
            var dbAccess = new DatabaseAccessLayer();
            string dbString = dbAccess.ConnectionString;
            return dbString;
        }

        private string getNextFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
            {
                int i = 0;
                while (System.IO.File.Exists(fileName))
                {
                    if (i == 0)
                        fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
                    else
                        fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
                }
            }
            return fileName;
        }




        [HttpGet]
        [Route("api/[controller]/LgnDsn")]
        public List<DosenModel> LoginDosen(int NIDN, string password)
        {
            var hashpassword = new Authentication();
            List<DosenModel> dosens = new List<DosenModel>();

            DosenModel dosen = new DosenModel();
            dosen.NIDN = NIDN;
            string passwordhash = hashpassword.MD5Hash(password);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Mst_Dosen WHERE NIDN = '" + dosen.NIDN + "'" + " AND Password = '" + passwordhash + "'";
              //  Console.WriteLine(query);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var data = new DosenModel();
                        data.NIDN = Convert.ToInt32(reader["NIDN"]);
                        data.NIP = Convert.ToInt32(reader["NIP"]);
                        data.NoHp = Convert.ToInt32(reader["noHp"]);
                        data.Jabatan = reader["Jabatan"].ToString();
                        data.Name = reader["name"].ToString();
                        data.Password = reader["Password"].ToString();
                        data.Profile = reader["Profile"].ToString();
                        data.email = reader["email"].ToString();
                        dosens.Add(data);
                    }
                    conn.Close();
                    return dosens;
                }

                else
                {
                    conn.Close();
                    return dosens;
                }
            }
        }

        [HttpPost]
        [Route("api/[controller]/RgstrDsn")]
        public async Task<IActionResult> EditImageFile(List<IFormFile> imagePath, int NIDN, int NIP, string prodi, string jabatan, string name, int noHp, string email, string password)
        {
            long size = imagePath.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in imagePath)
            {
                if (formFile.Length > 0)
                {
                    // application's base path
                    string contentRootPath = _hostingEnvironment.ContentRootPath;
                    string filePath = getNextFileName(contentRootPath + "\\Upload\\Images\\" + formFile.FileName);
                    Console.WriteLine("contenroot " + contentRootPath);
                    Console.WriteLine("coba " + filePath);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    var hashpassword = new Authentication();
                    DosenModel dosen = new DosenModel();
                    dosen.NIDN = NIDN;
                    dosen.NIP = NIP;
                    dosen.Prodi = prodi;
                    dosen.Jabatan = jabatan;
                    dosen.Profile = filePath;
                    dosen.Name = name;
                    dosen.NoHp = noHp;
                    dosen.email = email;
                    string passwordhash = hashpassword.MD5Hash(password);

                    string query = "INSERT INTO [dbo].[Mst_Dosen] ([NIDN],[NIP],[Prodi],[Jabatan],[Profile],[name],[noHp],[email],[Password])" +
                        "VALUES (@NIDN, @NIP, @Prodi, @Jabatan, @Profile, @Name, @NoHp, @Email, '" + passwordhash + "')";
                    using (SqlConnection con = new SqlConnection(DbConnection()))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@NIDN", dosen.NIDN);
                            cmd.Parameters.AddWithValue("@NIP", dosen.NIP);
                            cmd.Parameters.AddWithValue("@Prodi", dosen.Prodi);
                            cmd.Parameters.AddWithValue("@Jabatan", dosen.Jabatan);
                            cmd.Parameters.AddWithValue("@Profile", dosen.Profile);
                            cmd.Parameters.AddWithValue("@Name", dosen.Name);
                            cmd.Parameters.AddWithValue("@NoHp", dosen.NoHp);
                            cmd.Parameters.AddWithValue("@Email", dosen.email);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            return Ok(new { count = imagePath.Count, filePaths });
        }
        
       
    }
}