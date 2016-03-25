using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using zControlsC;
using System.Diagnostics;
using zControlsC.Properties;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using vb = Microsoft.VisualBasic;
using System.Net;
using zControlsC.Encryption;

namespace zControlsC
{
    public static class zFunctions
    {


        public static System.DateTime Date_EndDate(int numberToAdd, string WeekMonthYear)
        {
            switch (WeekMonthYear.ToLower())
            {
                case "day":
                case "d":
                    return Date_DayEndDate(numberToAdd);
                case "week":
                case "w":
                    return Date_WeekEndDate(numberToAdd);
                case "month":
                case "m":
                    return Date_MonthEndDate(numberToAdd);
                case "year":
                case "y":
                    return Date_YearEndDate(numberToAdd);
                default:
                    DateTime d;
                    DateTime.TryParse(@"1/1/1900", out d);
                    return d;
            }
        }

        public static System.DateTime Date_StartDate(int numberToAdd, string WeekMonthYear)
        {
            switch (WeekMonthYear.ToLower())
            {
                case "day":
                case "d":
                    return Date_DayStartDate(numberToAdd);
                case "week":
                case "w":
                    return Date_WeekStartDate(numberToAdd);
                case "month":
                case "m":
                    return Date_MonthStartDate(numberToAdd);
                case "year":
                case "y":
                    return Date_YearStartDate(numberToAdd);
                default:
                    DateTime d;
                    DateTime.TryParse(@"1/1/1900", out d);
                    return d;
            }
        }

        private static System.DateTime Date_DayEndDate(int numberToAdd)
        {
            System.DateTime d = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " 11:59:59 PM");
            return d.AddDays(numberToAdd);
        }

