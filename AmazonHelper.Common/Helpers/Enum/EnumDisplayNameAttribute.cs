using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonHelper.Common
{
    public class EnumDisplayNameAttribute : Attribute
    {
        public string Name { get; set; }

        public EnumDisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
