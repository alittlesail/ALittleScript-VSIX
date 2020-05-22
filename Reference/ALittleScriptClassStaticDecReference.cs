
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassStaticDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassStaticDecElement>
    {
        public ALittleScriptClassStaticDecReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetMethodNameDec() == null)
                return new ABnfGuessError(m_element, "没有函数名");

            if (m_element.GetMethodBodyDec() == null)
                return new ABnfGuessError(m_element, "没有函数体");

            var parent = m_element.GetParent() as ALittleScriptClassElementDecElement;
            if (parent == null)
                return new ABnfGuessError(m_element, "ALittleScriptClassStaticDecElement的父节点不是ALittleScriptClassElementDecElement");

            var co_text = ALittleScriptUtility.GetCoroutineType(parent.GetModifierList());

            int return_count = 0;
            var return_dec = m_element.GetMethodReturnDec();
            if (return_dec != null) return_count = return_dec.GetMethodReturnOneDecList().Count;

            if (co_text == "async" && return_count > 0)
                return new ABnfGuessError(return_dec, "带async修饰的函数，不能有返回值");
            return null;
        }
    }
}

