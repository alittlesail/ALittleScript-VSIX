
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassData
    {
        // Key1:访问权限，Key2:属性类型，Key3:名称，Value:元素
        private Dictionary<ALittleScriptUtility.ClassAccessType, Dictionary<ALittleScriptUtility.ClassAttrType, Dictionary<string, ABnfElement>>> m_element_map
            = new Dictionary<ALittleScriptUtility.ClassAccessType, Dictionary<ALittleScriptUtility.ClassAttrType, Dictionary<string, ABnfElement>>>();

        // 添加新元素
        public void AddClassChildDec(ABnfElement dec)
        {
            // 模板定义特殊处理
            if (dec is ALittleScriptTemplateDecElement)
            {
                var template_dec = dec as ALittleScriptTemplateDecElement;
                var pair_dec_list = template_dec.GetTemplatePairDecList();
                foreach (var pair_dec in pair_dec_list)
                    AddClassChildDec(pair_dec);
                return;
            }

            ALittleScriptUtility.ClassAccessType access_type = ALittleScriptUtility.ClassAccessType.PRIVATE;
            ALittleScriptUtility.ClassAttrType attr_type;
            string name;

            // 处理模板参数
            if (dec is ALittleScriptTemplatePairDecElement)
            {
                var pair_dec = dec as ALittleScriptTemplatePairDecElement;
                name = pair_dec.GetTemplateNameDec().GetElementText();
                access_type = ALittleScriptUtility.ClassAccessType.PUBLIC;
                attr_type = ALittleScriptUtility.ClassAttrType.TEMPLATE;
            }
            else if (dec is ALittleScriptClassElementDecElement)
            {
                var element_dec = dec as ALittleScriptClassElementDecElement;
                access_type = ALittleScriptUtility.CalcAccessType(element_dec.GetModifierList());
                // 处理成员变量
                if (element_dec.GetClassVarDec() != null)
                {
                    var var_dec = element_dec.GetClassVarDec();
                    var name_dec = var_dec.GetClassVarNameDec();
                    if (name_dec == null) return;
                    name = name_dec.GetElementText();
                    attr_type = ALittleScriptUtility.ClassAttrType.VAR;
                    dec = element_dec.GetClassVarDec();
                }
                // 处理成员函数
                else if (element_dec.GetClassMethodDec() != null)
                {
                    var method_dec = element_dec.GetClassMethodDec();
                    var name_dec = method_dec.GetMethodNameDec();
                    if (name_dec == null) return;
                    dec = name_dec;
                    name = name_dec.GetElementText();
                    attr_type = ALittleScriptUtility.ClassAttrType.FUN;
                }
                // 处理getter函数
                else if (element_dec.GetClassGetterDec() != null)
                {
                    var method_dec = element_dec.GetClassGetterDec();
                    var name_dec = method_dec.GetMethodNameDec();
                    if (name_dec == null) return;
                    dec = name_dec;
                    name = name_dec.GetElementText();
                    attr_type = ALittleScriptUtility.ClassAttrType.GETTER;
                }
                // 处理setter函数
                else if (element_dec.GetClassSetterDec() != null)
                {
                    var method_dec = element_dec.GetClassSetterDec();
                    var name_dec = method_dec.GetMethodNameDec();
                    if (name_dec == null) return;
                    dec = name_dec;
                    name = name_dec.GetElementText();
                    attr_type = ALittleScriptUtility.ClassAttrType.SETTER;
                }
                // 处理静态函数
                else if (element_dec.GetClassStaticDec() != null)
                {
                    var method_dec = element_dec.GetClassStaticDec();
                    var name_dec = method_dec.GetMethodNameDec();
                    if (name_dec == null) return;
                    dec = name_dec;
                    name = name_dec.GetElementText();
                    attr_type = ALittleScriptUtility.ClassAttrType.STATIC;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (!m_element_map.TryGetValue(access_type, out Dictionary<ALittleScriptUtility.ClassAttrType, Dictionary<string, ABnfElement>> map))
            {
                map = new Dictionary<ALittleScriptUtility.ClassAttrType, Dictionary<string, ABnfElement>>();
                m_element_map.Add(access_type, map);
            }
            if (!map.TryGetValue(attr_type, out Dictionary<string, ABnfElement> sub_map))
            {
                sub_map = new Dictionary<string, ABnfElement>();
                map.Add(attr_type, sub_map);
            }
            if (!sub_map.ContainsKey(name))
                sub_map.Add(name, dec);
        }

        // 获取元素集合
        private Dictionary<string, ABnfElement> GetElementMap(ALittleScriptUtility.ClassAttrType attr_type, ALittleScriptUtility.ClassAccessType access_type)
        {
            if (!m_element_map.TryGetValue(access_type, out Dictionary<ALittleScriptUtility.ClassAttrType, Dictionary<string, ABnfElement>> map)) return null;
            if (!map.TryGetValue(attr_type, out Dictionary<string, ABnfElement> sub_map)) return null;
            return sub_map;
        }

        // 查找元素
        public void FindClassAttrList(int access_level, ALittleScriptUtility.ClassAttrType attr_type, string name, List<ABnfElement> result)
        {
            if (access_level >= ALittleScriptUtility.sAccessOnlyPublic)
            {
                var map = GetElementMap(attr_type, ALittleScriptUtility.ClassAccessType.PUBLIC);
                if (map != null)
                {
                    if (name.Length == 0)
                        result.AddRange(map.Values);
                    else
                    {
                        if (map.TryGetValue(name, out ABnfElement dec))
                            result.Add(dec);
                    }
                }
            }

            if (access_level >= ALittleScriptUtility.sAccessProtectedAndPublic)
            {
                var map = GetElementMap(attr_type, ALittleScriptUtility.ClassAccessType.PROTECTED);
                if (map != null)
                {
                    if (name.Length == 0)
                        result.AddRange(map.Values);
                    else
                    {
                        if (map.TryGetValue(name, out ABnfElement dec))
                            result.Add(dec);
                    }
                }
            }

            if (access_level >= ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic)
            {
                var map = GetElementMap(attr_type, ALittleScriptUtility.ClassAccessType.PRIVATE);
                if (map != null)
                {
                    if (name.Length == 0)
                        result.AddRange(map.Values);
                    else
                    {
                        if (map.TryGetValue(name, out ABnfElement dec))
                            result.Add(dec);
                    }
                }
            }
        }
    }
}
