namespace FIAP.PLAY.Domain.Shared.Extensions
{
    public static class PasswordExtension
    {
        public static string Encrypt(this string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
        public static bool Decrypt(this string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
}
