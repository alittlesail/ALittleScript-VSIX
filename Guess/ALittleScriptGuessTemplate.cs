using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessTemplate : ALittleScriptGuess
    {
        // 模板范围限定
        public ABnfGuess template_extends;
        public bool is_class;
        public bool is_struct;

        // 元素对象
        public ALittleScriptTemplatePairDecElement template_pair_dec;
        private string native_value = "";

        public ALittleScriptGuessTemplate(ALittleScriptTemplatePairDecElement p_template_pair_dec
            , ABnfGuess p_template_extends
            , bool p_is_class, bool p_is_struct)
        {
            is_register = ALittleScriptUtility.IsRegister(p_template_pair_dec);
            template_pair_dec = p_template_pair_dec;
            template_extends = p_template_extends;
            is_class = p_is_class;
            is_struct = p_is_struct;
            if (p_template_extends != null) is_const = p_template_extends.is_const;

            var name_dec = template_pair_dec.GetTemplateNameDec();
            if (name_dec != null) native_value = name_dec.GetElementText();
        }

        public override ABnfElement GetElement()
        {
            return template_pair_dec;
        }

        public override bool NeedReplace()
        {
            return true;
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            if (fill_map.TryGetValue(native_value, out ABnfGuess new_guess))
                return new_guess;
            return this;
        }

        public override string GetTotalValue()
        {
            var v = "";
            if (is_const) v += "const ";
            v += native_value;
            if (template_extends != null)
                return v + ":" + template_extends.GetValue();
            else if (is_class)
                return v + ":class";
            else if (is_struct)
                return v + ":struct";
            return v;
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            value += native_value;
        }

        public override bool IsChanged()
        {
            if (template_extends != null && template_extends.IsChanged()) return true;
            return ALittleScriptIndex.inst.GetGuessTypeList(template_pair_dec) == null;
        }
    }
}
