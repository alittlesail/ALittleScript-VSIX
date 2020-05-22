
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptEnumData
    {
        // Key:名称，Value:成员
        private Dictionary<string, ALittleScriptEnumVarDecElement> m_element_map = new Dictionary<string, ALittleScriptEnumVarDecElement>();

        // 添加新元素
        public void AddVarDec(ALittleScriptEnumVarDecElement dec)
        {
            var name_dec = dec.GetEnumVarNameDec();
            if (name_dec == null) return;
            string name = name_dec.GetElementText();
            if (m_element_map.ContainsKey(name)) return;
            m_element_map.Add(name, dec);
        }

        // 查找元素
        public void FindVarDecList(string name, List<ALittleScriptEnumVarDecElement> result)
        {
            if (name.Length == 0)
                result.AddRange(m_element_map.Values);
            else
            {
                if (m_element_map.TryGetValue(name, out ALittleScriptEnumVarDecElement dec))
                    result.Add(dec);
            }
        }
    }
}
