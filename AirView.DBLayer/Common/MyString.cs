using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWI.Libraries.Common
{
    public  class  MyString
    {
        public  string Between( string token, string first, string second)
        {
            if (!token.Contains(first)) return "";

            var afterFirst = token.Split(new[] { first }, StringSplitOptions.None)[1];

            if (!afterFirst.Contains(second)) return "";

            var result = afterFirst.Split(new[] { second }, StringSplitOptions.None)[0];

            return result;
        }

       

        public  string BetweenTag( string Str, string Tag,string JoinChar)
        {
            string Record = string.Empty;
            try
            {

            Again:
                var startTag = "<" + Tag + ">";
                int startIndex = Str.IndexOf(startTag) + startTag.Length;
                int endIndex = Str.IndexOf("</" + Tag + ">", startIndex);
                string rec = Str.Substring(startIndex, endIndex - startIndex);

                if (rec.Trim().Length > 2)
                {
                    Record = Record + rec.Trim()+ JoinChar;
                    Str = Str.Substring(endIndex).Trim();
                    goto Again;

                }
            }
            catch { }

            return Record;
        }

        public string ReplaceSpecialCharacter(string text) {
            try
            {
                return Regex.Replace(text, @"[^0-9a-zA-Z]+", "");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
