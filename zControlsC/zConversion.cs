using System.Text;
using zControlsC;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections.Generic;



namespace zControlsC
{

    public static class zConversion
    {

     



        public static string ExcelSerialToDateString(double ExcelSerialDate)
        {
            DateTime d = Convert.ToDateTime(@"12/30/1899");
            d = d.AddDays(ExcelSerialDate);
            return d.Month + "/" + d.Day + "/" + d.Year + " " + d.Hour + ":" + AddZero(d.Minute);
        }

        public static int MinutesToMilliseconds(int minutes)
        {
            if (minutes < 1)
            {
                return 0;
            }
            else
            {
                return ((minutes * 60) * 1000);
            }
        }

            /* Many map projections work with coordinates in the form of radians instead of degrees.
             * These constants will help us convert back and forth.
             */
            private const double DEGREEStoRADIANS = Math.PI / 180;
            private const double RADIANStoDEGREES = 180 / Math.PI;

            /* These values represent the equatorial radius of the WGS84 ellipsoid in meters.
             * resulting in projected coordinates which are also in meters
             */
            private const double WGS84SEMIMAJOR = 6378137.0;
            private const double ONEOVERWGS84SEMIMAJOR = 1.0 / WGS84SEMIMAJOR;


            /// <summary>
            /// Converts a geographic coordinate into a projected coordinate
            /// </summary>
            /// <param name="geographicCoordinate">A <strong>PointF</strong> object, representing a geographic coordinate.</param>
            /// <returns>A projected coordinate in the form of a <strong>PointF</strong> object.</returns>
            public static PointF Project(PointF geographicCoordinate)
            {
                // Make a new Point object
                PointF result = new PointF();

                // Calculate the projected X coordinate
                result.X = (float)(geographicCoordinate.X * DEGREEStoRADIANS * Math.Cos(0) * WGS84SEMIMAJOR);

                // Calculate the projected Y coordinate
                result.Y = (float)(geographicCoordinate.Y * DEGREEStoRADIANS * WGS84SEMIMAJOR);

                // Return the result
                return result;
            }

            /// <summary>
            /// Converts a projected coordinate to a geographic coordinate
            /// </summary>
            /// <param name="projectedCoordinate">A <strong>PointF</strong> object, representing a projected coordinate.</param>
            /// <returns>A geographic coordinate in the form of a <strong>PointF</strong> object.</returns>
            public static PointF Deproject(PointF projectedCoordinate)
            {
                // Make a new point to store the result
                PointF result = new PointF();

                // Calculate the geographic X coordinate (longitude)
                result.X = (float)(projectedCoordinate.X * ONEOVERWGS84SEMIMAJOR / Math.Cos(0) * RADIANStoDEGREES);

                // Calculate the geographic Y coordinate (latitude)
                result.Y = (float)(projectedCoordinate.Y * ONEOVERWGS84SEMIMAJOR * RADIANStoDEGREES);

                return result;
            }


            /// <summary>
            /// Round a decimal number down
            /// </summary>
        public static decimal RoundDown(decimal number)
        {
            decimal floorValue = Math.Floor(number);
            if ((number <= 1) && (number > 0))
            {
                return floorValue + 1;
            }
            else
            {
                return floorValue;
            }
        }

        /// <summary>
        /// Round a decimal number up
        /// </summary>
        public static decimal RoundUp(decimal number)
        {
            return Math.Ceiling(number);
        }

        /// <summary>
        /// Used for creating charts
        /// </summary>
        public static int PixelsFromChartValue(decimal chartValue, decimal chartMax, decimal chartMin, int size)
        {
            int p = 0;

            try
            {
                p = (int)((size / (chartMax - chartMin)) * (chartValue - chartMin));
            }
            catch (Exception)
            {
                return -1;
            }

            return p;
        }

        /// <summary>
        /// Returns a file's full path and filename without the extension
        /// </summary>
        public static string FilePath_FileNameNoExtension(string fileName)
        {
            return Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName);
        }


