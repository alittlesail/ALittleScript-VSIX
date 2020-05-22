using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessMap : ALittleScriptGuess
    {
        public ABnfGuess key_type;
        public ABnfGuess value_type;

        public ALittleScriptGuessMap(ABnfGuess p_key_type, ABnfGuess p_value_type, bool p_is_const)
        {
            key_type = p_key_type;
            value_type = p_value_type;
            is_const = p_is_const;
        }

        public override bool HasAny()
        {
            return key_type.HasAny() || value_type.HasAny();
        }

        public override bool NeedReplace()
        {
            return key_type.NeedReplace() || value_type.NeedReplace();
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            var key_replace = key_type.ReplaceTemplate(fill_map);
            if (key_replace == null) return null;

            var value_replace = value_type.ReplaceTemplate(fill_map);
            if (value_replace == null) return null;

            var guess = new ALittleScriptGuessMap(key_replace, value_replace, is_const);
            guess.UpdateValue();
            return guess;
        }

        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessMap(key_type, value_type, is_const);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            value += "Map<" + key_type.GetValue() + "," + value_type.GetValue() + ">";
        }

        public override bool IsChanged()
        {
            return key_type.IsChanged() || value_type.IsChanged();
        }
    }
}
