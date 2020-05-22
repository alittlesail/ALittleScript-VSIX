
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptAccessData
    {
        // Key1:元素类型，Key2:名称，Value:对应的元素集合
        private Dictionary<ALittleScriptUtility.ABnfElementType, Dictionary<string, HashSet<ABnfElement>>> m_element_map
            = new Dictionary<ALittleScriptUtility.ABnfElementType, Dictionary<string, HashSet<ABnfElement>>>();

        // 获取集合
        public Dictionary<ALittleScriptUtility.ABnfElementType, Dictionary<string, HashSet<ABnfElement>>> GetElementMap()
        {
            return m_element_map;
        }

        // 添加新元素
        public void AddNameDec(ABnfElement dec)
        {
            // 获取名称
            string name = dec.GetElementText();

            // 计算类型
            ALittleScriptUtility.ABnfElementType type;
            if (dec is ALittleScriptClassNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.CLASS_NAME;
            else if (dec is ALittleScriptEnumNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.ENUM_NAME;
            else if (dec is ALittleScriptStructNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.STRUCT_NAME;
            else if (dec is ALittleScriptVarAssignNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.INSTANCE_NAME;
            else if (dec is ALittleScriptMethodNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD;
            else if (dec is ALittleScriptUsingNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.USING_NAME;
            else
                return;

            // 添加到映射表
            if (!m_element_map.TryGetValue(type, out Dictionary<string, HashSet<ABnfElement>> map))
            {
                map = new Dictionary<string, HashSet<ABnfElement>>();
                m_element_map.Add(type, map);
            }
            if (!map.TryGetValue(name, out HashSet<ABnfElement> set))
            {
                set = new HashSet<ABnfElement>();
                map.Add(name, set);
            }
            if (!set.Contains(dec)) set.Add(dec);
        }

        // 查找元素
        public void FindNameDecList(ALittleScriptUtility.ABnfElementType type, string name, List<ABnfElement> result)
        {
            if (!m_element_map.TryGetValue(type, out Dictionary<string, HashSet<ABnfElement>> map)) return;
            
            if (name.Length == 0)
            {
                foreach (var pair in map)
                    result.AddRange(pair.Value);
                return;
            }

            if (!map.TryGetValue(name, out HashSet<ABnfElement> set)) return;
            result.AddRange(set);
        }

        // 移除元素
        public void RemoveNameDec(ABnfElement dec)
        {
            // 获取名称
            string name = dec.GetElementText();

            // 计算类型
            ALittleScriptUtility.ABnfElementType type;
            if (dec is ALittleScriptClassNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.CLASS_NAME;
            else if (dec is ALittleScriptEnumNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.ENUM_NAME;
            else if (dec is ALittleScriptStructNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.STRUCT_NAME;
            else if (dec is ALittleScriptVarAssignNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.INSTANCE_NAME;
            else if (dec is ALittleScriptMethodNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD;
            else if (dec is ALittleScriptUsingNameDecElement)
                type = ALittleScriptUtility.ABnfElementType.USING_NAME;
            else
                return;

            if (!m_element_map.TryGetValue(type, out Dictionary<string, HashSet<ABnfElement>> map)) return;
            if (!map.TryGetValue(name, out HashSet<ABnfElement> set)) return;
            set.Remove(dec);
            if (set.Count == 0) map.Remove(name);
        }
    }
}
