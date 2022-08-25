namespace APISkripsi.Model
{
    public class DosenModel
    {
        public IFormFile profile { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NIDN { get; set; }
        public int NIP { get; set; }
        public string Jabatan { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string Prodi { get; set; } = string.Empty;
        public int NoHp { get; set; }
        public string email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
