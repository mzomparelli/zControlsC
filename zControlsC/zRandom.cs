using System.Windows.Forms;
using System.Drawing;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace zControlsC
{
    public static class zRandom
    {
        static System.Random r = new System.Random(Guid.NewGuid().GetHashCode());

        //public static Random()
        //{
            
        //}
        
        public enum RandomAlphaCharCase
        {
            Upper,
            Lower,
            Random
        }

        public static bool RandomBoolean()
        {
            bool b;
            int i = RandomNumber(1, 10000);
            if (i % 2 == 0)
            {
                b = true;
            }
            else
            {
                b = false;
            }

            //if (i < 5001)
            //{
            //    b = false;
            //}
            //else
            //{
            //    b = true;
            //}

            return b;
        }

        public static string CoinFlip()
        {
            if (RandomBoolean() == true)
            {
                return "HEADS";
            }
            else
            {
                return "TAILS";
            }
        }

        public static Color RandomColor()
        {
            int a;
            int r;
            int g;
            int b;

            g = RandomNumber(0, 255);
            b = RandomNumber(0, 255);
            r = RandomNumber(0, 255);
            a = RandomNumber(0, 255);

            return Color.FromArgb(r, g, b);
        }

        public static int RandomNumber(int low, int high)
        {            
            return r.Next(low, high + 1);
        }

        public static string RandomNumberSequence(int NumberCount)
        {
            string s = "";
            for (int i = 1; i <= NumberCount; i++)
            {
                int ii = RandomNumber(0, 9);
                s = s + ii.ToString();
            }
            return s;
        }

        public static string RandomLetterSequence(int LetterCount, RandomAlphaCharCase CharCase)
        {
            string s = "";

            for (int i = 1; i <= LetterCount; i++)
            {
                string ss = ((char)(RandomNumber(1, 26) + 96)).ToString();

                switch (CharCase)
                {
                    case RandomAlphaCharCase.Lower:
                        ss = ss.ToLower();
                        break;
                    case RandomAlphaCharCase.Upper:
                        ss = ss.ToUpper();
                        break;
                    case RandomAlphaCharCase.Random:
                        bool b = RandomBoolean();
                        if (b == false)
                        {
                            ss = ss.ToLower();
                        }
                        else
                        {
                            ss = ss.ToUpper();
                        }
                        break;

                }
                s = s + ss;
            }

            return s;

        }

        public static string RandomAlphaNumericSequence(int iLength, RandomAlphaCharCase CharCase)
        {
            string s = "";
            for (int i = 1; i <= iLength; i++)
            {
                bool r = RandomBoolean();
                if (r == true)
                {
                    s = s + RandomLetterSequence(1, CharCase);
                }
                else
                {
                    s = s + RandomNumberSequence(1);
                }
            }
            return s;
        }

        public static string RandomFileName()
        {
            return System.IO.Path.GetRandomFileName();
        }


        public static string LotteryNumbers()
        {
            List<int> list = new List<int>();
            //First number
            int i = RandomNumber(1, 75);
            list.Add(i);

            //Second number
            while (list.IndexOf(i) > -1)
            {
                i = RandomNumber(1, 75);
            }
            list.Add(i);

            //Third number
            while (list.IndexOf(i) > -1)
            {
                i = RandomNumber(1, 75);
            }
            list.Add(i);

            //Fourth number
            while (list.IndexOf(i) > -1)
            {
                i = RandomNumber(1, 75);
            }
            list.Add(i);

            //Fifth number
            while (list.IndexOf(i) > -1)
            {
                i = RandomNumber(1, 75);
            }
            list.Add(i);


            list.Sort();
           StringBuilder sb = new StringBuilder();
            for (int ii=0;ii<list.Count;ii++)
            {
                if(ii==0)
                {
                    sb.Append(list[ii].ToString());
                }
                else
                {
                    sb.Append("-" + list[ii].ToString());
                }
            }

            return sb.ToString() + "-" + RandomNumber(1, 15).ToString();

        }

        public static string RandomFile(string dir, bool includeSubDirectories)
        {
            try
            {
                Dictionary<int, string> dict = GetFileDictionary(dir, includeSubDirectories);
                int tries = 0;
            TryAgain:
                if (tries > 5)
                {
                    return "";
                }
            tries++;

                string filename = dict[zControlsC.zRandom.RandomNumber(1, dict.Count)];

                if (filename == "")
                {
                    goto TryAgain;
                }
                return filename;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            

        }

        public static Dictionary<int, string> GetFileDictionary(string dir, bool includeSubDirectories)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            GetFileDictionary(dir, ref dict, includeSubDirectories);
            return dict;
        }

        public static void GetFileDictionary(string dir, ref Dictionary<int, string> dict, bool includeSubDirectories)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            foreach (FileInfo fi in d.GetFiles())
            {
                dict.Add(dict.Count + 1, fi.FullName);
            }
            if (includeSubDirectories)
            {
                foreach (DirectoryInfo di in d.GetDirectories())
                {
                    GetFileDictionary(di.FullName, ref dict, true);
                }
            }
        }


        private static int RandomFile(string dir, bool includeSubDirectories, int randomFileNumber, ref string filename, ref int count)
        {
            //fileName = "";
            DirectoryInfo diMain = new DirectoryInfo(dir);
            //int count = 1;
            foreach (FileInfo fi in diMain.GetFiles())
            {
                if (filename != "")
                {
                    break;
                }
                if (count == randomFileNumber)
                {
                    filename = fi.FullName;
                    break;
                }            
                count++;
            }

            if (includeSubDirectories)
            {
                foreach (DirectoryInfo di in diMain.GetDirectories())
                {
                    if (filename != "")
                    {
                        break;
                    }
                    RandomFile(di.FullName, true, randomFileNumber, ref filename, ref count);
                }
            }

            return count;
        }

    }
}

public class zFile

{

    public int ID = 0;
    public string FullName = "";

    public zFile()
    {

    }

    public zFile(int id, string fullname)
    {
        this.ID = id;
        this.FullName = fullname;
    }

}