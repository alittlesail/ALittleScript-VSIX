
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueThisTypeReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueThisTypeElement>
    {
        private ALittleScriptClassDecElement m_class_dec = null;
        private ALittleScriptClassCtorDecElement m_class_ctor_dec = null;
        private ALittleScriptClassGetterDecElement m_class_getter_dec = null;
        private ALittleScriptClassSetterDecElement m_class_setter_dec = null;
        private ALittleScriptClassMethodDecElement m_class_method_dec = null;
        private ALittleScriptClassStaticDecElement m_class_static_dec = null;
        private ALittleScriptGlobalMethodDecElement m_global_method_dec = null;
        private bool m_is_const = false;

        public ALittleScriptPropertyValueThisTypeReference(ABnfElement element) : base(element)
        {
            ReloadInfo();
        }

        private void ReloadInfo()
        {
            m_class_dec = null;
            m_class_ctor_dec = null;
            m_class_setter_dec = null;
            m_class_method_dec = null;
            m_class_static_dec = null;

            ABnfElement parent = m_element;
            while (true)
            {
                if (parent == null) break;

                if (parent is ALittleScriptNamespaceDecElement) {
                    break;
                } else if (parent is ALittleScriptClassDecElement) {
                    m_class_dec = (ALittleScriptClassDecElement)parent;
                    break;
                } else if (parent is ALittleScriptClassCtorDecElement) {
                    m_class_ctor_dec = (ALittleScriptClassCtorDecElement)parent;
                } else if (parent is ALittleScriptClassGetterDecElement) {
                    m_class_getter_dec = (ALittleScriptClassGetterDecElement)parent;
                    var modifier = (m_class_getter_dec.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    m_is_const = ALittleScriptUtility.IsConst(modifier);
                } else if (parent is ALittleScriptClassSetterDecElement) {
                    m_class_setter_dec = (ALittleScriptClassSetterDecElement)parent;
                    var modifier = (m_class_setter_dec.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    m_is_const = ALittleScriptUtility.IsConst(modifier);
                } else if (parent is ALittleScriptClassMethodDecElement) {
                    m_class_method_dec = (ALittleScriptClassMethodDecElement)parent;
                    var modifier = (m_class_method_dec.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    m_is_const = ALittleScriptUtility.IsConst(modifier);
                } else if (parent is ALittleScriptClassStaticDecElement) {
                    m_class_static_dec = (ALittleScriptClassStaticDecElement)parent;
                } else if (parent is ALittleScriptGlobalMethodDecElement) {
                    m_global_method_dec = (ALittleScriptGlobalMethodDecElement)parent;
                }

                parent = parent.GetParent();
            }
        }

        private List<ABnfElement> CalcResolve()
        {
            var dec_list = new List<ABnfElement>();
            if (m_class_dec != null && m_global_method_dec == null && m_class_static_dec == null)
                dec_list.Add(m_class_dec);
            return dec_list;
        }

        public override ABnfElement GotoDefinition()
        {
            var result_list = CalcResolve();
            foreach (var result in result_list) return result;
            return null;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            var result_list = CalcResolve();
            foreach (var result in result_list)
            {
                if (result is ALittleScriptClassDecElement)
                {
                    var error = result.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    if (m_is_const && !guess.is_const)
                    {
                        if (guess is ALittleScriptGuessPrimitive)
                        {
                            ALittleScriptIndex.inst.sPrimitiveGuessMap.TryGetValue("const " + guess.GetValue(), out guess);
                            if (guess == null) return new ABnfGuessError(m_element, "找不到const " + guess.GetValue());
                        }
                        else
                        {
                            guess = guess.Clone();
                            guess.is_const = true;
                            guess.UpdateValue();
                        }
                    }
                    guess_list.Add(guess);
                }
            }

            return null;
        }


        public override ABnfGuessError CheckError()
        {
            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }
    }
}

