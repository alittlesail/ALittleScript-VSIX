using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessList : ALittleScriptGuess
    {
        public ABnfGuess sub_type;
        public bool is_native;

        public ALittleScriptGuessList(ABnfGuess p_sub_type, bool p_is_const, bool p_is_native)
        {
            sub_type = p_sub_type;
            is_const = p_is_const;
            is_native = p_is_native;
        }

        public override bool HasAny()
        {
            return sub_type.HasAny();
        }

        public override bool NeedReplace()
        {
            return sub_type.NeedReplace();
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            var replace = sub_type.ReplaceTemplate(fill_map);
            if (replace == null) return null;
            var guess = new ALittleScriptGuessList(replace, is_const, is_native);
            guess.UpdateValue();
            return guess;
        }

        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessList(sub_type, is_const, is_native);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            if (is_native) value += "native ";
            value += "List<" + sub_type.GetValue() + ">";
        }

        public override bool IsChanged()
        {
            return sub_type.IsChanged();
        }
    }
}
