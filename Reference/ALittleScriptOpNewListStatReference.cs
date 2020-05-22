
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptOpNewListStatReference : ALittleScriptReferenceTemplate<ALittleScriptOpNewListStatElement>
    {
        public ALittleScriptOpNewListStatReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "List列表不能为空");

            var error = value_stat_list[0].GuessType(out ABnfGuess guess);
            if (error != null) return error;
            var info = new ALittleScriptGuessList(guess, false, false);
            info.UpdateValue();
            guess_list = new List<ABnfGuess>() { info };
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "这种方式必须填写参数，否则请使用new List的方式");

            // 列表里面的所有元素的类型必须和第一个元素一致
            var error = value_stat_list[0].GuessType(out ABnfGuess value_stat_first);
            if (error != null) return error;
            for (int i = 1; i < value_stat_list.Count; ++i)
            {
                var value_stat = value_stat_list[i];
                error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                error = value_stat.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (value_stat_first.GetValue() != guess.GetValue())
                    return new ABnfGuessError(value_stat_list[i], "列表内的元素类型，必须和第一个元素类型一致");
            }
            return null;
        }
    }
}

