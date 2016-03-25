using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System;

namespace zControlsC.Encryption
{
    public class EncryptFiles : IDisposable
    {
        #region "Declarations"

        //abstract frmProgress fProgress = new frmProgress();

        private bool _ShowProgress = true;
        #endregion
        #region "Properties and Structures"

        public bool ShowProgress
        {
            get { return _ShowProgress; }
            set { _ShowProgress = value; }
        }
        #endregion
        #region "Public Methods"

        public EncryptFiles()
        {

        }


        public void EncryptFile(string InputFile, string OutputFile, string Password)
        {
            byte[] bytKey;
            byte[] bytIV;
            //Send the password to the CreateKey function.
            bytKey = CreateKey(Password);
            //Send the password to the CreateIV function.
            bytIV = CreateIV(Password);
            //Start the encryption.
            EncryptOrDecryptFile(InputFile, OutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt);
        }

        public void DecryptFile(string InputFile, string OutputFile, string Password)
        {
            byte[] bytKey;
            byte[] bytIV;
            //Send the password to the CreateKey function.
            bytKey = CreateKey(Password);
            //Send the password to the CreateIV function.
            bytIV = CreateIV(Password);
            //Start the decryption.
            EncryptOrDecryptFile(InputFile, OutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt);
        }
        #endregion
        #region "Private Methods"
        #endregion
        #region "Encryption"


        System.IO.FileStream fsInput;
        System.IO.FileStream fsOutput;

        private enum CryptoAction
        {
            //Define the enumeration for CryptoAction.
            ActionEncrypt = 1,
            ActionDecrypt = 2
        }


        private byte[] CreateKey(string strPassword)
        {
            //Convert strPassword to an array and store in chrData.
            char[] chrData = strPassword.ToCharArray();
            //Use intLength to get strPassword size.
            int intLength = chrData.GetUpperBound(0);
            //Declare bytDataToHash and make it the same size as chrData.
            byte[] bytDataToHash = new byte[intLength];

            //Use For Next to convert and store chrData into bytDataToHash.
            for (int i = 0; i <= chrData.GetUpperBound(0); i++)
            {
                bytDataToHash[i] = (byte)(char)(chrData[i]);
            }

            //Declare what hash to use.
            System.Security.Cryptography.SHA512Managed SHA512 = new System.Security.Cryptography.SHA512Managed();
            //Declare bytResult, Hash bytDataToHash and store it in bytResult.
            byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
            //Declare bytKey(31).  It will hold 256 bits.
            byte[] bytKey = new byte[31];

            //Use For Next to put a specific size (256 bits) of 
            //bytResult into bytKey. The 0 To 31 will put the first 256 bits
            //of 512 bits into bytKey.
            for (int i = 0; i <= 31; i++)
            {
                bytKey[i] = bytResult[i];
            }

            return bytKey;
            //Return the key.
        }


        private byte[] CreateIV(string strPassword)
        {
            //Convert strPassword to an array and store in chrData.
            char[] chrData = strPassword.ToCharArray();
            //Use intLength to get strPassword size.
            int intLength = chrData.GetUpperBound(0);
            //Declare bytDataToHash and make it the same size as chrData.
            byte[] bytDataToHash = new byte[intLength];

            //Use For Next to convert and store chrData into bytDataToHash.
            for (int i = 0; i <= chrData.GetUpperBound(0); i++)
            {
                bytDataToHash[i] = (byte)(char)(chrData[i]);
            }

            //Declare what hash to use.
            System.Security.Cryptography.SHA512Managed SHA512 = new System.Security.Cryptography.SHA512Managed();
            //Declare bytResult, Hash bytDataToHash and store it in bytResult.
            byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
            //Declare bytIV(15).  It will hold 128 bits.
            byte[] bytIV = new byte[15];

            //Use For Next to put a specific size (128 bits) of 
            //bytResult into bytIV. The 0 To 30 for bytKey used the first 256 bits.
            //of the hashed password. The 32 To 47 will put the next 128 bits into bytIV.
            for (int i = 32; i <= 47; i++)
            {
                bytIV[i - 32] = bytResult[i];
            }

            return bytIV;
            //return the IV
        }


