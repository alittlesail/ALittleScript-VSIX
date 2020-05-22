
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOpNewListStatElement : ABnfNodeElement
	{
		public ALittleScriptOpNewListStatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptValueStatElement> m_list_ValueStat = null;
        public List<ALittleScriptValueStatElement> GetValueStatList()
        {
            var list = new List<ALittleScriptValueStatElement>();
            if (m_list_ValueStat == null)
            {
                m_list_ValueStat = new List<ALittleScriptValueStatElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptValueStatElement)
                        m_list_ValueStat.Add(child as ALittleScriptValueStatElement);
                }   
            }
            list.AddRange(m_list_ValueStat);
            return list;
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