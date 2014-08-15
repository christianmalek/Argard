using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argard
{
    public class Argument
    {
        private string _value;

        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                    return "";
                else
                    return _value;
            }
            set
            {
                _value = value;
            }
        }

        public bool IsOption { get; set; }

        public Argument(string value, bool isOption)
        {
            Value = value;
            IsOption = isOption;
        }
    }
}