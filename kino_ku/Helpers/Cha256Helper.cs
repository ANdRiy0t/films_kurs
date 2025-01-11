using System.Security.Cryptography;
using System.Text;

namespace kino_ku.Helpers;

public static class Sha256Helper
{
    public static string HashToSha256(this string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input), "Вхідний рядок не може бути порожнім або null.");

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}