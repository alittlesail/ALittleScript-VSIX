
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptCoroutineStatReference : ALittleScriptReferenceTemplate<ALittleScriptCoroutineStatElement>
    {
        public ALittleScriptCoroutineStatReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            // 检查这次所在的函数必须要有await或者async修饰
            ABnfElement parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptNamespaceDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassCtorDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassGetterDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassSetterDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassMethodDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    if (ALittleScriptUtility.GetCoroutineType(modifier) == "await")
                        return null;
                    break;
                }
                else if (parent is ALittleScriptClassStaticDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    if (ALittleScriptUtility.GetCoroutineType(modifier) == "await")
                        return null;
                    break;
                }
                else if (parent is ALittleScriptGlobalMethodDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptNamespaceElementDecElement).GetModifierList();
                    if (ALittleScriptUtility.GetCoroutineType(modifier) == "await")
                        return null;
                    break;
                }
                parent = parent.GetParent();
            }

            return new ABnfGuessError(m_element, "co关键字只能在await修饰的函数中使用");
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            return ALittleScriptIndex.inst.FindALittleStructGuessList("ALittle", "Thread", out guess_list);
        }
    }
}

