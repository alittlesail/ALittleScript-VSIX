
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptVarAssignNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptVarAssignNameDecElement>
    {
        private string m_key;
        private ABnfElement m_method_dec;
        private ALittleScriptMethodBodyDecElement m_method_body_dec;

        public ALittleScriptVarAssignNameDecReference(ABnfElement element) : base(element)
        {
            m_key = m_element.GetElementText();
        }

        private void ReloadInfo()
        {
            m_method_dec = null;
            ABnfElement parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptNamespaceDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassCtorDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassCtorDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassSetterDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassSetterDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassGetterDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassGetterDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassMethodDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassMethodDecElement).GetMethodBodyDec();
                    break;

                }
                else if (parent is ALittleScriptClassStaticDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassStaticDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptGlobalMethodDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptGlobalMethodDecElement).GetMethodBodyDec();
                    break;
                }

                parent = parent.GetParent();
            }
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return null;
            if (guess is ALittleScriptGuessFunctor)
            {
                var guess_functor = guess as ALittleScriptGuessFunctor;
                if (guess_functor.element is ALittleScriptClassStaticDecElement
                    || guess_functor.element is ALittleScriptClassMethodDecElement
                    || guess_functor.element is ALittleScriptClassGetterDecElement
                    || guess_functor.element is ALittleScriptClassSetterDecElement
                    || guess_functor.element is ALittleScriptGlobalMethodDecElement)
                    return "ALittleScriptMethodName";
            }
            else if (guess is ALittleScriptGuessNamespaceName
                || guess is ALittleScriptGuessClassName
                || guess is ALittleScriptGuessStructName
                || guess is ALittleScriptGuessEnumName)
            {
                return "ALittleScriptCustomName";
            }

            return "ALittleScriptVarName";
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            ABnfElement parent = m_element.GetParent();
            if (parent is ALittleScriptVarAssignDecElement) {
                return parent.GuessTypes(out guess_list);
            } else if (parent is ALittleScriptForPairDecElement) {
                return parent.GuessTypes(out guess_list);
            }
            guess_list = new List<ABnfGuess>();
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetElementText().StartsWith("___"))
                return new ABnfGuessError(m_element, "变量名不能以3个下划线开头");

            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }

        public override bool PeekHighlightWord()
        {
            return true;
        }

        public override void QueryHighlightWordTag(List<ALanguageHighlightWordInfo> list)
        {
            if (m_method_dec == null)
                ReloadInfo();

            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return;
            CollectHighlight(guess, m_method_body_dec, list);

            // 处理参数
            if (m_method_dec != null)
            {
                var dec_list = ALittleScriptUtility.FindMethodParamNameDecList(m_method_dec, m_key);
                foreach (var dec in dec_list)
                {
                    var info = new ALanguageHighlightWordInfo();
                    info.start = dec.GetStart();
                    info.end = dec.GetEnd();
                    list.Add(info);
                }
            }
        }

        private void CollectHighlight(ABnfGuess target_guess, ABnfElement element, List<ALanguageHighlightWordInfo> list)
        {
            if (element is ALittleScriptPropertyValueCustomTypeElement
                || element is ALittleScriptVarAssignNameDecElement)
            {
                if (element.GetElementText() != m_key) return;

                var error = element.GuessType(out ABnfGuess guess);
                if (error != null) return;
                if (guess.GetValue() == target_guess.GetValue())
                {
                    var info = new ALanguageHighlightWordInfo();
                    info.start = element.GetStart();
                    info.end = element.GetEnd();
                    list.Add(info);
                }
                return;
            }

            if (element is ABnfNodeElement)
            {
                var childs = (element as ABnfNodeElement).GetChilds();
                foreach (var child in childs)
                    CollectHighlight(target_guess, child, list);
            }
        }
    }
}

