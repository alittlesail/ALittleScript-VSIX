
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueBracketValueReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueBracketValueElement>
    {
        public ALittleScriptPropertyValueBracketValueReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            // 获取父节点
            var property_value_suffix = m_element.GetParent() as ALittleScriptPropertyValueSuffixElement;
            var property_value = property_value_suffix.GetParent() as ALittleScriptPropertyValueElement;
            var property_value_first_type = property_value.GetPropertyValueFirstType();
            var suffix_list = property_value.GetPropertyValueSuffixList();

            // 获取所在位置
            int index = suffix_list.IndexOf(property_value_suffix);
            if (index == -1) return null;

            // 获取前一个类型
            ABnfGuess pre_type;
            ABnfGuessError error = null;
            if (index == 0)
                error = property_value_first_type.GuessType(out pre_type);
            else
                error = suffix_list[index - 1].GuessType(out pre_type);
            if (error != null) return error;

            // 获取类型
            if (pre_type is ALittleScriptGuessList)
            {
                var sub_type = ((ALittleScriptGuessList)pre_type).sub_type;
                if (pre_type.is_const && !sub_type.is_const)
                {
                    sub_type = sub_type.Clone();
                    sub_type.is_const = true;
                    sub_type.UpdateValue();
                }
                guess_list.Add(sub_type);
            }
            else if (pre_type is ALittleScriptGuessMap)
            {
                var value_type = ((ALittleScriptGuessMap)pre_type).value_type;
                if (pre_type.is_const && !value_type.is_const)
                {
                    value_type = value_type.Clone();
                    value_type.is_const = true;
                    value_type.UpdateValue();
                }
                guess_list.Add(value_type);
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat = m_element.GetValueStat();
            if (value_stat == null) return null;

            // 获取父节点
            var property_value_suffix = m_element.GetParent() as ALittleScriptPropertyValueSuffixElement;
            var property_value = property_value_suffix.GetParent() as ALittleScriptPropertyValueElement;
            var property_value_first_type = property_value.GetPropertyValueFirstType();
            var suffixList = property_value.GetPropertyValueSuffixList();

            // 获取所在位置
            int index = suffixList.IndexOf(property_value_suffix);
            if (index == -1) return null;

            // 获取前一个类型
            ABnfGuess pre_type;
            ABnfGuessError error = null;
            if (index == 0)
                error = property_value_first_type.GuessType(out pre_type);
            else
                error = suffixList[index - 1].GuessType(out pre_type);
            if (error != null) return error;

            error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess key_guess_type);
            if (error != null) return error;

            // 获取类型
            if (pre_type is ALittleScriptGuessList)
            {
                if (!(key_guess_type is ALittleScriptGuessInt) && !(key_guess_type is ALittleScriptGuessLong))
                    return new ABnfGuessError(value_stat, "索引值的类型必须是int或者是long，不能是:" + key_guess_type.GetValue());
            }
            else if (pre_type is ALittleScriptGuessMap)
            {
                var pre_type_map = pre_type as ALittleScriptGuessMap;
                error = ALittleScriptOp.GuessTypeEqual(((ALittleScriptGuessMap)pre_type).key_type, value_stat, key_guess_type, true, false);
                if (error != null)
                    return new ABnfGuessError(error.GetElement(), "索引值的类型不能是:" + key_guess_type.GetValue() + " :" + error.GetError());
            }

            {
                error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
                if (error != null) return error;
                if (guess_list.Count == 0)
                    return new ABnfGuessError(m_element, "该元素不能直接使用[]取值，请先cast");
                else if (guess_list.Count != 1)
                    return new ABnfGuessError(m_element, "重复定义");
                return null;
            }
        }
    }
}

