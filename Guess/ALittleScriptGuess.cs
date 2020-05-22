using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuess : ABnfGuess
    {
        public bool is_register = false;

        public ALittleScriptGuess()
        {
        }

        public virtual ABnfElement GetElement()
        {
            return null;
        }
    }
}
