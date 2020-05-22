
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp4SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp4SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op4 = false;
        private ALittleScriptOp4Element m_cache_Op4 = null;
        public ALittleScriptOp4Element GetOp4()
        {
            if (m_flag_Op4) return m_cache_Op4;
            m_flag_Op4 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp4Element)
                {
                    m_cache_Op4 = child as ALittleScriptOp4Element;
                    break;
                }
            }
            return m_cache_Op4;
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
        List<ALittleScriptOp4SuffixEeElement> m_list_Op4SuffixEe = null;
        public List<ALittleScriptOp4SuffixEeElement> GetOp4SuffixEeList()
        {
            var list = new List<ALittleScriptOp4SuffixEeElement>();
            if (m_list_Op4SuffixEe == null)
            {
                m_list_Op4SuffixEe = new List<ALittleScriptOp4SuffixEeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp4SuffixEeElement)
                        m_list_Op4SuffixEe.Add(child as ALittleScriptOp4SuffixEeElement);
                }   
            }
            list.AddRange(m_list_Op4SuffixEe);
            return list;
        }

	}
}