using System.Text;

namespace final_work
{
    internal class Encryption
    {
        //Class that defins how to encrypt and decrupt information before saving and reading
        private const int EncryptionOffset = 7;
        public static string Encrypt(string input)
        {
            StringBuilder encryptedString = new StringBuilder();

            foreach (char c in input)
            {
                encryptedString.Append((char)(c + EncryptionOffset));
            }

            return encryptedString.ToString();
        }
        public static string Decrypt(string input)
        {
            StringBuilder decryptedString = new StringBuilder();

            foreach (char c in input)
            {
                decryptedString.Append((char)(c - EncryptionOffset));
            }

            return decryptedString.ToString();
        }
    }
}