        /// <summary>
        /// Swaps two generic object variables
        /// </summary>
        /// <param name="object1">Object 1 - This object becomes object 2 after the method executes.</param>
        /// <param name="object2">Object 2 - This object becomes object 1 after the method executes.</param>
        public static void Swap<type>(ref type object1, ref type object2)
        {
            type a = object1;
            object1 = object2;
            object2 = a;
        }



        public enum TimeZones
        {
            Hawaii = -10,
            Alaska = -8,
            Pacific = -8,
            Mountain = -7,
            Central = -6,
            Eastern = -5,
            Atlantic = -4,
            UTC = 0,
            GMT = 0
        }

        /// <summary>
        /// Convert a date and time to another time zone.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <param name="fromTimeZoneCode">Proprietary code of any of the following (HT, AKT, PT, MTN, MT, CTN, CT, ETN, ET, ATN, AT, AST, BT, CET, CVT, EAT, GT, HKT, ILT, INT, IRT, JT, NET, NIT, NZT, PKT, TT, UKT, UTC, WAT) that represents the time zone of the date parameter.</param>
        /// <param name="toTimeZone">The time zone to convert to.</param>
        public static DateTime ConvertDateTime(DateTime date, string fromTimeZoneCode, TimeZones toTimeZone)
        {
	        TimeZone localZone = TimeZone.CurrentTimeZone;
	        bool isDST = localZone.IsDaylightSavingTime(date);

            Assembly _assembly;
            _assembly = Assembly.GetExecutingAssembly();

            StreamReader _textStreamReader;
            _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("zControlsC.TimeZones_Menlo_Codes.txt"));

            List<string> codes = new List<string>();
            List<float> utcOffset = new List<float>();
            List<int> dst = new List<int>();

            while (_textStreamReader.Peek() != -1)
            {
                string[] s = _textStreamReader.ReadLine().Split(',');
                codes.Add((string)s[0]);
                utcOffset.Add(float.Parse(s[1]));
                dst.Add(Int32.Parse(s[2]));
            }
	        
	        float HoursToAdd = 0;
            bool found = false;
            int i = 0;


            foreach (string code in codes)
            {
                if (code == fromTimeZoneCode)
                {
                    HoursToAdd = (float)toTimeZone - utcOffset[i];
                    if (((isDST) && (dst[i] == 0)) || ((isDST) && (toTimeZone == TimeZones.UTC)))   //Don't add an hour for UTC since it does not support DST
                    {
                        HoursToAdd++;
                    }
                       
                    found = true;
                    break;
                }
                i++;
            }

            _textStreamReader.Close();
            _textStreamReader.Dispose();


