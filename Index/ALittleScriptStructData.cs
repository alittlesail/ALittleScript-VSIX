
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptStructData
    {
        // Key:名称，Value:成员
        private Dictionary<string, ALittleScriptStructVarDecElement> m_element_map = new Dictionary<string, ALittleScriptStructVarDecElement>();

        // 添加新元素
        public void AddVarDec(ALittleScriptStructVarDecElement dec)
        {
            var name_dec = dec.GetStructVarNameDec();
            if (name_dec == null) return;
            string name = name_dec.GetElementText();
            if (m_element_map.ContainsKey(name)) return;
            m_element_map.Add(name, dec);
        }

        // 查找元素
        public void FindVarDecList(string name, List<ALittleScriptStructVarDecElement> result)
        {
            if (name.Length == 0)
                result.AddRange(m_element_map.Values);
            else
            {
                if (m_element_map.TryGetValue(name, out ALittleScriptStructVarDecElement dec))
                    result.Add(dec);
            }
        }
    }
}
