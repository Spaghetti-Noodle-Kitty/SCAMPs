using System.IO;
using System.Linq;

namespace PasswordCard
{
    internal class FS
    {
        // Read the Storage-File and reverse already reversed bytes //
        public static byte[] ReadFile(string LOC)
        {
            byte[] x = File.ReadAllBytes(LOC);
            return x.Reverse().ToArray();
        }

        // Write all bytes to Storage-File. Also reverse the bytes cuz funny //
        public static void WriteFile(string LOC, byte[] ENC)
        { 
            File.WriteAllBytes(LOC, ENC.Reverse().ToArray());
        }
    }
}
