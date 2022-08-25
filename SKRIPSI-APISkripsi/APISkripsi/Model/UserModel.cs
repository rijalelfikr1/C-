namespace APISkripsi.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? NIM { get; set; }
        public int? NoTelp { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? Angkatan { get; set; }
        public string? Semester { get; set; } = string.Empty;
        public string? Fakultas { get; set; } = string.Empty;
        public string? Jurusan { get; set; } = string.Empty;
        public string? Profile { get; set; } = string.Empty;

    }
}
