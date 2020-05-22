
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp8SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp8SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op8 = false;
        private ALittleScriptOp8Element m_cache_Op8 = null;
        public ALittleScriptOp8Element GetOp8()
        {
            if (m_flag_Op8) return m_cache_Op8;
            m_flag_Op8 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp8Element)
                {
                    m_cache_Op8 = child as ALittleScriptOp8Element;
                    break;
                }
            }
            return m_cache_Op8;
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
        List<ALittleScriptOp8SuffixEeElement> m_list_Op8SuffixEe = null;
        public List<ALittleScriptOp8SuffixEeElement> GetOp8SuffixEeList()
        {
            var list = new List<ALittleScriptOp8SuffixEeElement>();
            if (m_list_Op8SuffixEe == null)
            {
                m_list_Op8SuffixEe = new List<ALittleScriptOp8SuffixEeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp8SuffixEeElement)
                        m_list_Op8SuffixEe.Add(child as ALittleScriptOp8SuffixEeElement);
                }   
            }
            list.AddRange(m_list_Op8SuffixEe);
            return list;
        }

	}
}