
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptStructDecReference : ALittleScriptReferenceTemplate<ALittleScriptStructDecElement>
    {
        private string m_namespace_name;

        public ALittleScriptStructDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var struct_name_dec = m_element.GetStructNameDec();
            if (struct_name_dec == null)
                return new ABnfGuessError(m_element, "没有定义结构体名");

            var info = new ALittleScriptGuessStruct(m_namespace_name, struct_name_dec.GetElementText(), m_element, false);
            info.UpdateValue();
            guess_list = new List<ABnfGuess>() { info };
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var struct_name_dec = m_element.GetStructNameDec();
            if (struct_name_dec == null)
                return new ABnfGuessError(m_element, "没有定义结构体名");

            var body_dec = m_element.GetStructBodyDec();
            if (body_dec == null)
                return new ABnfGuessError(m_element, "没有定义结构体内容");

            var var_dec_list = body_dec.GetStructVarDecList();
            var name_set = new HashSet<string>();
            foreach (var var_dec in var_dec_list)
            {
                var var_name_dec = var_dec.GetStructVarNameDec();
                if (var_name_dec == null) return new ABnfGuessError(var_dec, "没有定义成员变量名");

                var text = var_name_dec.GetElementText();
                if (name_set.Contains(text))
                    return new ABnfGuessError(var_name_dec, "结构体字段名重复");
                name_set.Add(text);
            }

            var option_dec_list = body_dec.GetStructOptionDecList();
            var option_set = new HashSet<string>();
            foreach (var option_dec in option_dec_list)
            {
                var option_name_dec = option_dec.GetStructOptionNameDec();
                if (option_name_dec == null) return new ABnfGuessError(option_dec, "没有定义附加信息名");

                var text = option_name_dec.GetElementText();
                if (option_set.Contains(text))
                    return new ABnfGuessError(option_name_dec, "附加信息名重复");
                option_set.Add(text);

                var option_value = option_dec.GetText();
                if (option_value == null) return new ABnfGuessError(option_dec, text + "没有设置对应的值");

                if (text == "primary")
                {
                    text = option_value.GetElementString().Trim();
                    if (!name_set.Contains(text))
                        return new ABnfGuessError(option_value, "没有找到对应的字段名:" + text);
                    continue;
                }

                if (text == "unique" || text == "index")
                {
                    string[] list = option_value.GetElementString().Split(',');
                    foreach (var name in list)
                    {
                        text = name.Trim();
                        if (!name_set.Contains(text))
                            return new ABnfGuessError(option_value, "没有找到对应的字段名:" + text);
                    }
                    continue;
                }
            }

            return null;
        }
    }
}

