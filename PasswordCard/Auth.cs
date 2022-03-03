using System;
using System.Text;
using System.Security;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace PasswordCard
{
    internal class Auth
    {
        public static bool SUCCESS = false; // bool that shows, if access to card has been granted //

        // Method to get a RSA-Provider from the SmartCard //
        private static RSACryptoServiceProvider GetRSACSPSmartCard(SecureString PIN, out bool success)
        {
            CspParameters csp = new CspParameters(1, "Microsoft Base Smart Card Crypto Provider", "XML_ENC_RSA_KEY"); // Creates new parameters for RSA //
            csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
            csp.KeyPassword = PIN;
            try
            {
                // If PIN is correct, return RSA built from the CSPParameters //
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                success = true;
                return rsa;
            }
            catch (Exception ex)
            { 
                // Cope, Seethe, Mald //
                MessageBox.Show(ex.Message);
                success = false;
                Environment.Exit(1);
                return null;
            }
        }

        // Decrypt an encrypted PKCS1 bytestream using RSA //
        public static byte[] Decrypt(byte[] ENC, SecureString PIN)
        {
            RSACryptoServiceProvider rsa = GetRSACSPSmartCard(PIN, out SUCCESS);
            return rsa.Decrypt(ENC, RSAEncryptionPadding.Pkcs1);
        }

        // Encrypt an unencrypted bytestream with PKCS1 using RSA //
        public static byte[] Encrypt(string PLAINTEXT, SecureString PIN)
        {
            RSACryptoServiceProvider rsa = GetRSACSPSmartCard(PIN, out SUCCESS);
            byte[] DATA;

            DATA = Encoding.UTF8.GetBytes(PLAINTEXT);
            return rsa.Encrypt(DATA, RSAEncryptionPadding.Pkcs1);
        }
    }
}

