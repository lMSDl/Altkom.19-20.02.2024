using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Context
    {

        private Context() { }

        //konstruktor statyczny powoduje wyłączenie flagi beforefieldinit
        static Context() { }
        public static Context Instance { get; } = new Context();
    }
}
