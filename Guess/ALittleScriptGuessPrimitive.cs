using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessPrimitive : ALittleScriptGuess
    {
        private string native_value = "";
        public ALittleScriptGuessPrimitive(string p_value, bool p_is_const)
        {
            is_const = p_is_const;
            native_value = p_value;
            UpdateValue();
        }

        public override bool HasAny()
        {
            return value == "any" || value == "const any";
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
            return new ALittleScriptGuessPrimitive(native_value, is_const);
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            value += native_value;
        }

        public override bool IsChanged()
        {
            return false;
        }
    }
    public class ALittleScriptGuessBool : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessBool(bool p_is_const) : base("bool", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessBool(is_const); }
    }

    public class ALittleScriptGuessInt : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessInt(bool p_is_const) : base("int", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessInt(is_const); }
    }

    public class ALittleScriptGuessLong : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessLong(bool p_is_const) : base("long", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessLong(is_const); }
    }

    public class ALittleScriptGuessDouble : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessDouble(bool p_is_const) : base("double", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessDouble(is_const); }
    }

    public class ALittleScriptGuessString : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessString(bool p_is_const) : base("string", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessString(is_const); }
    }

    public class ALittleScriptGuessAny : ALittleScriptGuessPrimitive
    {
        public ALittleScriptGuessAny(bool p_is_const) : base("any", p_is_const) { }
        public override ABnfGuess Clone() { return new ALittleScriptGuessAny(is_const); }
    }
}