        private static System.DateTime Date_DayStartDate(int numberToAdd)
        {
            System.DateTime d = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " 12:00:00 AM");
            return d.AddDays(numberToAdd);
        }

        private static System.DateTime Date_WeekEndDate(int numberToAdd)
        {
            System.DateTime d = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " 11:59:59 PM");

            int iDays = 0;

            switch (d.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    iDays = 6;
                    break;
                case DayOfWeek.Monday:
                    iDays = 5;
                    break;
                case DayOfWeek.Tuesday:
                    iDays = 4;
                    break;
                case DayOfWeek.Wednesday:
                    iDays = 3;
                    break;
                case DayOfWeek.Thursday:
                    iDays = 2;
                    break;
                case DayOfWeek.Friday:
                    iDays = 1;
                    break;
                case DayOfWeek.Saturday:
                    iDays = 0;
                    break;
            }

            if (numberToAdd == 0)
            {
                //do nothing to iDays
            }
            else if (numberToAdd > 0)
            {
                for (int i = 1; i <= numberToAdd; i += 1)
                {
                    iDays += 7;
                }
            }
            else
            {
                for (int i = 1; i <= numberToAdd * -1; i += 1)
                {
                    iDays -= 7;
                }
            }


            return d.AddDays(iDays);
        }

        private static System.DateTime Date_WeekStartDate(int numberToAdd)
        {
            System.DateTime d = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " 12:00:00 AM");

            int iDays = 0;

            switch (d.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    iDays = 0;
                    break;
                case DayOfWeek.Monday:
                    iDays = -1;
                    break;
                case DayOfWeek.Tuesday:
                    iDays = -2;
                    break;
                case DayOfWeek.Wednesday:
                    iDays = -3;
                    break;
                case DayOfWeek.Thursday:
                    iDays = -4;
                    break;
                case DayOfWeek.Friday:
                    iDays = -5;
                    break;
                case DayOfWeek.Saturday:
                    iDays = -6;
                    break;
            }

            if (numberToAdd == 0)
            {
                //do nothing to iDays
            }
            else if (numberToAdd > 0)
            {
                for (int i = 1; i <= numberToAdd; i += 1)
                {
                    iDays += 7;
                }
            }
            else
            {
                for (int i = 1; i <= numberToAdd * -1; i += 1)
                {
                    iDays -= 7;
                }
            }


            return d.AddDays(iDays);


        }

        private static System.DateTime Date_MonthEndDate(int numberToAdd)
        {
            int m = DateTime.Now.Month;
            int y = DateTime.Now.Year;

            if (numberToAdd == 0)
            {
                //Do nothing to m and y
            }
            else if (numberToAdd > 0)
            {
                for (int i = 1; i <= numberToAdd; i += 1)
                {
                    if (m == 12)
                    {
                        m = 1;
                        y += 1;
                    }
                    else
                    {
                        m += 1;
                    }
                }
            }
            else
            {
                for (int i = 1; i <= numberToAdd * -1; i += 1)
                {
                    if (m == 1)
                    {
                        m = 12;
                        y -= 1;
                    }
                    else
                    {
                        m -= 1;
                    }
                }
            }

            int iDays = System.DateTime.DaysInMonth(y, m);
            return Convert.ToDateTime(m + "/" + iDays + "/" + y + " 11:59:59 PM");
        }

        private static System.DateTime Date_MonthStartDate(int numberToAdd)
        {
            int m = DateTime.Now.Month;
            int y = DateTime.Now.Year;

            if (numberToAdd == 0)
            {
                //Do nothing to m and y
            }
            else if (numberToAdd > 0)
            {
                for (int i = 1; i <= numberToAdd; i += 1)
                {
                    if (m == 12)
                    {
                        m = 1;
                        y += 1;
                    }
                    else
                    {
                        m += 1;
                    }
                }
            }
            else
            {
                for (int i = 1; i <= numberToAdd * -1; i += 1)
                {
                    if (m == 1)
                    {
                        m = 12;
                        y -= 1;
                    }
                    else
                    {
                        m -= 1;
                    }
                }
            }

            //Dim i As Integer = Date.DaysInMonth(y, m)
            return Convert.ToDateTime(m + "/1/" + y + " 12:00:00 AM");
        }

        private static System.DateTime Date_YearEndDate(int numberToAdd)
        {
            int m = DateTime.Now.Month;
            int y = DateTime.Now.Year;

            if (numberToAdd == 0)
            {
                //Do nothing to m and y
            }
            else if (numberToAdd > 0)
            {
                y += numberToAdd;
            }
            else
            {
                y -= numberToAdd * -1;
            }

            //Dim i As Integer = Date.DaysInMonth(y, m)
            return Convert.ToDateTime("12/31/" + y + " 11:59:59 PM");
        }

        private static System.DateTime Date_YearStartDate(int numberToAdd)
        {
            int m = DateTime.Now.Month;
            int y = DateTime.Now.Year;

            if (numberToAdd == 0)
            {
                //Do nothing to m and y
            }
            else if (numberToAdd > 0)
            {
                y += numberToAdd;
            }
            else
            {
                y -= numberToAdd * -1;
            }

            //Dim i As Integer = Date.DaysInMonth(y, m)
            return Convert.ToDateTime("1/1/" + y + " 12:00:00 AM");
        }



        public static int CountFiles(string dir, bool includeSubDirectories)
        {
            int count = 0;
            DirectoryInfo diMain = new DirectoryInfo(dir);
            count += diMain.GetFiles().GetLength(0);
            if (includeSubDirectories)
            {
                foreach (DirectoryInfo di in diMain.GetDirectories())
                {
                    count += CountFiles(di.FullName, true);
                }
            }

            return count;
        }

        public static int Seed()
        {
            int seed = 0;
        TryAgain:
            StringBuilder sb = new StringBuilder();
            string guid = Guid.NewGuid().ToString();

            foreach (char c in guid.Substring(0, 9))
            {
                if (Char.IsDigit(c)) { sb.Append(c); }
            }
            int i = sb.Length;
            if (i < 1) { goto TryAgain; }
            if (i > 4) { goto TryAgain; }
            int.TryParse(sb.ToString(), out seed);
            return seed;
        }

        public static NetworkCredential ReadProxyCredsFromFile(string EncryptedCredFile)
        {
            if (File.Exists(EncryptedCredFile))
            {
                NetworkCredential creds = new NetworkCredential();
                StreamReader oRead;
                oRead = File.OpenText(EncryptedCredFile);

                if (oRead.Peek() == -1) return null;
                creds.Domain = EncryptStrings.DecryptString(oRead.ReadLine());

                if (oRead.Peek() == -1) return null;
                creds.UserName = EncryptStrings.DecryptString(oRead.ReadLine());

                if (oRead.Peek() == -1) return null;
                creds.Password = EncryptStrings.DecryptString(oRead.ReadLine());
                
                oRead.Close();
                oRead.Dispose();
                return creds;
            }
            else
            {
                return null;
            }
        }

        public static List<DataTable> SplitDataTable(ref DataTable dt, int MaxRows)
        {
            List<DataTable> list = new List<DataTable>();

            int iRows = dt.Rows.Count;

            if (iRows <= MaxRows)
            {
                list.Add(dt.Copy());
                return list;
            }

            int iTables = (int)Math.Ceiling((double)(iRows / (double)MaxRows));

            for (int i = 1; i <= iTables; i++)
            {
                DataTable t = dt.Copy();
                t.Clear();

                int iRow = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    if (iRow > MaxRows)
                    {
                        iRow = 1;
                        break;
                    }
                    DataRow newRow = t.NewRow();
                    for (int x = 0; x <= dt.Columns.Count - 1; x++)
                    {
                        newRow[x] = dr[x];
                    }
                    t.Rows.Add(newRow);
                    dr.Delete();
                    iRow += 1;
                }

                dt.AcceptChanges();
                list.Add(t.Copy());
                t.Dispose();
            }

            GC.Collect();


            return list;
        }

        public static PointF[] PointsAlongLine(PointF start, PointF end, int steps)
        {
            float xDifference = (end.X - start.X);
            float yDifference = (end.Y - start.Y);
            float absoluteXdifference = (Math.Abs(start.X - end.X));
            float absoluteYdifference = (Math.Abs(start.Y - end.Y));

            double lineLength = Math.Sqrt((Math.Pow(absoluteXdifference, 2) + Math.Pow(absoluteYdifference, 2))); //pythagoras
            //int steps = lineLength / spacing;
            float xStep = xDifference / steps;
            float yStep = yDifference / steps;

            PointF[] result = new PointF[steps];

            for (int i = 0; i < steps; i++)
            {
                float x = (start.X + (xStep * i));


                float y = (start.Y + (yStep * i));
                result[i] = new PointF(x, y);
            }

            return result;
        }

        public static PointF LineCenterPoint(PointF pointA, PointF pointB)
        {
            try
            {
                return new PointF((pointA.X + pointB.X) / 2, (pointA.Y + pointB.Y) / 2);
            }
            catch (Exception)
            {
                throw;
            }


        }

        public static double LineLength(PointF pointA, PointF pointB)
        {
            double dx = Math.Abs(pointA.X - pointB.X);
            double dy = Math.Abs(pointA.Y - pointB.Y);

            return Math.Sqrt((dx * dx) + (dy * dy));

        }

        public static decimal Calculate_Weighted_Score(params Weighted_Score_Value[] values)
        {
            decimal nullCalc = 0;
            Weighted_Score_Value[] v = values;
            decimal weightSum = 0;

            foreach (Weighted_Score_Value value in v)
            {
                weightSum += value.Weight;
            }


            if (weightSum < 1 || weightSum > 1)
            {
                return -1;
            }

            foreach (Weighted_Score_Value value in v)
            {
                if (value.Value.HasValue == false)
                {
                    nullCalc += value.Weight;
                }
            }

            decimal reAllocate = nullCalc / (1 - nullCalc);

            int i = 0;
            for (i = 0; i < v.GetLength(0); i++)
            {
                v[i].ChangeWeight((v[i].Weight * reAllocate) + v[i].Weight);
            }

            decimal total = 0;

            foreach (Weighted_Score_Value value in v)
            {
                if (value.Value.HasValue == true)
                {
                    total += value.Value.Value * value.Weight;
                }
            }

            return Math.Round(total, 4);

        }

        public struct Weighted_Score_Value
        {
                public decimal? Value;
                public decimal Weight;

                public void ChangeWeight(decimal weight)
                {
                    Weight = weight;
                }
        }

        private static void KillExcelDLL()
        {
            string fName = Application.StartupPath + "\\C1.C1Excel.2.dll";

            while (File.Exists(fName))
            {
                try
                {
                    System.IO.File.Delete(fName);
                }
                catch
                {

                }

            } 
        }

        public static bool OpenEmailInEditor(string toEmailAddress, string Subject, string Body, string attachment)
        {

            bool bAns = true;
            string sParams = null;
            sParams = toEmailAddress;
            if (vb.Strings.Left(sParams, 7).ToLower() != "mailto:")
                sParams = "mailto:" + sParams;

            if (!string.IsNullOrEmpty(Subject))
                sParams = sParams + "?subject=" + Subject;

            if (!string.IsNullOrEmpty(Body))
            {
                sParams = sParams + (string.IsNullOrEmpty(Subject) ? "?" : "&");
                sParams = sParams + "body=" + Body;
            }

            if (!string.IsNullOrEmpty(attachment))
            {
                sParams = sParams + (string.IsNullOrEmpty(Subject) ? "?" : "&");
                sParams = sParams + "attachment=" + Body;
            }



            try
            {
                System.Diagnostics.Process.Start(sParams);

            }
            catch
            {
                bAns = false;
            }

            return bAns;

        }

        private static void WindowsScreenSaver(bool enableScreenSaver)
        {

            RegistryKey regKey = default(RegistryKey);

            string regVal;
            regKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

            regVal = regKey.GetValue("ScreenSaveActive").ToString();


            if (Convert.ToInt32(regVal) == (enableScreenSaver==true? 0 : 1))
            {
                regKey.SetValue("ScreenSaveActive", (enableScreenSaver == true ? 1 : 0).ToString());

            }

            regKey.Close();
        }



    #region "Send Email"

        public static List<string> ReadEmail_DL_TextFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            List<string> emails = new List<string>();
            StreamReader oRead;
            oRead = File.OpenText(filename);
            while(oRead.Peek() != -1)
            {
                string email = oRead.ReadLine();
                if (email.Substring(0, 2) == "--")
                {
                    continue;
                }
                emails.Add(email);             
            }

            oRead.Close();
            oRead.Dispose();
            oRead = null;

            return emails;
        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="attachment"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string[] bccAddress, string subject, string body, bool HTMLBody, string[] attachment)
        {

            if (!File.Exists(Application.StartupPath + "\\Emailer.exe"))
            {
                byte[] b = Resources.Emailer;
                FileStream tempFile = File.Create(Application.StartupPath + "\\Emailer.exe");
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }



            string from = (char)34 + fromAddress + (char)34 + " ";
            string to = (char)34 + "" + (char)34 + " ";
            string cc = (char)34 + "" + (char)34 + " ";
            string bcc = (char)34 + "" + (char)34 + " ";
            string emailSubject = (char)34 + "" + (char)34 + " ";
            string emailBody = (char)34 + "" + (char)34 + " ";
            string isHTML = (char)34 + "" + (char)34 + " ";
            string attachments = (char)34 + "" + (char)34 + " ";
            string view = (char)34 + "" + (char)34;


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in toAddress)
            {
                sb.Append(s + ";");
            }
            to = (char)34 + sb.ToString() + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in ccAddress)
            {
                sb.Append(s + ";");
            }
            cc = (char)34 + sb.ToString() + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in bccAddress)
            {
                sb.Append(s + ";");
            }
            bcc = (char)34 + sb.ToString() + (char)34 + " ";

            emailSubject = (char)34 + subject + (char)34 + " ";

            emailBody = (char)34 + body + (char)34 + " ";

            string bb = "";
            if (HTMLBody) { bb = "true"; } else { bb = "false"; }
            isHTML = (char)34 + bb + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in attachment)
            {
                sb.Append(s + ";");
            }
            attachments = (char)34 + sb.ToString() + (char)34 + " ";



            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Application.StartupPath + "\\Emailer.exe";
            p.StartInfo.Arguments = from + to + cc + bcc + emailSubject + emailBody + isHTML + attachments + view;
            p.Start();

            //MailMessage message = new MailMessage();
            //MailAddress from;
            //if (fromAddress == "your@valentine.4ever")
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else if (RegexEx.RegexEx.IsEmailAddress(fromAddress))
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else
            //{
            //    return "The from email address is not valid";
            //}
            
            //message.From = from;

            ////Add the TO Addresses
            //foreach (string s in toAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.To.Add(s.Trim());
            //    }
            //}

            ////Add the CC Addresses
            //foreach (string s in ccAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.CC.Add(s.Trim());
            //    }
            //}

            ////Add the BCC Addresses
            //foreach (string s in bccAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.Bcc.Add(s.Trim());
            //    }
            //}

            //message.Subject = subject;
            //message.IsBodyHtml = HTMLBody;
            //message.Body = body;

            ////Add the attachments if any
            //foreach (string s in attachment)
            //{
            //    if (System.IO.File.Exists(s))
            //    {
            //        Attachment a = new Attachment(s);
            //        message.Attachments.Add(a);
            //    }
            //}

            ////Send the email
            //try
            //{
            //    SmtpClient mailer = new SmtpClient("10.1.23.212", 25);
            //    mailer.ServicePoint.MaxIdleTime = 1;
            //    mailer.Timeout = 999999999;
            //    mailer.Send(message);
            //    mailer = null;
            //    message.Dispose();
            //    message = null;
            //}
            //catch (Exception ex)
            //{                
            //    return ex.Message;
            //}

            return "";
            

        }


        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="attachment"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string[] bccAddress, string subject, string body, bool HTMLBody, string[] attachment, AlternateView alternateView)
        {


            MailMessage message = new MailMessage();
            MailAddress from;
            if (fromAddress == "your@valentine.4ever")
            {
                from = new MailAddress(fromAddress);
            }
            else if (zRegexEx.IsEmailAddress(fromAddress))
            {
                from = new MailAddress(fromAddress);
            }
            else
            {
                return "The from email address is not valid";
            }

            message.From = from;

            //Add the TO Addresses
            foreach (string s in toAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.To.Add(s.Trim());
                }
            }

            //Add the CC Addresses
            foreach (string s in ccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.CC.Add(s.Trim());
                }
            }

            //Add the BCC Addresses
            foreach (string s in bccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.Bcc.Add(s.Trim());
                }
            }

            message.Subject = subject;
            message.IsBodyHtml = HTMLBody;
            message.Body = body;

            message.AlternateViews.Add(alternateView);


            //Add the attachments if any
            foreach (string s in attachment)
            {
                if (System.IO.File.Exists(s))
                {
                    Attachment a = new Attachment(s);
                    message.Attachments.Add(a);
                }
            }

            //Send the email
            try
            {
                SmtpClient mailer = new SmtpClient("10.1.23.212", 25);
                mailer.ServicePoint.MaxIdleTime = 1;
                mailer.Timeout = 999999999;
                mailer.Send(message);
                mailer = null;
                message.Dispose();
                message = null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";


        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="attachment"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string subject, string body, bool HTMLBody, string[] attachment)
        {


            if (!File.Exists(Application.StartupPath + "\\Emailer.exe"))
            {
                byte[] b = Resources.Emailer;
                FileStream tempFile = File.Create(Application.StartupPath + "\\Emailer.exe");
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }



            string from = (char)34 + fromAddress + (char)34 + " ";
            string to = (char)34 + "" + (char)34 + " ";
            string cc = (char)34 + "" + (char)34 + " ";
            string bcc = (char)34 + "" + (char)34 + " ";
            string emailSubject = (char)34 + "" + (char)34 + " ";
            string emailBody = (char)34 + "" + (char)34 + " ";
            string isHTML = (char)34 + "" + (char)34 + " ";
            string attachments = (char)34 + "" + (char)34 + " ";
            string view = (char)34 + "" + (char)34;


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in toAddress)
            {
                sb.Append(s + ";");
            }
            to = (char)34 + sb.ToString() + (char)34 + " ";


            emailSubject = (char)34 + subject + (char)34 + " ";

            emailBody = (char)34 + body + (char)34 + " ";

            string bb = "";
            if (HTMLBody) { bb = "true"; } else { bb = "false"; }
            isHTML = (char)34 + bb + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in attachment)
            {
                sb.Append(s + ";");
            }
            attachments = (char)34 + sb.ToString() + (char)34 + " ";



            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Application.StartupPath + "\\Emailer.exe";
            p.StartInfo.Arguments = from + to + cc + bcc + emailSubject + emailBody + isHTML + attachments + view;
            p.Start();


            //MailMessage message = new MailMessage();
            //MailAddress from;
            //if (fromAddress == "your@valentine.4ever")
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else if (RegexEx.RegexEx.IsEmailAddress(fromAddress))
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else
            //{
            //    return "The from email address is not valid";
            //}

            //message.From = from;

            ////Add the TO Addresses
            //foreach (string s in toAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.To.Add(s.Trim());
            //    }
            //}
            
            //message.Subject = subject;
            //message.IsBodyHtml = HTMLBody;
            //message.Body = body;

            ////Add the attachments if any
            //foreach (string s in attachment)
            //{
            //    if (System.IO.File.Exists(s))
            //    {
            //        Attachment a = new Attachment(s);
            //        message.Attachments.Add(a);
            //    }
            //}

            ////Send the email
            //try
            //{
            //    SmtpClient mailer = new SmtpClient("10.1.23.212", 25);
            //    mailer.ServicePoint.MaxIdleTime = 1;
            //    mailer.Timeout = 999999999;
            //    mailer.Send(message);
            //    mailer = null;
            //    message.Dispose();
            //    message = null;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}

            return "";

        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string[] bccAddress, string subject, string body, bool HTMLBody)
        {


            if (!File.Exists(Application.StartupPath + "\\Emailer.exe"))
            {
                byte[] b = Resources.Emailer;
                FileStream tempFile = File.Create(Application.StartupPath + "\\Emailer.exe");
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }



            string from = (char)34 + fromAddress + (char)34 + " ";
            string to = (char)34 + "" + (char)34 + " ";
            string cc = (char)34 + "" + (char)34 + " ";
            string bcc = (char)34 + "" + (char)34 + " ";
            string emailSubject = (char)34 + "" + (char)34 + " ";
            string emailBody = (char)34 + "" + (char)34 + " ";
            string isHTML = (char)34 + "" + (char)34 + " ";
            string attachments = (char)34 + "" + (char)34 + " ";
            string view = (char)34 + "" + (char)34;


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in toAddress)
            {
                sb.Append(s + ";");
            }
            to = (char)34 + sb.ToString() + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in ccAddress)
            {
                sb.Append(s + ";");
            }
            cc = (char)34 + sb.ToString() + (char)34 + " ";


            sb = new System.Text.StringBuilder();
            foreach (string s in bccAddress)
            {
                sb.Append(s + ";");
            }
            bcc = (char)34 + sb.ToString() + (char)34 + " ";

            emailSubject = (char)34 + subject + (char)34 + " ";

            emailBody = (char)34 + body + (char)34 + " ";

            string bb = "";
            if (HTMLBody) { bb = "true"; } else { bb = "false"; }
            isHTML = (char)34 + bb + (char)34 + " ";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Application.StartupPath + "\\Emailer.exe";
            p.StartInfo.Arguments = from + to + cc + bcc + emailSubject + emailBody + isHTML + attachments + view;
            p.Start();


            //MailMessage message = new MailMessage();
            //MailAddress from;
            //if (fromAddress == "your@valentine.4ever")
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else if (RegexEx.RegexEx.IsEmailAddress(fromAddress))
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else
            //{
            //    return "The from email address is not valid";
            //}

            //message.From = from;

            ////Add the TO Addresses
            //foreach (string s in toAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.To.Add(s.Trim());
            //    }
            //}

            ////Add the CC Addresses
            //foreach (string s in ccAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.CC.Add(s.Trim());
            //    }
            //}

            ////Add the BCC Addresses
            //foreach (string s in bccAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.Bcc.Add(s.Trim());
            //    }
            //}

            //message.Subject = subject;
            //message.IsBodyHtml = HTMLBody;
            //message.Body = body;

            
            ////Send the email
            //try
            //{
            //    SmtpClient mailer = new SmtpClient("10.1.23.212", 25);
            //    mailer.ServicePoint.MaxIdleTime = 1;
            //    mailer.Timeout = 999999999;
            //    mailer.Send(message);
            //    mailer = null;
            //    message.Dispose();
            //    message = null;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}

            return "";

        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string subject, string body, bool HTMLBody)
        {

            if (!File.Exists(Application.StartupPath + "\\Emailer.exe"))
            {
                byte[] b = Resources.Emailer;
                FileStream tempFile = File.Create(Application.StartupPath + "\\Emailer.exe");
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }



            string from = (char)34 + fromAddress + (char)34 + " ";
            string to = (char)34 + "" + (char)34 + " ";
            string cc = (char)34 + "" + (char)34 + " ";
            string bcc = (char)34 + "" + (char)34 + " ";
            string emailSubject = (char)34 + "" + (char)34 + " ";
            string emailBody = (char)34 + "" + (char)34 + " ";
            string isHTML = (char)34 + "" + (char)34 + " ";
            string attachments = (char)34 + "" + (char)34 + " ";
            string view = (char)34 + "" + (char)34;


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in toAddress)
            {
                sb.Append(s + ";");
            }
            to = (char)34 + sb.ToString() + (char)34 + " ";


            emailSubject = (char)34 + subject + (char)34 + " ";

            emailBody = (char)34 + body + (char)34 + " ";

            string bb = "";
            if (HTMLBody) { bb = "true"; } else { bb = "false"; }
            isHTML = (char)34 + bb + (char)34 + " ";


            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Application.StartupPath + "\\Emailer.exe";
            p.StartInfo.Arguments = from + to + cc + bcc + emailSubject + emailBody + isHTML + attachments + view;
            p.Start();


            //MailMessage message = new MailMessage();
            //MailAddress from;
            //if (fromAddress == "your@valentine.4ever")
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else if (RegexEx.RegexEx.IsEmailAddress(fromAddress))
            //{
            //    from = new MailAddress(fromAddress);
            //}
            //else
            //{
            //    return "The from email address is not valid";
            //}

            //message.From = from;

            ////Add the TO Addresses
            //foreach (string s in toAddress)
            //{
            //    if (RegexEx.RegexEx.IsEmailAddress(s.Trim()))
            //    {
            //        message.To.Add(s.Trim());
            //    }
            //}

           

            //message.Subject = subject;
            //message.IsBodyHtml = HTMLBody;
            //message.Body = body;

            
            ////Send the email
            //try
            //{
            //    SmtpClient mailer = new SmtpClient("10.1.23.212", 25);
            //    mailer.ServicePoint.MaxIdleTime = 1;
            //    mailer.Timeout = 999999999;
            //    mailer.Send(message);
            //    mailer = null;
            //    message.Dispose();
            //    message = null;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}

            return "";

        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="attachment"></param>
        /// <param name="SMTP_Server"></param>
        /// <param name="SMTP_Port"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string[] bccAddress, string subject, string body, bool HTMLBody, string[] attachment, string SMTP_Server, int SMTP_Port)
        {

            MailMessage message = new MailMessage();
            MailAddress from;
            if (fromAddress == "your@valentine.4ever")
            {
                from = new MailAddress(fromAddress);
            }
            else if (zRegexEx.IsEmailAddress(fromAddress))
            {
                from = new MailAddress(fromAddress);
            }
            else
            {
                return "The from email address is not valid";
            }

            message.From = from;

            //Add the TO Addresses
            foreach (string s in toAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.To.Add(s.Trim());
                }
            }

            //Add the CC Addresses
            foreach (string s in ccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.CC.Add(s.Trim());
                }
            }

            //Add the BCC Addresses
            foreach (string s in bccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.Bcc.Add(s.Trim());
                }
            }

            message.Subject = subject;
            message.IsBodyHtml = HTMLBody;
            message.Body = body;

            //Add the attachments if any
            foreach (string s in attachment)
            {
                if (System.IO.File.Exists(s))
                {
                    Attachment a = new Attachment(s);
                    message.Attachments.Add(a);
                }
            }

            //Send the email
            try
            {
                SmtpClient mailer = new SmtpClient(SMTP_Server, SMTP_Port);
                mailer.ServicePoint.MaxIdleTime = 1;
                mailer.Timeout = 999999999;
                mailer.Send(message);
                mailer = null;
                message.Dispose();
                message = null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";


        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="SMTP_Server"></param>
        /// <param name="SMTP_Port"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string[] ccAddress, string[] bccAddress, string subject, string body, bool HTMLBody, string SMTP_Server, int SMTP_Port)
        {

            MailMessage message = new MailMessage();
            MailAddress from;
            if (fromAddress == "your@valentine.4ever")
            {
                from = new MailAddress(fromAddress);
            }
            else if (zRegexEx.IsEmailAddress(fromAddress))
            {
                from = new MailAddress(fromAddress);
            }
            else
            {
                return "The from email address is not valid";
            }

            message.From = from;

            //Add the TO Addresses
            foreach (string s in toAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.To.Add(s.Trim());
                }
            }

            //Add the CC Addresses
            foreach (string s in ccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.CC.Add(s.Trim());
                }
            }

            //Add the BCC Addresses
            foreach (string s in bccAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.Bcc.Add(s.Trim());
                }
            }

            message.Subject = subject;
            message.IsBodyHtml = HTMLBody;
            message.Body = body;

            
            //Send the email
            try
            {
                SmtpClient mailer = new SmtpClient(SMTP_Server, SMTP_Port);
                mailer.ServicePoint.MaxIdleTime = 1;
                mailer.Timeout = 999999999;
                mailer.Send(message);
                mailer = null;
                message.Dispose();
                message = null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";

        }

        /// <summary>
        /// Sends an email using Menlo's SMTP server.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="HTMLBody"></param>
        /// <param name="SMTP_Server"></param>
        /// <param name="SMTP_Port"></param>
        private static string SendEmail(string fromAddress, string[] toAddress, string subject, string body, bool HTMLBody, string SMTP_Server, int SMTP_Port)
        {

            MailMessage message = new MailMessage();
            MailAddress from;
            if (fromAddress == "your@valentine.4ever")
            {
                from = new MailAddress(fromAddress);
            }
            else if (zRegexEx.IsEmailAddress(fromAddress))
            {
                from = new MailAddress(fromAddress);
            }
            else
            {
                return "The from email address is not valid";
            }

            message.From = from;

            //Add the TO Addresses
            foreach (string s in toAddress)
            {
                if (zRegexEx.IsEmailAddress(s.Trim()))
                {
                    message.To.Add(s.Trim());
                }
            }

            
            message.Subject = subject;
            message.IsBodyHtml = HTMLBody;
            message.Body = body;

            
            //Send the email
            try
            {
                SmtpClient mailer = new SmtpClient(SMTP_Server, SMTP_Port);
                mailer.ServicePoint.MaxIdleTime = 1;
                mailer.Timeout = 999999999;
                mailer.Send(message);
                mailer = null;
                message.Dispose();
                message = null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";

        }

    #endregion

        private static void ExportUserGroup(string groupName, string exportLocation)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = String.Format(@"/c net localgroup {0} > {1}{0}.txt", groupName, exportLocation);
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;

            p.Start();
        }

        private struct ColumnInfo
        {
            public string ColumnName;
            public int MaxOfLength;
            public int Rows_For_Max;
            public int MajorityLength;
            public int Rows_For_Majority;
            public float MajorityPercent;
            public int NullRows;
            public float NullPercent;
            public int TotalRows;
            public long TotalChars;

            public float Bytes
            {
                get { return (float)(TotalChars * 2) + 2 + (float)(NullRows * 2); }
            }

            public float Kilobytes
            {
                get { return (float)(Bytes / 1024); }
            }

            public float Megabytes
            {
                get { return (float)(Kilobytes / 1024); }
            }

        }

    }
}
