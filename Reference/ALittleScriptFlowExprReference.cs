
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptFlowExprReference : ALittleScriptReferenceTemplate<ALittleScriptFlowExprElement>
    {
        public ALittleScriptFlowExprReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError CheckError()
        {
            // 获取对应的函数对象
            ABnfElement parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptClassGetterDecElement
                    || parent is ALittleScriptClassSetterDecElement
                    || parent is ALittleScriptClassMethodDecElement
                    || parent is ALittleScriptClassCtorDecElement
                    || parent is ALittleScriptClassStaticDecElement
                    || parent is ALittleScriptGlobalMethodDecElement)
                {
                    break;
                }
                
                if (parent is ALittleScriptForExprElement
                    || parent is ALittleScriptWhileExprElement
                    || parent is ALittleScriptDoWhileExprElement)
                {
                    return null;
                }
                parent = parent.GetParent();
            }

            return new ABnfGuessError(m_element, "break和continue只能在for,while,do while中");
        }
    }
}

