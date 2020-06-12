
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ALittle
{
    // 对象工厂
    public class ALittleScriptFactoryClass : ALittleScriptFactory
    {
        public static ALittleScriptFactoryClass inst = new ALittleScriptFactoryClass();

        Dictionary<System.Type, Func<ABnfElement, ABnfReference>> m_create_map = new Dictionary<System.Type, Func<ABnfElement, ABnfReference>>();

        public ALittleScriptFactoryClass()
        {
            m_create_map[typeof(ALittleScriptAllExprElement)] = (element) => { return new ALittleScriptAllExprReference(element); };
            m_create_map[typeof(ALittleScriptAllTypeElement)] = (element) => { return new ALittleScriptAllTypeReference(element); };
            m_create_map[typeof(ALittleScriptAssertExprElement)] = (element) => { return new ALittleScriptAssertExprReference(element); };
            m_create_map[typeof(ALittleScriptBindStatElement)] = (element) => { return new ALittleScriptBindStatReference(element); };
            m_create_map[typeof(ALittleScriptClassCtorDecElement)] = (element) => { return new ALittleScriptClassCtorDecReference(element); };
            m_create_map[typeof(ALittleScriptClassGetterDecElement)] = (element) => { return new ALittleScriptClassGetterDecReference(element); };
            m_create_map[typeof(ALittleScriptClassSetterDecElement)] = (element) => { return new ALittleScriptClassSetterDecReference(element); };
            m_create_map[typeof(ALittleScriptClassMethodDecElement)] = (element) => { return new ALittleScriptClassMethodDecReference(element); };
            m_create_map[typeof(ALittleScriptClassStaticDecElement)] = (element) => { return new ALittleScriptClassStaticDecReference(element); };
            m_create_map[typeof(ALittleScriptClassDecElement)] = (element) => { return new ALittleScriptClassDecReference(element); };
            m_create_map[typeof(ALittleScriptClassExtendsDecElement)] = (element) => { return new ALittleScriptClassExtendsDecReference(element); };
            m_create_map[typeof(ALittleScriptClassElementDecElement)] = (element) => { return new ALittleScriptClassElementDecReference(element); };
            m_create_map[typeof(ALittleScriptClassNameDecElement)] = (element) => { return new ALittleScriptClassNameDecReference(element); };
            m_create_map[typeof(ALittleScriptClassVarDecElement)] = (element) => { return new ALittleScriptClassVarDecReference(element); };
            m_create_map[typeof(ALittleScriptClassVarNameDecElement)] = (element) => { return new ALittleScriptClassVarNameDecReference(element); };
            m_create_map[typeof(ALittleScriptConstValueElement)] = (element) => { return new ALittleScriptConstValueReference(element); };
            m_create_map[typeof(ALittleScriptCoroutineStatElement)] = (element) => { return new ALittleScriptCoroutineStatReference(element); };
            m_create_map[typeof(ALittleScriptCustomTypeDotIdNameElement)] = (element) => { return new ALittleScriptCustomTypeDotIdNameReference(element); };
            m_create_map[typeof(ALittleScriptCustomTypeDotIdElement)] = (element) => { return new ALittleScriptCustomTypeDotIdReference(element); };
            m_create_map[typeof(ALittleScriptCustomTypeNameElement)] = (element) => { return new ALittleScriptCustomTypeNameReference(element); };
            m_create_map[typeof(ALittleScriptCustomTypeElement)] = (element) => { return new ALittleScriptCustomTypeReference(element); };
            m_create_map[typeof(ALittleScriptEnumDecElement)] = (element) => { return new ALittleScriptEnumDecReference(element); };
            m_create_map[typeof(ALittleScriptEnumNameDecElement)] = (element) => { return new ALittleScriptEnumNameDecReference(element); };
            m_create_map[typeof(ALittleScriptEnumVarDecElement)] = (element) => { return new ALittleScriptEnumVarDecReference(element); };
            m_create_map[typeof(ALittleScriptEnumVarNameDecElement)] = (element) => { return new ALittleScriptEnumVarNameDecReference(element); };
            m_create_map[typeof(ALittleScriptFlowExprElement)] = (element) => { return new ALittleScriptFlowExprReference(element); };
            m_create_map[typeof(ALittleScriptForExprElement)] = (element) => { return new ALittleScriptForExprReference(element); };
            m_create_map[typeof(ALittleScriptForPairDecElement)] = (element) => { return new ALittleScriptForPairDecReference(element); };
            m_create_map[typeof(ALittleScriptGenericTypeElement)] = (element) => { return new ALittleScriptGenericTypeReference(element); };
            m_create_map[typeof(ALittleScriptGlobalMethodDecElement)] = (element) => { return new ALittleScriptGlobalMethodDecReference(element); };
            m_create_map[typeof(ALittleScriptIdElement)] = (element) => { return new ALittleScriptIdReference(element); };
            m_create_map[typeof(ALittleScriptIfConditionElement)] = (element) => { return new ALittleScriptIfConditionReference(element); };
            m_create_map[typeof(ALittleScriptElseIfConditionElement)] = (element) => { return new ALittleScriptElseIfConditionReference(element); };
            m_create_map[typeof(ALittleScriptWhileConditionElement)] = (element) => { return new ALittleScriptWhileConditionReference(element); };
            m_create_map[typeof(ALittleScriptDoWhileConditionElement)] = (element) => { return new ALittleScriptDoWhileConditionReference(element); };
            m_create_map[typeof(ALittleScriptKeyElement)] = (element) => { return new ALittleScriptKeyReference(element); };
            m_create_map[typeof(ALittleScriptLineCommentElement)] = (element) => { return new ALittleScriptLineCommentReference(element); };
            m_create_map[typeof(ALittleScriptMethodBodyDecElement)] = (element) => { return new ALittleScriptMethodBodyDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodNameDecElement)] = (element) => { return new ALittleScriptMethodNameDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodParamOneDecElement)] = (element) => { return new ALittleScriptMethodParamOneDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodParamNameDecElement)] = (element) => { return new ALittleScriptMethodParamNameDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodParamTailDecElement)] = (element) => { return new ALittleScriptMethodParamTailDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodReturnDecElement)] = (element) => { return new ALittleScriptMethodReturnDecReference(element); };
            m_create_map[typeof(ALittleScriptMethodReturnTailDecElement)] = (element) => { return new ALittleScriptMethodReturnTailDecReference(element); };
            m_create_map[typeof(ALittleScriptNamespaceDecElement)] = (element) => { return new ALittleScriptNamespaceDecReference(element); };
            m_create_map[typeof(ALittleScriptNamespaceElementDecElement)] = (element) => { return new ALittleScriptNamespaceElementDecReference(element); };
            m_create_map[typeof(ALittleScriptNamespaceNameDecElement)] = (element) => { return new ALittleScriptNamespaceNameDecReference(element); };
            m_create_map[typeof(ALittleScriptOp1ExprElement)] = (element) => { return new ALittleScriptOp1ExprReference(element); };
            m_create_map[typeof(ALittleScriptOpAssignExprElement)] = (element) => { return new ALittleScriptOpAssignExprReference(element); };
            m_create_map[typeof(ALittleScriptOpNewListStatElement)] = (element) => { return new ALittleScriptOpNewListStatReference(element); };
            m_create_map[typeof(ALittleScriptOpNewStatElement)] = (element) => { return new ALittleScriptOpNewStatReference(element); };
            m_create_map[typeof(ALittleScriptPathsValueElement)] = (element) => { return new ALittleScriptPathsValueReference(element); };
            m_create_map[typeof(ALittleScriptPrimitiveTypeElement)] = (element) => { return new ALittleScriptPrimitiveTypeReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueBracketValueElement)] = (element) => { return new ALittleScriptPropertyValueBracketValueReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueCastTypeElement)] = (element) => { return new ALittleScriptPropertyValueCastTypeReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueCustomTypeElement)] = (element) => { return new ALittleScriptPropertyValueCustomTypeReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueDotIdNameElement)] = (element) => { return new ALittleScriptPropertyValueDotIdNameReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueDotIdElement)] = (element) => { return new ALittleScriptPropertyValueDotIdReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueFirstTypeElement)] = (element) => { return new ALittleScriptPropertyValueFirstTypeReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueMethodCallElement)] = (element) => { return new ALittleScriptPropertyValueMethodCallReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueElement)] = (element) => { return new ALittleScriptPropertyValueReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueSuffixElement)] = (element) => { return new ALittleScriptPropertyValueSuffixReference(element); };
            m_create_map[typeof(ALittleScriptPropertyValueThisTypeElement)] = (element) => { return new ALittleScriptPropertyValueThisTypeReference(element); };
            m_create_map[typeof(ALittleScriptReflectValueElement)] = (element) => { return new ALittleScriptReflectValueReference(element); };
            m_create_map[typeof(ALittleScriptReturnExprElement)] = (element) => { return new ALittleScriptReturnExprReference(element); };
            m_create_map[typeof(ALittleScriptStructDecElement)] = (element) => { return new ALittleScriptStructDecReference(element); };
            m_create_map[typeof(ALittleScriptStructNameDecElement)] = (element) => { return new ALittleScriptStructNameDecReference(element); };
            m_create_map[typeof(ALittleScriptStructVarDecElement)] = (element) => { return new ALittleScriptStructVarDecReference(element); };
            m_create_map[typeof(ALittleScriptStructOptionNameDecElement)] = (element) => { return new ALittleScriptStructOptionNameDecReference(element); };
            m_create_map[typeof(ALittleScriptStructVarNameDecElement)] = (element) => { return new ALittleScriptStructVarNameDecReference(element); };
            m_create_map[typeof(ALittleScriptTcallStatElement)] = (element) => { return new ALittleScriptTcallStatReference(element); };
            m_create_map[typeof(ALittleScriptTemplateDecElement)] = (element) => { return new ALittleScriptTemplateDecReference(element); };
            m_create_map[typeof(ALittleScriptTemplateNameDecElement)] = (element) => { return new ALittleScriptTemplateNameDecReference(element); };
            m_create_map[typeof(ALittleScriptTemplatePairDecElement)] = (element) => { return new ALittleScriptTemplatePairDecReference(element); };
            m_create_map[typeof(ALittleScriptThrowExprElement)] = (element) => { return new ALittleScriptThrowExprReference(element); };
            m_create_map[typeof(ALittleScriptUsingDecElement)] = (element) => { return new ALittleScriptUsingDecReference(element); };
            m_create_map[typeof(ALittleScriptUsingNameDecElement)] = (element) => { return new ALittleScriptUsingNameDecReference(element); };
            m_create_map[typeof(ALittleScriptValueFactorStatElement)] = (element) => { return new ALittleScriptValueFactorStatReference(element); };
            m_create_map[typeof(ALittleScriptValueStatElement)] = (element) => { return new ALittleScriptValueStatReference(element); };
            m_create_map[typeof(ALittleScriptVarAssignDecElement)] = (element) => { return new ALittleScriptVarAssignDecReference(element); };
            m_create_map[typeof(ALittleScriptVarAssignExprElement)] = (element) => { return new ALittleScriptVarAssignExprReference(element); };
            m_create_map[typeof(ALittleScriptVarAssignNameDecElement)] = (element) => { return new ALittleScriptVarAssignNameDecReference(element); };
            m_create_map[typeof(ALittleScriptWrapValueStatElement)] = (element) => { return new ALittleScriptWrapValueStatReference(element); };
            m_create_map[typeof(ALittleScriptLanguageModifierElement)] = (element) => { return new ALittleScriptLanguageModifierReference(element); };
            m_create_map[typeof(ALittleScriptLanguageNameDecElement)] = (element) => { return new ALittleScriptLanguageNameDecReference(element); };

            m_create_map[typeof(ALittleScriptStringElement)] = (element) => { return new ALittleScriptStringReference(element); };
            m_create_map[typeof(ALittleScriptRegexElement)] = (element) => { return new ALittleScriptRegexReference(element); };
            m_create_map[typeof(ALittleScriptTextElement)] = (element) => { return new ALittleScriptTextReference(element); };
            m_create_map[typeof(ALittleScriptNumberElement)] = (element) => { return new ALittleScriptNumberReference(element); };
            m_create_map[typeof(ALittleScriptBlockCommentElement)] = (element) => { return new ALittleScriptBlockCommentReference(element); };
        }

        public override void MainThreadInit()
        {
            ALittleScriptIndex.inst.Start();

            { var T = GeneralOptions.Instance; }
        }

        public override ABnfReference CreateReference(ABnfElement element)
        {
            if (m_create_map.TryGetValue(element.GetType(), out Func<ABnfElement, ABnfReference> fun))
                return fun(element);

            return new ALittleScriptReferenceTemplate<ABnfElement>(element);
        }

        public override TextMarkerTag CreateTextMarkerTag()
        {
            return new ALittleScriptHighlightWordTag();
        }

        public override string GetDotExt() { return ".alittle"; }

        public override byte[] LoadABnf() { return Properties.Resources.ALittleScript; }

        public override ABnfFile CreateABnfFile(string full_path, ABnf abnf, string text)
        {
            return new ALittleScriptFile(full_path, abnf, text);
        }

        public override Icon GetFileIcon()
        {
            return ALittleScriptIndex.inst.sFileIcon;
        }

        public override ProjectInfo CreateProjectInfo(ABnfFactory factory, ABnf abnf, string path)
        {
            return new ALittleScriptProjectInfo(factory, abnf, path);
        }

        public override ABnfGuessError GuessTypes(ABnfElement element, out List<ABnfGuess> guess_list)
         {
            guess_list = null;

            var error = ALittleScriptIndex.inst.GetGuessError(element);
            if (error != null) return error;

            guess_list = ALittleScriptIndex.inst.GetGuessTypeList(element);
            if (guess_list != null && guess_list.Count > 0)
            {
                bool is_changed = false;
                foreach (var guess in guess_list)
                {
                    if (guess.IsChanged())
                    {
                        is_changed = true;
                        break;
                    }
                }
                if (!is_changed) return null;
            }

            var reference = element.GetReference();
            if (reference == null)
            {
                error = new ABnfGuessError(element, "ALittleReference对象创建失败 element:" + element);
                ALittleScriptIndex.inst.AddGuessError(element, error);
                return error;
            }

            error = reference.GuessTypes(out guess_list);
            if (error != null)
            {
                ALittleScriptIndex.inst.AddGuessError(element, error);
                return error;
            }

            if (guess_list == null)
                guess_list = new List<ABnfGuess>();

            // 如果是两个，并且一个是register，一个不是。那么就要把register那个删掉
            if (!reference.MultiGuessTypes()
                && guess_list.Count == 2
                && guess_list[0].GetValue() == guess_list[1].GetValue()
                && guess_list[0] is ALittleScriptGuess
                && guess_list[1] is ALittleScriptGuess)
            {
                if ((guess_list[0] as ALittleScriptGuess).is_register && !(guess_list[1] as ALittleScriptGuess).is_register)
                    guess_list.RemoveAt(0);
                else if (!(guess_list[0] as ALittleScriptGuess).is_register && (guess_list[1] as ALittleScriptGuess).is_register)
                    guess_list.RemoveAt(1);
            }

            foreach (var guess in guess_list)
            {
                if (guess == null)
                {
                    error = new ABnfGuessError(element, "guess列表出现null:" + element);
                    ALittleScriptIndex.inst.AddGuessError(element, error);
                    return error;
                }
            }

            ALittleScriptIndex.inst.AddGuessTypeList(element, guess_list);
            return null;
        }

        public override string ShowKeyWordCompletion(string input, ABnfElement pick)
        {
            var node = pick as ABnfNodeElement;
            if (node == null && pick != null) node = pick.GetParent();

            if (node is ALittleScriptVarAssignDecElement) return input;
            if (node is ALittleScriptTextElement) return input;
            if (node is ALittleScriptPropertyValueDotIdElement) return null;
            if (node is ALittleScriptLineCommentElement) return null;
            if (node is ALittleScriptBlockCommentElement) return null;
            if (node is ALittleScriptClassExtendsDecElement) return null;

            if (node != null
                && node is ALittleScriptIdElement
                && !(node.GetParent() is ALittleScriptCustomTypeNameElement)
                && !(node.GetParent() is ALittleScriptPropertyValueCustomTypeElement))
            {
                return null;
            }

            return input;
        }

        public override int ReCalcSignature(ABnfElement element, int offset)
        {
            ABnfElement parent = element;
            while (parent != null)
            {
                List<ALittleScriptStringElement> string_element_list = null;
                if (parent is ALittleScriptPropertyValueMethodCallElement)
                {
                    var call_element = parent as ALittleScriptPropertyValueMethodCallElement;
                    string_element_list = call_element.GetStringList();
                }
                else if (parent is ALittleScriptOpNewStatElement)
                {
                    var call_element = parent as ALittleScriptOpNewStatElement;
                    string_element_list = call_element.GetStringList();
                }

                if (string_element_list != null)
                {
                    int index = 0;
                    int jump = 0;
                    for (int i = 0; i < string_element_list.Count; ++i)
                    {
                        if (string_element_list[i].GetElementText() != ",")
                        {
                            ++jump;
                            continue;
                        }
                        if (offset > string_element_list[i].GetStart())
                            index = i - jump + 1;
                        else
                            break;
                    }
                    return index;
                }

                if (parent is ALittleScriptMethodBodyDecElement) return -1;
                if (parent is ALittleScriptAllExprElement) return -1;

                parent = parent.GetParent();
            }

            return -1;
        }
    }

    public class ALittleScriptReferenceTemplate<T> : ABnfReferenceTemplate<T> where T : ABnfElement
    {
        private int m_indent = -1;

        public ALittleScriptReferenceTemplate(ABnfElement element) : base(element)
        {
        }

        public ALittleScriptFile m_file { get { return m_element.GetFile() as ALittleScriptFile; } }
        public ALittleScriptProjectInfo m_project { get { return m_element.GetFile().GetProjectInfo() as ALittleScriptProjectInfo; } }

        public override string QueryQuickInfo()
        {
            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return null;
            return guess.GetValue();
        }

        public override ALanguageSignatureInfo QuerySignatureHelp(out int start, out int length)
        {
            start = 0;
            length = 0;
            ABnfElement parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptPropertyValueMethodCallElement)
                {
                    var refe = parent.GetReference() as ALittleScriptPropertyValueMethodCallReference;
                    if (refe == null) return null;

                    var error = refe.GuessPreType(out ABnfGuess guess);
                    if (error != null) return null;

                    var guess_functor = guess as ALittleScriptGuessFunctor;
                    if (guess_functor == null) return null;

                    var info = new ALanguageSignatureInfo();
                    for (int i = 0; i < guess_functor.param_name_list.Count; ++i)
                    {
                        string type = "";
                        if (i < guess_functor.param_nullable_list.Count && guess_functor.param_nullable_list[i])
                            type = "[Nullable] ";
                        if (i < guess_functor.param_list.Count)
                            type += guess_functor.param_list[i].GetValue();

                        var param = new ALanguageParameterInfo();
                        param.name = type + " " + guess_functor.param_name_list[i];

                        info.param_list.Add(param);
                    }
                    if (guess_functor.param_tail != null)
                    {
                        var param = new ALanguageParameterInfo();
                        param.name = guess_functor.param_tail.GetValue();

                        info.param_list.Add(param);
                    }

                    start = parent.GetStart();
                    length = parent.GetLengthWithoutError();
                    var method_call = parent as ALittleScriptPropertyValueMethodCallElement;
                    var string_list = method_call.GetStringList();
                    if (string_list.Count > 1)
                    {
                        start = string_list[0].GetStart();
                        length = parent.GetLength() - (string_list[0].GetStart() - parent.GetStart()) - 1;
                    }
                    return info;
                }
                else if (parent is ALittleScriptOpNewStatElement)
                {
                    var custom_type = (parent as ALittleScriptOpNewStatElement).GetCustomType();
                    if (custom_type == null) return null;

                    var error = custom_type.GuessType(out ABnfGuess guess);
                    if (error != null) return null;

                    if (guess is ALittleScriptGuessTemplate)
                    {
                        var guess_template = guess as ALittleScriptGuessTemplate;
                        if (guess_template.template_extends != null)
                            guess = guess_template.template_extends;
                    }

                    if (guess is ALittleScriptGuessClass)
                    {
                        var class_dec = ((ALittleScriptGuessClass)guess).class_dec;
                        var ctor = ALittleScriptUtility.FindFirstCtorDecFromExtends(class_dec, 100);
                        if (ctor == null) return null;

                        var param_dec = ctor.GetMethodParamDec();
                        if (param_dec == null) return null;

                        var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                        if (param_one_dec_list.Count == 0) return null;

                        var info = new ALanguageSignatureInfo();
                        for (int i = 0; i < param_one_dec_list.Count; ++i)
                        {
                            var param_one_dec = param_one_dec_list[i];
                            var tail_dec = param_one_dec.GetMethodParamTailDec();
                            if (tail_dec != null)
                            {
                                var param_info = new ALanguageParameterInfo();
                                param_info.name = tail_dec.GetElementText();
                                info.param_list.Add(param_info);
                                continue;
                            }

                            var nullable = ALittleScriptUtility.IsNullable(param_one_dec.GetModifierList());

                            var all_type = param_one_dec.GetAllType();
                            if (all_type == null) return null;

                            error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return null;

                            var param = new ALanguageParameterInfo();
                            param.name = all_type_guess.GetValue();
                            if (param_one_dec.GetMethodParamNameDec() != null)
                            {
                                if (nullable) param.name += " [Nullable]";
                                param.name += " " + param_one_dec.GetMethodParamNameDec().GetElementText();
                            }

                            info.param_list.Add(param);
                        }

                        start = parent.GetStart();
                        length = parent.GetLengthWithoutError();
                        var new_stat = parent as ALittleScriptOpNewStatElement;
                        var string_list = new_stat.GetStringList();
                        if (string_list.Count > 1)
                        {
                            start = string_list[0].GetStart();
                            length = parent.GetLength() - (string_list[0].GetStart() - parent.GetStart()) - 1;
                        }
                        return info;
                    }
                    return null;
                }

                if (parent is ALittleScriptMethodBodyDecElement) return null;
                if (parent is ALittleScriptAllExprElement) return null;

                parent = parent.GetParent();
            }

            return null;
        }

        public override int GetDesiredIndentation(int offset, ABnfElement select)
        {
            if (m_indent >= 0) return m_indent;

            ABnfElement parent = m_element.GetParent();
            if (parent == null)
            {
                m_indent = 0;
                return m_indent;
            }
            
            if (m_element is ALittleScriptClassBodyDecElement
                || m_element is ALittleScriptStructBodyDecElement
                || m_element is ALittleScriptEnumBodyDecElement
                || m_element is ALittleScriptMethodBodyDecElement
                || m_element is ALittleScriptForBodyElement
                || m_element is ALittleScriptWhileBodyElement
                || m_element is ALittleScriptDoWhileBodyElement
                || m_element is ALittleScriptIfBodyElement
                || m_element is ALittleScriptElseIfBodyElement
                || m_element is ALittleScriptElseBodyElement
                || m_element is ALittleScriptWrapExprElement)
            {
                if (select is ABnfStringElement && (select.GetElementText() == "{" || select.GetElementText() == "}"))
                {
                    m_indent = parent.GetReference().GetDesiredIndentation(offset, null);
                    return m_indent;
                }

                m_indent = parent.GetReference().GetDesiredIndentation(offset, null) + ALanguageSmartIndentProvider.s_indent_size;
                return m_indent;
            }
            else if (m_element is ALittleScriptMethodParamDecElement
                || m_element is ALittleScriptOpNewListStatElement)
            {
                var element = m_element as ABnfNodeElement;
                var childs = element.GetChilds();
                if (childs.Count > 0)
                {
                    m_indent = childs[0].GetStartIndent();
                    return m_indent;
                }
            }

            m_indent = parent.GetReference().GetDesiredIndentation(offset, null);
            return m_indent;
        }

    }
}

