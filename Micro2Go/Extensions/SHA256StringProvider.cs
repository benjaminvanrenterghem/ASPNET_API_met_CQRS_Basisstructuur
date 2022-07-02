using System.Security.Cryptography;
using System.Text;

namespace Micro2Go.Extensions {
	public static class SHA256StringProvider {
		public static string GetSHA256String(this string s) {
			using (SHA256 sha256Hash = SHA256.Create()) {
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++) {
					builder.Append(bytes[i].ToString("x2"));
				}

				return builder.ToString();
			}
		}
	}
}