            if (!found)
            {
                return date;
            }
            else
            {
                return date.AddHours(HoursToAdd);
            }
		
	
        }



        /// <summary>
        /// Adds a zero to a one digit number to make it two digits
        /// </summary>
        /// <param name="i">The integer to add zero to.</param>
        public static string AddZero(int i)
        {
            if (i >= 0 & i < 10)
            {
                return @"0" + i.ToString();
            }
            else
            {
                return i.ToString();
            }
        }

        /// <summary>
        /// Adds zeros to a one digit number to make it the specified number of digits.
        /// </summary>
        /// <param name="number">The integer to add zero to.</param>
        /// <param name="digits">The number of digits the final string should be.</param>
        public static string AddZeros(int number, int digits)
        {
            int numLen = number.ToString().Length;
            

            if (numLen > digits)
            {
                return "Error";
            }
            else if (numLen == digits)
            {
                return number.ToString();
            }
            else
            {
                int diff = digits - numLen;
                string numString = "";

                for (int j = 1; j == diff; j++)
                {
                    numString = numString + "0";
                }

                return numString + number.ToString();

            }

        }

        /// <summary>
        /// Replaces any apostrophe's to avoid SQL injection attacks
        /// </summary>
        /// <param name="s">The string to fix.</param>
        public static string SQLReplace(string s)
        {
            s = s.Replace("'", "''");
            return s;
        }

        /// <summary>
        /// Returns empty string or zero if object passed is null
        /// </summary>
        public static object nz(object o, bool ToString)
        {
            try
            {
                if ((System.Convert.IsDBNull(o) || o == null || o.ToString() == ""))
                {
                    if (ToString == true)
                    {
                        return "";
                    }
                    else
                    {
                        return 0;
                    }
                    
                }
                else
                {
                    return o;
                }
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }



        /// <summary>
        /// Returns HTML representation of a DataTable
        /// </summary>
        public static string DataTableToHTML(DataTable dt)
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("<table class='styledTable' cellspacing='0' border='1' width='100%'>");
            s.AppendLine("<tbody align='center' style='font-family:verdana; color:black; background-color:yellow'>");
            s.AppendLine("<tr>");
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                s.AppendLine("<b><td BGCOLOR='#000080'><FONT COLOR='#FFFFFF'>" + dt.Columns[i].ColumnName + "</FONT></td></b>");
            }
            s.AppendLine("</tr>");
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                s.AppendLine("<tr>");
                for (int ii = 0; ii <= dt.Columns.Count - 1; ii++)
                {
                    s.AppendLine("<td>" + dt.Rows[i][ii].ToString() + "</td>");
                }
                s.AppendLine("</tr>");
            }
            s.AppendLine("</tbody>");
            s.AppendLine("</table>");
            return s.ToString();

        }

        public enum IntegerToMonthStringOptions
        {
            FullMonth,
            Abbreviation
        }

        /// <summary>
        /// Returns the Excel column letter for the passed column number
        /// </summary>
        public static string ExcelColumnLetter(int ColumnNumber)
        {
            int CumulativeSum;
            int StringPosition;
            int i;
            int Modulus;
            string TempString;
            int PartialValue;

            try
            {
                if (ColumnNumber < 1)
                {
                    return "";
                }
                else
                {
                    StringPosition = 0;
                    CumulativeSum = 0;
                    TempString = "";
                    do
                    {
                        PartialValue = (int)((ColumnNumber - CumulativeSum - 1) / (Math.Pow(26, StringPosition)));
                        Modulus = PartialValue - (int)(PartialValue / 26) * 26;
                        TempString = (char)(Modulus + 65) + TempString;
                        StringPosition = StringPosition + 1;
                        CumulativeSum = 0;
                        for (i = 1; i <= StringPosition; i++)
                        {
                            CumulativeSum = (CumulativeSum + 1) * 26;
                        }
                    }
                    while (ColumnNumber > CumulativeSum);
                    return TempString;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the Excel column number for the passed column letter
        /// </summary>
        public static long ExcelColumnNumber(string ColumnLetter)
        {

            int intCount;
            int intColumnLetterLength;

            int i = 0;
            intColumnLetterLength = ColumnLetter.Length;
            for (intCount = 0; intCount <= intColumnLetterLength - 1; intCount++)
            {
                char[] c = ColumnLetter.Substring(intCount, 1).ToUpper().ToCharArray();
                i = i * 26 + (int)c[0] - 64;
            }
            return i;

        }


        public static object ConvertToDatabaseType(ref DataColumn c, String value)
        {
            //if (c == null)
            //{
            //    System.Windows.Forms.MessageBox.Show("DataColumn is null");
            //}

            if (c.DataType == System.Type.GetType("System.Boolean"))
            {
                if(value.ToLower() == "true")
                {
                    return true;
                }
                else if (value.ToLower() == "false")
                {
                    return false;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Byte"))
            {
                try
                {
                    return Convert.ToByte(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Char"))
            {
                try
                {
                    return Convert.ToChar(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.DateTime"))
            {
                try
                {
                    return Convert.ToDateTime(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Decimal"))
            {
                try
                {
                    return Convert.ToDecimal(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Double"))
            {
                try
                {
                    return Convert.ToDouble(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Int16"))
            {
                try
                {
                    return Convert.ToInt16(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Int32"))
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Int64"))
            {
                try
                {
                    return Convert.ToInt64(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.SByte"))
            {
                try
                {
                    return Convert.ToSByte(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.Single"))
            {
                try
                {
                    return Convert.ToSingle(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.String"))
            {
                try
                {
                    return value;
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.TimeSpan"))
            {
                try
                {
                    return value;
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.UInt16"))
            {
                try
                {
                    return Convert.ToUInt16(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.UInt32"))
            {
                try
                {
                    return Convert.ToUInt32(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else if (c.DataType == System.Type.GetType("System.uInt64"))
            {
                try
                {
                    return Convert.ToUInt64(value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            else
            {
                return value;
            }
        }



        /// <summary>
        /// Returns a month string for a passed month number
        /// </summary>
        public static string IntegerToMonthString(int i, IntegerToMonthStringOptions options)
        {
            switch (options)
            {
                case IntegerToMonthStringOptions.Abbreviation:
                    switch (i)
                    {
                        case 1:
                            return "Jan";
                        case 2:
                            return "Feb";
                        case 3:
                            return "Mar";
                        case 4:
                            return "Apr";
                        case 5:
                            return "May";
                        case 6:
                            return "Jun";
                        case 7:
                            return "Jul";
                        case 8:
                            return "Aug";
                        case 9:
                            return "Sep";
                        case 10:
                            return "Oct";
                        case 11:
                            return "Nov";
                        case 12:
                            return "Dec";
                        default:
                            return "";
                    }
                case IntegerToMonthStringOptions.FullMonth:
                    switch (i)
                    {
                        case 1:
                            return "January";
                        case 2:
                            return "February";
                        case 3:
                            return "March";
                        case 4:
                            return "April";
                        case 5:
                            return "May";
                        case 6:
                            return "June";
                        case 7:
                            return "July";
                        case 8:
                            return "August";
                        case 9:
                            return "September";
                        case 10:
                            return "October";
                        case 11:
                            return "November";
                        case 12:
                            return "December";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }

        public enum ParsePhoneNumberOptions
        {
            DigitsOnly,
            Formatted,
            AreaCode,
            Prefix,
            Suffix
        }

        public enum ParseEmailAddressOptions
        {
            MailboxName,
            DomainName,
            DomainExtension,
            DomainNameAndExtension
        }

        public enum ParsePostalCodeOptions
        {
            Zipcode,
            POBox_US_Only,
            ThreeZip,
            FullZip_NoSpacesNoHyphens,
            FullZip_Formatted,
            Full_State_Name,
            State_Abbreviation
        }

        /// <summary>
        /// Rerturns any given portion of a phone number
        /// </summary>
        public static string ParsePhoneNumber(string s, ParsePhoneNumberOptions options)
        {
            if (zControlsC.zRegexEx.IsPhoneNumber(s))
            {
                Match m = Regex.Match(s, zControlsC.zRegexEx.REGEXType.REGEX_PHONENUMBER);
                switch (options)
                {
                    case ParsePhoneNumberOptions.DigitsOnly:
                        return m.Groups["AreaCode"].ToString() + m.Groups["Prefix"].ToString() + m.Groups["Suffix"].ToString();
                    case ParsePhoneNumberOptions.Formatted:
                        return string.Format("({0}) {1}-{2}", m.Groups["AreaCode"], m.Groups["Prefix"], m.Groups["Suffix"]);
                    case ParsePhoneNumberOptions.AreaCode:
                        return m.Groups["AreaCode"].ToString();
                    case ParsePhoneNumberOptions.Prefix:
                        return m.Groups["Prefix"].ToString();
                    case ParsePhoneNumberOptions.Suffix:
                        return m.Groups["Suffix"].ToString();
                    default:
                        return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns any given portion of an email address
        /// </summary>
        public static string ParseEmailAddress(string s, ParseEmailAddressOptions options)
        {
            if (zControlsC.zRegexEx.IsEmailAddress(s))
            {
                Match m = Regex.Match(s, zControlsC.zRegexEx.REGEXType.REGEX_EMAIL);
                switch (options)
                {
                    case ParseEmailAddressOptions.DomainExtension:
                        return m.Groups["DomainExtension"].ToString();
                    case ParseEmailAddressOptions.DomainName:
                        return m.Groups["DomainName"].ToString();
                    case ParseEmailAddressOptions.DomainNameAndExtension:
                        return m.Groups["DomainName"].ToString() + m.Groups["DomainExtension"].ToString();
                    case ParseEmailAddressOptions.MailboxName:
                        return m.Groups["Username"].ToString();
                    default:
                        return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns any given portion of a US zipcode
        /// </summary>
        public static string ParsePostalCode(string s, ParsePostalCodeOptions options)
        {
            int number;
            bool result;

            if (zControlsC.zRegexEx.IsPostalCode(s))
            {
                Match m = System.Text.RegularExpressions.Regex.Match(s, zControlsC.zRegexEx.REGEXType.REGEX_ZIPCODE);
                string zip; //= IIf(m.Groups("Zip").ToString == "", m.Groups("ThreeZip").ToString + m.Groups("LastThree").ToString, m.Groups("Zip").ToString);
                if (m.Groups["Zip"].ToString() == "")
                {
                    zip = m.Groups["ThreeZip"].ToString() + m.Groups["LastThree"].ToString();
                }
                else
                {
                    zip = m.Groups["Zip"].ToString();
                }
                
                switch (options)
                {
                    case ParsePostalCodeOptions.ThreeZip:
                        return m.Groups["ThreeZip"].ToString();
                    case ParsePostalCodeOptions.FullZip_Formatted:
                        if (m.Groups["POBox"].ToString() == "")
                        {
                            return zip;
                        }
                        else
                        {
                            return zip + " " + m.Groups["POBox"].ToString();
                        }
                    case ParsePostalCodeOptions.FullZip_NoSpacesNoHyphens:
                        return zip + m.Groups["POBox"].ToString();
                    case ParsePostalCodeOptions.POBox_US_Only:
                        return m.Groups["POBox"].ToString();
                    case ParsePostalCodeOptions.Zipcode:
                        return zip;
                    case ParsePostalCodeOptions.State_Abbreviation:
                        result = Int32.TryParse(zip, out number);
                        if (result)
                        {
                            return GetState(number, GetStateOptions.Abbreviation);
                        }
                        else
                        {
                            return "Non U.S. Postal Code";
                        }


                    case ParsePostalCodeOptions.Full_State_Name:
                        result = Int32.TryParse(zip, out number);
                        if (result)
                        {
                            return GetState(number, GetStateOptions.FullName);
                        }
                        else
                        {
                            return "Non U.S. Postal Code";
                        }


                    default:
                        return "";
                }
            }
            else
            {
                return "";
            }
        }

        public enum GetStateOptions
        {
            FullName,
            Abbreviation
        }

        /// <summary>
        /// Returns a US State for a passed zipcode
        /// </summary>
        public static string GetState(int Zipcode, GetStateOptions options)
        {

            string StateCode = "";
            if (((Zipcode >= 1000) && (Zipcode <= 2799)) || ((Zipcode >= 5500) && (Zipcode <= 5599)))
            {
                StateCode = "MA";
            }
            else if ((Zipcode >= 2800) && (Zipcode <= 2999))
            {
                StateCode = "RI";
            }
            else if ((Zipcode >= 3000) && (Zipcode <= 3899))
            {
                StateCode = "NH";
            }
            else if ((Zipcode >= 3900) && (Zipcode <= 4999))
            {
                StateCode = "ME";
            }
            else if ((Zipcode >= 5000) && (Zipcode <= 5999))
            {
                StateCode = "VT";
            }
            else if ((Zipcode >= 6000) && (Zipcode <= 6999))
            {
                StateCode = "CT";
            }
            else if ((Zipcode >= 7000) && (Zipcode <= 8999))
            {
                StateCode = "NJ";
            }
            else if (((Zipcode >= 9000) && (Zipcode <= 14999)) || ((Zipcode >= 400) && (Zipcode <= 499)) || (((Zipcode >= 6300) && (Zipcode <= 6399))))
            {
                StateCode = "NY";
            }
            else if ((Zipcode >= 15000) && (Zipcode <= 19699))
            {
                StateCode = "PA";
            }
            else if ((Zipcode >= 19700) && (Zipcode <= 19999))
            {
                StateCode = "DE";
            }
            else if ((Zipcode >= 20000) && (Zipcode <= 24699))
            {
                if ((Zipcode >= 22000) && (Zipcode <= 24699))
                {
                    StateCode = "VA";
                }
                else if ((Zipcode >= 20100) && (Zipcode <= 20199))
                {
                    StateCode = "VA";
                }
                else if ((Zipcode >= 20000) && (Zipcode <= 20599))
                {
                    StateCode = "DC";
                }
            }
            else if ((Zipcode >= 20600) && (Zipcode <= 21999))
            {
                StateCode = "MD";
            }
            else if ((Zipcode >= 24700) && (Zipcode <= 26899))
            {
                StateCode = "WV";
            }
            else if ((Zipcode >= 26900) && (Zipcode <= 28999))
            {
                StateCode = "NC";
            }
            else if ((Zipcode >= 29000) && (Zipcode <= 29999))
            {
                StateCode = "SC";
            }
            else if ((Zipcode >= 30000) && (Zipcode <= 31999))
            {
                StateCode = "GA";
            }
            else if ((Zipcode >= 32000) && (Zipcode <= 34999))
            {
                if (!(Zipcode >= 34300) && (Zipcode <= 34399))
                {
                    if (!(Zipcode >= 34500) && !(Zipcode <= 34599))
                    {
                        if (!(Zipcode >= 34800) && !(Zipcode <= 34899))
                        {
                            StateCode = "FL";
                        }
                    }
                }
            }
            else if ((Zipcode >= 34000) && (Zipcode <= 34099))
            {
                StateCode = "AA";
            }
            else if ((Zipcode >= 35000) && (Zipcode <= 36999))
            {
                StateCode = "AL";
            }
            else if ((Zipcode >= 37000) && (Zipcode <= 38599))
            {
                StateCode = "TN";
            }
            else if ((Zipcode >= 38600) && (Zipcode <= 39799))
            {
                StateCode = "MS";
            }
            else if ((Zipcode >= 40000) && (Zipcode <= 42799))
            {
                StateCode = "KY";
            }
            else if ((Zipcode >= 43000) && (Zipcode <= 45899))
            {
                StateCode = "OH";
            }
            else if ((Zipcode >= 46000) && (Zipcode <= 47999))
            {
                StateCode = "IN";
            }
            else if ((Zipcode >= 48000) && (Zipcode <= 49999))
            {
                StateCode = "MI";
            }
            else if ((Zipcode >= 50000) && (Zipcode <= 52899))
            {
                StateCode = "IA";
            }
            else if ((Zipcode >= 53000) && (Zipcode <= 54999))
            {
                StateCode = "WI";
            }
            else if ((Zipcode >= 55000) && (Zipcode <= 56799))
            {
                StateCode = "MN";
            }
            else if ((Zipcode >= 57000) && (Zipcode <= 57799))
            {
                StateCode = "SD";
            }
            else if ((Zipcode >= 58000) && (Zipcode <= 58899))
            {
                StateCode = "ND";
            }
            else if ((Zipcode >= 59000) && (Zipcode <= 59999))
            {
                StateCode = "MT";
            }
            else if ((Zipcode >= 60000) && (Zipcode <= 62999))
            {
                StateCode = "IL";
            }
            else if ((Zipcode >= 63000) && (Zipcode <= 65899))
            {
                StateCode = "MO";
            }
            else if ((Zipcode >= 66000) && (Zipcode <= 67999))
            {
                StateCode = "KS";
            }
            else if ((Zipcode >= 68000) && (Zipcode <= 69399))
            {
                StateCode = "NE";
            }
            else if ((Zipcode >= 70000) && (Zipcode <= 71499))
            {
                StateCode = "LA";
            }
            else if (((Zipcode >= 71600) && (Zipcode <= 72999)) || ((Zipcode >= 75500) && (Zipcode <= 75599)))
            {
                StateCode = "AR";
            }
            else if ((Zipcode >= 73000) && (Zipcode <= 74999))
            {
                StateCode = "OK";
            }
            else if (((Zipcode >= 75000) && (Zipcode <= 79999)) || ((Zipcode >= 88500) && (Zipcode <= 88599)))
            {
                StateCode = "TX";
            }
            else if ((Zipcode >= 80000) && (Zipcode <= 81699))
            {
                StateCode = "CO";
            }
            else if ((Zipcode >= 82000) && (Zipcode <= 83199))
            {
                StateCode = "WY";
            }
            else if ((Zipcode >= 83200) && (Zipcode <= 83899))
            {
                StateCode = "ID";
            }
            else if ((Zipcode >= 84000) && (Zipcode <= 84799))
            {
                StateCode = "UT";
            }
            else if ((Zipcode >= 85000) && (Zipcode <= 86599))
            {
                StateCode = "AZ";
            }
            else if ((Zipcode >= 87000) && (Zipcode <= 88499))
            {
                StateCode = "NM";
            }
            else if ((Zipcode >= 88900) && (Zipcode <= 89899))
            {
                StateCode = "NV";
            }
            else if ((Zipcode >= 90000) && (Zipcode <= 96699))
            {
                StateCode = "CA";
                if ((Zipcode >= 96200) && (Zipcode <= 96699))
                {
                    StateCode = "AP";
                }
            }
            else if ((Zipcode >= 96700) && (Zipcode <= 96899))
            {
                StateCode = "HI";
            }
            else if ((Zipcode >= 96900) && (Zipcode <= 96999))
            {
                StateCode = "MP";
            }
            else if ((Zipcode >= 97000) && (Zipcode <= 97999))
            {
                StateCode = "OR";
            }
            else if ((Zipcode >= 98000) && (Zipcode <= 99499))
            {
                StateCode = "WA";
            }
            else if ((Zipcode >= 99500) && (Zipcode <= 99999))
            {
                StateCode = "AK";
            }
            else
            {
                return "Non U.S. Postal Code";
            }

            switch (options)
            {
                case GetStateOptions.Abbreviation:
                    return StateCode;
                case GetStateOptions.FullName:
                    return StateCodeToFullName(StateCode);
                default:
                    return "";
            }

        }

        /// <summary>
        /// Returns the full US State name for a passed US State code
        /// </summary>
        public static string StateCodeToFullName(string StateCode)
        {
            switch (StateCode)
            {
                case "AA":
                    return "APO Atlantic";
                case "AK":
                    return "Alaska";
                case "AL":
                    return "Alabama";
                case "AP":
                    return "APO Pacific";
                case "AR":
                    return "Arkansas";
                case "AZ":
                    return "Arizona";
                case "CA":
                    return "California";
                case "CO":
                    return "Colorado";
                case "CT":
                    return "Connecticut";
                case "DC":
                    return "District of Columbia";
                case "DE":
                    return "Delaware";
                case "FL":
                    return "Florida";
                case "GA":
                    return "Georgia";
                case "HI":
                    return "Hawaii";
                case "IA":
                    return "Iowa";
                case "ID":
                    return "Idaho";
                case "IL":
                    return "Illinois";
                case "IN":
                    return "Indiana";
                case "KS":
                    return "Kansas";
                case "KY":
                    return "Kentucky";
                case "LA":
                    return "Louisiana";
                case "MA":
                    return "Massachusetts";
                case "MD":
                    return "Maryland";
                case "ME":
                    return "Maine";
                case "MI":
                    return "Michigan";
                case "MN":
                    return "Minnesota";
                case "MO":
                    return "Missouri";
                case "MP":
                    return "Northern Mariana Islands";
                case "MS":
                    return "Mississippi";
                case "MT":
                    return "Montana";
                case "NC":
                    return "North Carolina";
                case "ND":
                    return "North Dakota";
                case "NE":
                    return "Nebraska";
                case "NH":
                    return "New Hampshire";
                case "NJ":
                    return "New Jersey";
                case "NM":
                    return "New Mexico";
                case "NV":
                    return "Nevada";
                case "NY":
                    return "New York";
                case "OH":
                    return "Ohio";
                case "OK":
                    return "Oklahoma";
                case "OR":
                    return "Oregon";
                case "PA":
                    return "Pennsylvania";
                case "RI":
                    return "Rhode Island";
                case "SC":
                    return "South Carolina";
                case "SD":
                    return "South Dakota";
                case "TN":
                    return "Tennessee";
                case "TX":
                    return "Texas";
                case "UT":
                    return "Utah";
                case "VA":
                    return "Virginia";
                case "VT":
                    return "Vermont";
                case "WA":
                    return "Washington";
                case "WI":
                    return "Wisconsin";
                case "WV":
                    return "West Virginia";
                case "WY":
                    return "Wyoming";
                default:
                    return "Unrecognized U.S. State Abbreviation";
            }
        }


        public static bool IsUSState(string StateCode)
        {
            switch (StateCode)
            {
                case "AK":
                case "AL":
                case "AR":
                case "AZ":
                case "CA":
                case "CO":
                case "CT":
                case "DC":
                case "DE":
                case "FL":
                case "GA":
                case "HI":
                case "IA":
                case "ID":
                case "IL":
                case "IN":
                case "KS":
                case "KY":
                case "LA":
                case "MA":
                case "MD":
                case "ME":
                case "MI":
                case "MN":
                case "MO":
                case "MS":
                case "MT":
                case "NC":
                case "ND":
                case "NE":
                case "NH":
                case "NJ":
                case "NM":
                case "NV":
                case "NY":
                case "OH":
                case "OK":
                case "OR":
                case "PA":
                case "RI":
                case "SC":
                case "SD":
                case "TN":
                case "TX":
                case "UT":
                case "VA":
                case "VT":
                case "WA":
                case "WI":
                case "WV":
                case "WY":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns the US State code for a passed US State name
        /// </summary>
        public static string FullStateToStateCode(string FullStateName)
        {
            switch (FullStateName)
            {
                case "APO Atlantic":
                    return "AA";
                case "Alaska":
                    return "AK";
                case "Alabama":
                    return "AL";
                case "APO Pacific":
                    return "AP";
                case "Arkansas":
                    return "AR";
                case "Arizona":
                    return "AZ";
                case "California":
                    return "CA";
                case "Colorado":
                    return "CO";
                case "Connecticut":
                    return "CT";
                case "District of Columbia":
                    return "DC";
                case "Delaware":
                    return "DE";
                case "Florida":
                    return "FL";
                case "Georgia":
                    return "GA";
                case "Hawaii":
                    return "HI";
                case "Iowa":
                    return "IA";
                case "Idaho":
                    return "ID";
                case "Illinois":
                    return "IL";
                case "Indiana":
                    return "IN";
                case "Kansas":
                    return "KS";
                case "Kentucky":
                    return "KY";
                case "Louisiana":
                    return "LA";
                case "Massachusetts":
                    return "MA";
                case "Maryland":
                    return "MD";
                case "Maine":
                    return "ME";
                case "Michigan":
                    return "MI";
                case "Minnesota":
                    return "MN";
                case "Missouri":
                    return "MO";
                case "Mississippi":
                    return "MS";
                case "Montana":
                    return "MT";
                case "North Carolina":
                    return "NC";
                case "North Dakota":
                    return "ND";
                case "Nebraska":
                    return "NE";
                case "New Hampshire":
                    return "NH";
                case "New Jersey":
                    return "NJ";
                case "New Mexico":
                    return "NM";
                case "Nevada":
                    return "NV";
                case "New York":
                    return "NY";
                case "Ohio":
                    return "OH";
                case "Oklahoma":
                    return "OK";
                case "Oregon":
                    return "OR";
                case "Pennsylvania":
                    return "PA";
                case "Rhode Island":
                    return "RI";
                case "South Carolina":
                    return "SC";
                case "South Dakota":
                    return "SD";
                case "Tennessee":
                    return "TN";
                case "Texas":
                    return "TX";
                case "Utah":
                    return "UT";
                case "Virginia":
                    return "VA";
                case "Vermont":
                    return "VT";
                case "Washington":
                    return "WA";
                case "Wisconsin":
                    return "WI";
                case "West Virginia":
                    return "WV";
                case "Wyoming":
                    return "WY";
                default:
                    return "Unrecognized U.S. State Name";
            }
        }

    }
}

