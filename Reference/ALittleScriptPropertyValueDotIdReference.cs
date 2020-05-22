
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueDotIdReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueDotIdElement>
    {
        public ALittleScriptPropertyValueDotIdReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var dot_id_name = m_element.GetPropertyValueDotIdName();
            if (dot_id_name != null)
                return dot_id_name.GuessTypes(out guess_list);

            guess_list = new List<ABnfGuess>();
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            // 获取父节点
            var property_value_dot_id = m_element;
            var property_value_suffix = property_value_dot_id.GetParent() as ALittleScriptPropertyValueSuffixElement;
            var property_value = property_value_suffix.GetParent() as ALittleScriptPropertyValueElement;
            var property_value_first_type = property_value.GetPropertyValueFirstType();
            var suffix_list = property_value.GetPropertyValueSuffixList();

            // 获取所在位置
            int index = suffix_list.IndexOf(property_value_suffix);
            if (index == -1) return false;

            // 获取前一个类型
            ABnfGuess pre_type;
            ABnfGuessError error = null;
            if (index == 0)
                error = property_value_first_type.GuessType(out pre_type);
            else
                error = suffix_list[index - 1].GuessType(out pre_type);
            if (error != null) return false;

            bool is_const = false;
            if (pre_type != null) is_const = pre_type.is_const;

            if (pre_type is ALittleScriptGuessTemplate)
            {
                pre_type = (pre_type as ALittleScriptGuessTemplate).template_extends;
                if (is_const && !pre_type.is_const)
                {
                    pre_type = pre_type.Clone();
                    pre_type.is_const = true;
                    pre_type.UpdateValue();
                }
            }

            if (pre_type == null) return false;

            // 处理类的实例对象
            if (pre_type is ALittleScriptGuessClass)
            {
                var class_dec = (pre_type as ALittleScriptGuessClass).class_dec;

                // 计算当前元素对这个类的访问权限
                int access_level = ALittleScriptUtility.CalcAccessLevelByTargetClassDecForElement(m_element, class_dec);

                var class_var_dec_list = new List<ABnfElement>();
                // 所有成员变量
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.VAR, "", class_var_dec_list, 100);
                foreach (var dec in class_var_dec_list)
                {
                    var class_var_dec = dec as ALittleScriptClassVarDecElement;
                    if (class_var_dec == null) continue;
                    var class_var_name_dec = class_var_dec.GetClassVarNameDec();
                    if (class_var_name_dec == null) continue;
                    list.Add(new ALanguageCompletionInfo(class_var_name_dec.GetElementText(), ALittleScriptIndex.inst.sVariableIcon));
                }

                // 所有setter,getter
                var class_method_name_dec_list = new List<ABnfElement>();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.SETTER, "", class_method_name_dec_list, 100);
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.GETTER, "", class_method_name_dec_list, 100);
                class_method_name_dec_list = ALittleScriptUtility.FilterSameName(class_method_name_dec_list);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_method_name_dec.GetElementText(), ALittleScriptIndex.inst.sFieldMethodIcon));

                // 所有成员函数
                class_method_name_dec_list.Clear();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.FUN, "", class_method_name_dec_list, 10);
                class_method_name_dec_list = ALittleScriptUtility.FilterSameName(class_method_name_dec_list);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_method_name_dec.GetElementText(), ALittleScriptIndex.inst.sMemberMethodIcon));

            }
            // 处理结构体的实例对象
            else if (pre_type is ALittleScriptGuessStruct)
            {
                var struct_dec = ((ALittleScriptGuessStruct)pre_type).struct_dec;
                var struct_var_dec_list = new List<ALittleScriptStructVarDecElement>();
                // 所有成员变量
                ALittleScriptUtility.FindStructVarDecList(struct_dec, "", struct_var_dec_list, 100);
                foreach (var struct_var_dec in struct_var_dec_list)
                {
                    var name_dec = struct_var_dec.GetStructVarNameDec();
                    if (name_dec == null) continue;
                    list.Add(new ALanguageCompletionInfo(name_dec.GetElementText(), ALittleScriptIndex.inst.sPropertyIcon));
                }
            }
            // 比如 ALittleName.XXX
            else if (pre_type is ALittleScriptGuessNamespaceName)
            {
                var namespace_name_dec = ((ALittleScriptGuessNamespaceName)pre_type).namespace_name_dec;
                string namespace_name = namespace_name_dec.GetElementText();
                // 所有枚举名
                var enum_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), namespace_name, "", true);
                foreach (var enum_name_dec in enum_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(enum_name_dec.GetElementText(), ALittleScriptIndex.inst.sEnumIcon));
                // 所有全局函数
                var method_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), namespace_name, "", true);
                foreach (var method_name_dec in method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(method_name_dec.GetElementText(), ALittleScriptIndex.inst.sGlobalMethodIcon));
                // 所有类名
                var class_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), namespace_name, "", true);
                foreach (var class_name_dec in class_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_name_dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
                // 所有结构体名
                var struct_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), namespace_name, "", true);
                foreach (var struct_name_dec in struct_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(struct_name_dec.GetElementText(), ALittleScriptIndex.inst.sStructIcon));
                // 所有单例
                var instance_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.INSTANCE_NAME, m_element.GetFile(), namespace_name, "", false);
                foreach (var instance_name_dec in instance_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(instance_name_dec.GetElementText(), ALittleScriptIndex.inst.sInstanceIcon));
            }
            // 比如 AClassName.XXX
            else if (pre_type is ALittleScriptGuessClassName)
            {
                var class_name_dec = ((ALittleScriptGuessClassName)pre_type).class_name_dec;
                var class_dec = class_name_dec.GetParent() as ALittleScriptClassDecElement;

                // 计算当前元素对这个类的访问权限
                int access_level = ALittleScriptUtility.CalcAccessLevelByTargetClassDecForElement(m_element, class_dec);

                // 所有静态函数
                var class_method_name_dec_list = new List<ABnfElement>();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.STATIC, "", class_method_name_dec_list, 100);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_method_name_dec.GetElementText(), ALittleScriptIndex.inst.sStaticMethodIcon));
                // 所有成员函数
                class_method_name_dec_list.Clear();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.FUN, "", class_method_name_dec_list, 100);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_method_name_dec.GetElementText(), ALittleScriptIndex.inst.sMemberMethodIcon));
                // 所有setter,getter
                class_method_name_dec_list.Clear();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.SETTER, "", class_method_name_dec_list, 100);
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.GETTER, "", class_method_name_dec_list, 100);
                class_method_name_dec_list = ALittleScriptUtility.FilterSameName(class_method_name_dec_list);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    list.Add(new ALanguageCompletionInfo(class_method_name_dec.GetElementText(), ALittleScriptIndex.inst.sFieldMethodIcon));;
            }
            // 比如 AEnum.XXX
            else if (pre_type is ALittleScriptGuessEnumName)
            {
                // 所有枚举字段
                var enum_name_dec = ((ALittleScriptGuessEnumName)pre_type).enum_name_dec;
                var enum_dec = enum_name_dec.GetParent() as ALittleScriptEnumDecElement;
                var var_dec_list = new List<ALittleScriptEnumVarDecElement>();
                ALittleScriptUtility.FindEnumVarDecList(enum_dec, "", var_dec_list);
                foreach (var var_dec in var_dec_list)
                {
                    var var_name_dec = var_dec.GetEnumVarNameDec();
                    if (var_name_dec == null) continue;
                    list.Add(new ALanguageCompletionInfo(var_name_dec.GetElementText(), ALittleScriptIndex.inst.sEnumIcon));
                }
            }

            return true;
        }
    }
}

