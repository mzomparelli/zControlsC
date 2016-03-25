using System.Text.RegularExpressions;
using zControlsC;
using System;


namespace zControlsC
{
    public static class zRegexEx
    {

        public struct REGEXType
        {
            public const string REGEX_EMAIL = "^(?<Username>[\\w-\\.]+)@((?<DomainName>\\[[0-9]{1,3}[0-9]{1,3}[0-9]{1,3})|((?<DomainName>[\\w-]+)+))(?<DomainExtension>\\.[a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
            public const string REGEX_PHONENUMBER = "^[1-1]{1}?[\\s\\-]?\\(?(?<AreaCode>[2-9]{1}\\d{2})\\)?[\\s\\-]?(?<Prefix>\\d{3})\\-?(?<Suffix>\\d{4})$";
            public const string REGEX_ZIPCODE = "^(?<Zip>(?<ThreeZip>\\d{3})\\d{2})[\\s\\-]?(?<POBox>\\d{4})?$|^(?<ThreeZip>[a-zA-Z]{1}[0-9a-zA-Z]{2})[\\s\\-]?(?<LastThree>[0-9a-zA-Z]{3})$";
            public const string REGEX_NUMBER = "[^0-9]";
            //private string _NOTHING;
        }



        /// <summary>
        /// Returns TRUE if provided string is the correct phone number fromat and Returns FALSE if it is not. This function does not check the validity of a phone number.
        /// </summary>
        /// <param name="s">The string to check for phone number format.</param>
        public static bool IsPhoneNumber(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (s == "")
            {
                return false;
            }
            return Regex.IsMatch(s, REGEXType.REGEX_PHONENUMBER);
        }

        /// <summary>
        /// Returns TRUE if provided string is the correct email address fromat and Returns FALSE if it is not. This function does not check the validity of an email address.
        /// </summary>
        /// <param name="s">The string to check for email format.</param>
        public static bool IsEmailAddress(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (s == "")
            {
                return false;
            }
            return Regex.IsMatch(s, REGEXType.REGEX_EMAIL);
        }


        public static bool IsPostalCode(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (s == "")
            {
                return false;
            }
            return Regex.IsMatch(s, REGEXType.REGEX_ZIPCODE);
        }

        public static bool IsNumeric(string s)
        {
            if (s == null)
            {
                return false;
            }
            if (s == "")
            {
                return false;
            }
            return Regex.IsMatch(s, REGEXType.REGEX_NUMBER);
        }

        public static bool IsDate(string s)
        {
            try
            {
                DateTime d = Convert.ToDateTime(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        

        

    }
}

