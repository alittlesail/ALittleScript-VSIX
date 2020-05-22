
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptWhileConditionElement : ABnfNodeElement
	{
		public ALittleScriptWhileConditionElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ValueStat = false;
        private ALittleScriptValueStatElement m_cache_ValueStat = null;
        public ALittleScriptValueStatElement GetValueStat()
        {
            if (m_flag_ValueStat) return m_cache_ValueStat;
            m_flag_ValueStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptValueStatElement)
                {
                    m_cache_ValueStat = child as ALittleScriptValueStatElement;
                    break;
                }
            }
            return m_cache_ValueStat;
        }
        List<ALittleScriptStringElement> m_list_String = null;
        public List<ALittleScriptStringElement> GetStringList()
        {
            var list = new List<ALittleScriptStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<ALittleScriptStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStringElement)
                        m_list_String.Add(child as ALittleScriptStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}