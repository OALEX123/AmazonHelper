using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonHelper.Common;
using AmazonHelper.DataAccess;

namespace TestConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new AmazonHelperDb())
            {
                var uses = ctx.Users.ToList();
                Console.WriteLine(uses.Count);
            }
        }
    }
}
