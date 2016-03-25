using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;

namespace zControlsC
{
    public class zMail : IDisposable
    {

        public string Body = "";
        public string Subject = "";
        public List<string> To = new List<string>();
        public List<string> CC = new List<string>();
        public List<string> BCC = new List<string>();
        public string From = "";

        public List<string> Attachments = new List<string>();
        public List<LinkedResource> LinkedResources = new List<LinkedResource>();

        public string SMTPServer = "10.1.23.212";
        public int SMTPPort = 25;

        private MailMessage Message = new MailMessage();



        public zMail()
        {

        }

        public void UseMenloServer()
        {
            SMTPServer = "10.1.23.212";
            SMTPPort = 25;
        }

        public string BodyFromFile(string filename)
        {
            try
            {
                StreamReader reader;
                reader = File.OpenText(filename);
                Body = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                reader = null;
                return "";
            }
            catch (Exception ex)
            {

                return ex.Message;
            } 
        }

        public string AddDLFomFile(Field field, string filename)
        {
            try
            {
                switch (field)
                {
                    case Field.To:
                        To.AddRange(zControlsC.zFunctions.ReadEmail_DL_TextFile(filename));
                        return "";
                    case Field.CC:
                        CC.AddRange(zControlsC.zFunctions.ReadEmail_DL_TextFile(filename));
                        return "";
                    case Field.BCC:
                        BCC.AddRange(zControlsC.zFunctions.ReadEmail_DL_TextFile(filename));
                        return "";
                }
                return "";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            
        }

        public enum Field
        {
            To,
            CC,
            BCC
        }

        private void BuildEmail()
        {
            Message.Dispose();

            Message = new MailMessage();
            Message.From = new MailAddress(From);

            foreach (string s in To)
            {
                if (zRegexEx.IsEmailAddress(s))
                {
                    Message.To.Add(s);
                }
            }

            foreach (string s in CC)
            {
                if (zRegexEx.IsEmailAddress(s))
                {
                    Message.CC.Add(s);
                }
            }

            foreach (string s in BCC)
            {
                if (zRegexEx.IsEmailAddress(s))
                {
                    Message.Bcc.Add(s);
                }
            }

            foreach (string s in Attachments)
            {
                if (File.Exists(s))
                {
                    Message.Attachments.Add(new Attachment(s));
                }
            }

            Message.Body = Body;
            Message.Subject = Subject;
            Message.IsBodyHtml = true;

            if (LinkedResources.Count > 0)
            {
                AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                foreach (LinkedResource s in LinkedResources)
                {
                    HTMLView.LinkedResources.Add(s);
                }

                Message.AlternateViews.Add(HTMLView);
            }

            
            
        }


        public string Send()
        {
            if (From == "" || zRegexEx.IsEmailAddress(From) == false)
            {
                return "The from address is not specified or invalid.";
            }

            if (To.Count == 0 && CC.Count == 0 && BCC.Count == 0)
            {
                return "No recipients have been added to the emaill message.";
            }

            if (From == "" || !zRegexEx.IsEmailAddress(From))
            {
                return "Invalid from address";
            }

            try
            {
                BuildEmail();
                SmtpClient mailer = new SmtpClient(SMTPServer, SMTPPort);
                mailer.ServicePoint.MaxIdleTime = 1;
                mailer.Send(Message);
                return "";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            
        }

        public void Reset()
        {
            Body = "";
            Subject = "";
            To.Clear();
            CC.Clear();
            BCC.Clear();
            Attachments.Clear();
            foreach (LinkedResource l in LinkedResources)
            {
                l.Dispose();
            }
            LinkedResources.Clear();
            Message.Dispose();
            GC.Collect();
        }


        public void Dispose()
        {
            Reset();
        }


    }
}
