using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessParamTail : ALittleScriptGuess
    {
        public ALittleScriptGuessParamTail(string p_value)
        {
            value = p_value;
        }

        public override bool NeedReplace()
        {
            return false;
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            return this;
        }

        public override ABnfGuess Clone()
        {
            return new ALittleScriptGuessParamTail(value);
        }

        public override void UpdateValue()
        {
        }

        public override bool IsChanged()
        {
            return false;
        }
    }
}
