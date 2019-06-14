using BCrypt;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TaskMenagerService.Interfaces;

namespace TaskMenagerService.Security
{
	public static class BCrypt
	{
		const string salt = "$2b$10$WpizuiCSCGJvFT31br3tJO";
		public static string Encrypt(this string password) => BCryptHelper.HashPassword(password, salt);
		public static bool VerifyPassword(this string hashPassword, string userPassword) => BCryptHelper.CheckPassword(userPassword, hashPassword);
	}
	public class AesAlgorithm : IAesAlgorithm
	{
		private readonly byte[] privateKey = { 8, 2, 2, 4, 3, 6, 7, 8, 1, 2, 3, 7, 2, 6, 4, 8,
						   1, 2, 3, 9, 5, 6, 7, 4, 1, 2, 7, 4, 5, 6, 7, 8 };
		AesCryptoServiceProvider aes_provider;
		private AesCryptoServiceProvider CreateProvider(byte[] key)
		{
			return new AesCryptoServiceProvider
			{
				Key = key,
				Padding = PaddingMode.PKCS7,
				Mode = CipherMode.ECB
			};
		}
		public AesAlgorithm() => aes_provider = CreateProvider(privateKey);	
		public string Encrypt(string text)
		{
			ICryptoTransform transform = aes_provider.CreateEncryptor();
			byte[] encrypted_bytes = transform.TransformFinalBlock(Encoding.ASCII.GetBytes(text), 0, text.Length);
			string ecryptedText = Convert.ToBase64String(encrypted_bytes);
			return ecryptedText;
		}
		public string Decrypt(string text)
		{
			ICryptoTransform transform = aes_provider.CreateDecryptor();
			byte[] enc_bytes = Convert.FromBase64String(text);
			byte[] decrypted_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);
			string decryptedText = Encoding.ASCII.GetString(decrypted_bytes);
			return decryptedText;
		}
	}
}
