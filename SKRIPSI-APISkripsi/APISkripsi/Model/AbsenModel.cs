namespace APISkripsi.Model
{
    public class AbsenModel
    {
        public int Id { get; set; }
        public string Absen { get; set; }
        public string Keterangan { get; set; } = string.Empty;
        public int Kd_Mk { get; set; }
        public string Pukul { get; set; }
        public int Id_user { get; set; }
    }
}
