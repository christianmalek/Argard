using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    static class StringHelper
    {
        public static string RemoveLastAndFirstChar(string text)
        {
            string result;
            if (text.Length >= 3)
            {
                result = text.Substring(1, text.Length - 2);
            }
            else
            {
                result = text;
            }
            return result;
        }

        public static bool CheckFirstAndLastCharOfString(char firstChar, char lastChar, string text)
        {
            return text.Length > 1 && text[0] == firstChar && text[text.Length - 1] == lastChar;
        }
    }
}
