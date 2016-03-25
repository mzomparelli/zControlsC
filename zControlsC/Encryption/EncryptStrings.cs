using System;
using System.Collections.Generic;
using System.Text;



namespace zControlsC.Encryption
{
    public static class EncryptStrings
    {
        public static string EncryptString(string PlainText)
        {
            string p = "menloMike";
            RijndaelEnhanced r = new RijndaelEnhanced(p);
            return r.Encrypt(PlainText);
        }

        public static string EncryptString(string PlainText, string Password)
        {
            RijndaelEnhanced r = new RijndaelEnhanced(Password);
            return r.Encrypt(PlainText);
        }

        public static string DecryptString(string EncryptedText)
        {
            string p = "menloMike";
            RijndaelEnhanced r = new RijndaelEnhanced(p);
            return r.Decrypt(EncryptedText);
        }

        public static string DecryptString(string EncryptedText, string Password)
        {
            RijndaelEnhanced r = new RijndaelEnhanced(Password);
            return r.Decrypt(EncryptedText);
        }


    }
}
