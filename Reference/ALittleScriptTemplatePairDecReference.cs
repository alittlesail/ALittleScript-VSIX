
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptTemplatePairDecReference : ALittleScriptReferenceTemplate<ALittleScriptTemplatePairDecElement>
    {
        public ALittleScriptTemplatePairDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            ALittleScriptAllTypeElement all_type = null;
            ALittleScriptTemplateExtendsClassDecElement extends_class_dec = null;
            ALittleScriptTemplateExtendsStructDecElement extends_struct_dec = null;

            var extends = m_element.GetTemplateExtendsDec();
            if (extends != null)
            {
                all_type = extends.GetAllType();
                extends_class_dec = extends.GetTemplateExtendsClassDec();
                extends_struct_dec = extends.GetTemplateExtendsStructDec();
            }

            ABnfGuess template_extends = null;
            bool is_class = false;
            bool is_struct = false;
            if (all_type != null)
            {
                var error = all_type.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (!(guess is ALittleScriptGuessClass) && !(guess is ALittleScriptGuessStruct)) {
                    return new ABnfGuessError(all_type, "继承的对象必须是一个类或者结构体");
                }
                template_extends = guess;
            }
            else if (extends_class_dec != null)
            {
                is_class = true;
            }
            else if (extends_struct_dec != null)
            {
                is_struct = true;
            }

            if (m_element.GetParent() == null) return new ABnfGuessError(m_element, "没有父节点");
            var parent = m_element.GetParent();
            if (parent.GetParent() == null) return new ABnfGuessError(parent, "没有父节点");
            parent = parent.GetParent();

            // 根据定义区分类模板还是函数模板
            if (parent is ALittleScriptClassDecElement)
            {
                var info = new ALittleScriptGuessClassTemplate(m_element, template_extends, is_class, is_struct);
                info.UpdateValue();
                guess_list.Add(info);
            }
            else
            {
                var info = new ALittleScriptGuessMethodTemplate(m_element, template_extends, is_class, is_struct);
                info.UpdateValue();
                guess_list.Add(info);
            }
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var name_dec = m_element.GetTemplateNameDec();
            if (name_dec == null) return null;

            if (name_dec.GetElementText().StartsWith("___"))
                return new ABnfGuessError(name_dec, "模板名不能以3个下划线开头");
            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(name_dec, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(name_dec, "重复定义");

            return null;
        }
    }
}

