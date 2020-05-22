
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodParamTailDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodParamTailDecElement>
    {
        public ALittleScriptMethodParamTailDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var info = new ALittleScriptGuessParamTail(m_element.GetElementText());
            info.UpdateValue();
            guess_list =  new List<ABnfGuess>() { info };
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var parent = m_element.GetParent();
            if (parent is ALittleScriptMethodParamDecElement) return null;

            while (parent != null)
            {
                ALittleScriptMethodParamDecElement param_dec = null;
                if (parent is ALittleScriptClassMethodDecElement) {
                    param_dec = ((ALittleScriptClassMethodDecElement)parent).GetMethodParamDec();
                } else if (parent is ALittleScriptClassStaticDecElement) {
                    param_dec = ((ALittleScriptClassStaticDecElement)parent).GetMethodParamDec();
                } else if (parent is ALittleScriptClassCtorDecElement) {
                    param_dec = ((ALittleScriptClassCtorDecElement)parent).GetMethodParamDec();
                } else if (parent is ALittleScriptGlobalMethodDecElement) {
                    param_dec = ((ALittleScriptGlobalMethodDecElement)parent).GetMethodParamDec();
                }

                if (param_dec != null)
                {
                    var param_one_list = param_dec.GetMethodParamOneDecList();
                    if (param_one_list.Count == 0)
                        return new ABnfGuessError(m_element, "参数占位符未定义");
                    var param_tail = param_one_list[param_one_list.Count - 1].GetMethodParamTailDec();
                    if (param_tail == null)
                        return new ABnfGuessError(m_element, "参数占位符未定义");
                    break;
                }

                parent = parent.GetParent();
            }
            return null;
        }
    }
}