        private void EncryptOrDecryptFile(string strInputFile, string strOutputFile, byte[] bytKey, byte[] bytIV, CryptoAction Direction)
        {
            //In case of errors.
            try
            {

                //Setup file streams to handle input and output.
                fsInput = new System.IO.FileStream(strInputFile, FileMode.Open, FileAccess.Read);
                fsOutput = new System.IO.FileStream(strOutputFile, FileMode.OpenOrCreate, FileAccess.Write);
                fsOutput.SetLength(0);
                //make sure fsOutput is empty

                //Declare variables for encrypt/decrypt process.
                byte[] bytBuffer = new byte[4096];
                //holds a block of bytes for processing
                long lngBytesProcessed = 0;
                //running count of bytes processed
                long lngFileLength = fsInput.Length;
                //the input file's length
                int intBytesInCurrentBlock;
                //current bytes being processed
                CryptoStream csCryptoStream;
                //Declare your CryptoServiceProvider.
                System.Security.Cryptography.RijndaelManaged cspRijndael = new System.Security.Cryptography.RijndaelManaged();
                //Setup Progress Bar

                if (ShowProgress)
                {
                    //fProgress.Show();
                    //fProgress.bar.Value = 0;
                    //fProgress.bar.Maximum = 100;
                }


                //Determine if ecryption or decryption and setup CryptoStream.
                switch (Direction)
                {
                    case CryptoAction.ActionEncrypt:
                        csCryptoStream = new CryptoStream(fsOutput, cspRijndael.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write);
                        if (ShowProgress)
                        {
                            //fProgress.lblState.Text = "Encrypting File...";
                        }
                        break;

                    case CryptoAction.ActionDecrypt:
                        csCryptoStream = new CryptoStream(fsOutput, cspRijndael.CreateDecryptor(bytKey, bytIV), CryptoStreamMode.Write);
                        if (ShowProgress)
                        {
                            //fProgress.lblState.Text = "Decrypting File...";
                        }
                        break;

                    default:
                        csCryptoStream = new CryptoStream(fsOutput, cspRijndael.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write);
                        break;
                }

                //Use While to loop until all of the file is processed.
                while (lngBytesProcessed < lngFileLength)
                {
                    //Read file with the input filestream.
                    intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096);
                    //Write output file with the cryptostream.
                    csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock);
                    //Update lngBytesProcessed
                    lngBytesProcessed = lngBytesProcessed + (long)intBytesInCurrentBlock;
                    //Update Progress Bar
                    //fProgress.bar.Value = (int)(lngBytesProcessed / lngFileLength) * 100;
                    Application.DoEvents();
                }

                //Close FileStreams and CryptoStream.
                csCryptoStream.Close();
                fsInput.Close();
                fsOutput.Close();

                //If encrypting then delete the original unencrypted file.
                if (Direction == CryptoAction.ActionEncrypt)
                {
                    FileInfo fileOriginal = new FileInfo(strInputFile);
                    fileOriginal.Delete();
                }

                //If decrypting then delete the encrypted file.
                if (Direction == CryptoAction.ActionDecrypt)
                {
                    FileInfo fileEncrypted = new FileInfo(strInputFile);
                    fileEncrypted.Delete();
                }

                //Update the user when the file is done.
                string Wrap = ((char)13).ToString() + ((char)10).ToString();
                if (Direction == CryptoAction.ActionEncrypt)
                {
                    //MsgBox("Encryption Complete" + Wrap + Wrap + _
                    //        "Total bytes processed = " + _
                    //        lngBytesProcessed.ToString, _
                    //        MsgBoxStyle.Information, "Done")

                    //Update the progress bar and textboxes.
                    if (ShowProgress)
                    {
                        //fProgress.bar.Value = 0;
                        //fProgress.Close();
                    }
                }


                else
                {
                    //Update the user when the file is done.
                    //MsgBox("Decryption Complete" + Wrap + Wrap + _
                    //       "Total bytes processed = " + _
                    //        lngBytesProcessed.ToString, _
                    //        MsgBoxStyle.Information, "Done")

                    //Update the progress bar and textboxes.
                    if (ShowProgress)
                    {
                        //fProgress.bar.Value = 0;
                        //fProgress.Close();
                    }

                }
            }


            //Catch all other errors. And delete partial files.
            catch (Exception)
            {
                fsInput.Close();
                fsOutput.Close();

                if (Direction == CryptoAction.ActionDecrypt)
                {
                    FileInfo fileDelete = new FileInfo(strOutputFile);
                    fileDelete.Delete();
                    if (ShowProgress)
                    {
                        //fProgress.bar.Value = 0;
                        //fProgress.Close();
                    }


                    throw new Exception("Invalid Password");
                }

                else
                {
                    FileInfo fileDelete = new FileInfo(strOutputFile);
                    fileDelete.Delete();
                    if (ShowProgress)
                    {
                        //fProgress.bar.Value = 0;
                        //fProgress.Close();
                    }


                    throw new Exception("Invalid File");

                }

            }
        }
        #endregion


        // To detect redundant calls
        private bool disposedValue = false;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: free other state (managed objects).
                }

                // TODO: free your own state (unmanaged objects).
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }
        #region " IDisposable Support "

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}

