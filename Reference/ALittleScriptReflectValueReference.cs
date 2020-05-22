
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptReflectValueReference : ALittleScriptReferenceTemplate<ALittleScriptReflectValueElement>
    {
        public ALittleScriptReflectValueReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            ABnfGuess guess = null;
            if (m_element.GetReflectCustomType() != null)
            {
                var custom_type = m_element.GetReflectCustomType().GetCustomType();
                if (custom_type == null) return null;

                var error = custom_type.GuessType(out guess);
                if (error != null) return error;
            }
            else if (m_element.GetReflectValueStat() != null)
            {
                var value_stat = m_element.GetReflectValueStat().GetValueStat();
                if (value_stat == null) return null;

                var error = value_stat.GuessType(out guess);
                if (error != null) return error;
            }

            if (guess is ALittleScriptGuessStruct)
                return ALittleScriptIndex.inst.FindALittleStructGuessList("ALittle", "StructInfo", out guess_list);
            else if (guess is ALittleScriptGuessClass)
                return ALittleScriptIndex.inst.FindALittleStructGuessList("ALittle", "ClassInfo", out guess_list);
            else if (guess is ALittleScriptGuessTemplate)
            {
                var guess_template = guess as ALittleScriptGuessTemplate;
                if (guess_template.template_extends is ALittleScriptGuessClass || guess_template.is_class)
                    return ALittleScriptIndex.inst.FindALittleStructGuessList("ALittle", "ClassInfo", out guess_list);
                else if (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct)
                    return ALittleScriptIndex.inst.FindALittleStructGuessList("ALittle", "StructInfo", out guess_list);
            }
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            ABnfGuess guess = null;
            if (m_element.GetReflectCustomType() != null)
            {
                var custom_type = m_element.GetReflectCustomType().GetCustomType();
                if (custom_type == null) return null;

                var error = custom_type.GuessType(out guess);
                if (error != null) return error;
            }
            else if (m_element.GetReflectValueStat() != null)
            {
                var value_stat = m_element.GetReflectValueStat().GetValueStat();
                if (value_stat == null) return null;

                var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                error = value_stat.GuessType(out guess);
                if (error != null) return error;
            }

            if (guess is ALittleScriptGuessStruct || guess is ALittleScriptGuessClass)
                return null;

            if (guess is ALittleScriptGuessTemplate)
            {
                var guess_template = guess as ALittleScriptGuessTemplate;
                if (guess_template.template_extends is ALittleScriptGuessClass || guess_template.is_class)
                    return null;
                else if (m_element.GetReflectCustomType() != null
                    && (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct))
                    return null;
            }

            return new ABnfGuessError(m_element, "反射对象必须是struct或者是class以及class对象");
        }
    }
}

