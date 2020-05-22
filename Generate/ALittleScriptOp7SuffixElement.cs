
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp7SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp7SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op7 = false;
        private ALittleScriptOp7Element m_cache_Op7 = null;
        public ALittleScriptOp7Element GetOp7()
        {
            if (m_flag_Op7) return m_cache_Op7;
            m_flag_Op7 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp7Element)
                {
                    m_cache_Op7 = child as ALittleScriptOp7Element;
                    break;
                }
            }
            return m_cache_Op7;
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
        List<ALittleScriptOp7SuffixEeElement> m_list_Op7SuffixEe = null;
        public List<ALittleScriptOp7SuffixEeElement> GetOp7SuffixEeList()
        {
            var list = new List<ALittleScriptOp7SuffixEeElement>();
            if (m_list_Op7SuffixEe == null)
            {
                m_list_Op7SuffixEe = new List<ALittleScriptOp7SuffixEeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp7SuffixEeElement)
                        m_list_Op7SuffixEe.Add(child as ALittleScriptOp7SuffixEeElement);
                }   
            }
            list.AddRange(m_list_Op7SuffixEe);
            return list;
        }

	}
}