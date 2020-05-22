
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp6SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp6SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op6 = false;
        private ALittleScriptOp6Element m_cache_Op6 = null;
        public ALittleScriptOp6Element GetOp6()
        {
            if (m_flag_Op6) return m_cache_Op6;
            m_flag_Op6 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp6Element)
                {
                    m_cache_Op6 = child as ALittleScriptOp6Element;
                    break;
                }
            }
            return m_cache_Op6;
        }
        private bool m_flag_ValueFactorStat = false;
        private ALittleScriptValueFactorStatElement m_cache_ValueFactorStat = null;
        public ALittleScriptValueFactorStatElement GetValueFactorStat()
        {
            if (m_flag_ValueFactorStat) return m_cache_ValueFactorStat;
            m_flag_ValueFactorStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptValueFactorStatElement)
                {
                    m_cache_ValueFactorStat = child as ALittleScriptValueFactorStatElement;
                    break;
                }
            }
            return m_cache_ValueFactorStat;
        }
        private bool m_flag_Op2Value = false;
        private ALittleScriptOp2ValueElement m_cache_Op2Value = null;
        public ALittleScriptOp2ValueElement GetOp2Value()
        {
            if (m_flag_Op2Value) return m_cache_Op2Value;
            m_flag_Op2Value = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp2ValueElement)
                {
                    m_cache_Op2Value = child as ALittleScriptOp2ValueElement;
                    break;
                }
            }
            return m_cache_Op2Value;
        }
        List<ALittleScriptOp6SuffixEeElement> m_list_Op6SuffixEe = null;
        public List<ALittleScriptOp6SuffixEeElement> GetOp6SuffixEeList()
        {
            var list = new List<ALittleScriptOp6SuffixEeElement>();
            if (m_list_Op6SuffixEe == null)
            {
                m_list_Op6SuffixEe = new List<ALittleScriptOp6SuffixEeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp6SuffixEeElement)
                        m_list_Op6SuffixEe.Add(child as ALittleScriptOp6SuffixEeElement);
                }   
            }
            list.AddRange(m_list_Op6SuffixEe);
            return list;
        }

	}
}