using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using zControlsC.Zip;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using zControlsC.Properties;

namespace zControlsC
{
    public static class zSerialize
    {

       

        //[System.ComponentModel.Description("Serializes an object with the binary formatter and compresses it with zip.")]
        //public static long SerializeCompressed(object o, string OutputFile)
        //{
        //    FileStream fs = new FileStream(OutputFile, FileMode.Create);
            
        //    try
        //    {
        //        CreateDLL();
                
        //        C1ZStreamWriter compressor = new C1ZStreamWriter(fs);
        //        BinaryFormatter bf = new BinaryFormatter();
        //        bf.Serialize(compressor, o);
        //        long fileSize = fs.Length;
        //        return fileSize;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }
            
        //}


        //[System.ComponentModel.Description("De-Serializes an object with the binary formatter and de-compresses it with zip.")]
        //public static type DeSerializeCompressed<type>(string InputFile)
        //{
        //    FileStream fs = new FileStream(InputFile, FileMode.Open);

        //    try
        //    {
        //        CreateDLL();
                
        //        C1ZStreamReader decompressor = new C1ZStreamReader(fs);
        //        BinaryFormatter bf = new BinaryFormatter();
        //        type o = (type)bf.Deserialize(decompressor);
        //        return o;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }

        //}

        private static void CreateDLL()
        {
            string dllLoc = Application.StartupPath + "\\C1.C1Zip.2.dll";

            if (!File.Exists(dllLoc))
            {
                byte[] b = Resources.C1_C1Zip_2;
                FileStream tempFile = File.Create(dllLoc);
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }
        }

        //This Serialization procedure is obsolete. Do not use it any more.
        [System.ComponentModel.Description("Serializes an object with the binary formatter")]
        public static string SerializeObject(object o, string OutputFile)
        {
            System.IO.FileStream fs;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf;
            try
            {
                System.IO.File.Delete(OutputFile);
                fs = new System.IO.FileStream(OutputFile, System.IO.FileMode.OpenOrCreate);
                bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fs, o);
                fs.Close();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        //This Serialization procedure is obsolete. Do not use it any more.
        [System.ComponentModel.Description("Deserializes an object with the binary formatter")]
        public static type DeSerializeObject<type>(string InputFile)
        {
            System.IO.FileStream fs;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf;
            type o;
            try
            {
                fs = new System.IO.FileStream(InputFile, System.IO.FileMode.Open);
                bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                o = (type)bf.Deserialize(fs);
                fs.Close();
                return o;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //return default(type);
            }

        }

    }
}

