using System;
using System.IO;
using System.Text;
using System.Security;
using System.Windows.Forms;

namespace PasswordCard
{
    public partial class Form1 : Form
    {
        public static string DBLOC = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PASSWDSTORE"; // Password Storage Location //

        public Form1()
        {
            InitializeComponent();
            // If the "DaTaBaSe" does not exist, create a new one //
            if(!File.Exists(DBLOC))
                File.Create(DBLOC).Close();
        }

        // Checks if the Pin-TextBox is not empty //
        private bool CheckPinSet()
        {
            if (txbPin.Text.Length == 0)
                MessageBox.Show("Please enter your SmartCard Pin"); // If TextBox is empty, throw an error //
            return txbPin.Text.Length == (0);
        }

        // Build a SecureString from the PIN-Contents //
        private SecureString GetPin()
        {
            SecureString s = new System.Security.SecureString();
            foreach (char c in txbPin.Text)
                s.AppendChar(c);
            return s;
        }

        // Encrypts Contents of txbContent with Methods defined in Auth //
        private void btnEnc_Click(object sender, EventArgs e)
        {
            CheckPinSet();

            byte[] ENC = Auth.Encrypt(txbContent.Text, GetPin());
            if (Auth.SUCCESS)
            {
                FS.WriteFile(DBLOC, ENC);
                txbContent.Text = Encoding.UTF8.GetString(ENC);
            }
            else
                txbContent.Text = "";

            // Useability, blocks already used button //
            btnEnc.Enabled = false;
            btnDec.Enabled = true;
        }

        // Decrypts Contents of File with Methods defined in Auth //
        private void btnDec_Click(object sender, EventArgs e)
        {
            CheckPinSet();

            byte[] DEC = Auth.Decrypt(FS.ReadFile(DBLOC),GetPin());
            if (Auth.SUCCESS)
                txbContent.Text = Encoding.UTF8.GetString(DEC);
            else
                txbContent.Text = "";

            // Useability, blocks already used button //
            btnEnc.Enabled = true;
            btnDec.Enabled = false;
        }
    }
}
