using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Argard
{
    class IdentifierParser
    {
        private const string _identifierPattern = "(?:(?:(\\w+)(\\[\\w+\\])?(\\w*))|(?:(\\[\\w+\\])(\\w+)))";

        public IEnumerable<string> GetIdentifiers(string multipleIdentifierScheme)
        {
            List<string> list = new List<string>();
            string[] array = multipleIdentifierScheme.Split(new char[]
			{
				'|'
			});
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string singleIdentifierScheme = array2[i];
                list.AddRange(this.SplitIdentifiers(singleIdentifierScheme));
            }
            return list;
        }

        private IEnumerable<string> SplitIdentifiers(string singleIdentifierScheme)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex(_identifierPattern);
            Match match = regex.Match(singleIdentifierScheme);
            string[] array = new string[5];
            for (int i = 0; i < 5; i++)
            {
                array[i] = match.Groups[i + 1].ToString();
            }
            if (array[0] != string.Empty)
            {
                if (array[1] != string.Empty)
                {
                    if (array[2] != string.Empty)
                    {
                        list.Add(array[0] + array[2]);
                        list.Add(array[0] + StringHelper.RemoveLastAndFirstChar(array[1]) + array[2]);
                    }
                    else
                    {
                        list.Add(array[0]);
                        list.Add(array[0] + StringHelper.RemoveLastAndFirstChar(array[1]));
                    }
                }
                else
                {
                    list.Add(array[0]);
                }
            }
            else
            {
                list.Add(StringHelper.RemoveLastAndFirstChar(array[3]) + array[4]);
                list.Add(array[4]);
            }
            return list;
        }
    }
}
