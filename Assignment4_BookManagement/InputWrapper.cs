using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4_BookManagement
{
    public class InputWrapper<T>
    {
        public T input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }
        public Dictionary<string, string> fieldsMsg
        {
            get 
            { 
                return fieldsMsg; 
            }
            set 
            { 
                fieldsMsg = value; 
            }
        }
    }
}
