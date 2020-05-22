
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp5SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp5SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op5 = false;
        private ALittleScriptOp5Element m_cache_Op5 = null;
        public ALittleScriptOp5Element GetOp5()
        {
            if (m_flag_Op5) return m_cache_Op5;
            m_flag_Op5 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp5Element)
                {
                    m_cache_Op5 = child as ALittleScriptOp5Element;
                    break;
                }
            }
            return m_cache_Op5;
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
        List<ALittleScriptOp5SuffixEeElement> m_list_Op5SuffixEe = null;
        public List<ALittleScriptOp5SuffixEeElement> GetOp5SuffixEeList()
        {
            var list = new List<ALittleScriptOp5SuffixEeElement>();
            if (m_list_Op5SuffixEe == null)
            {
                m_list_Op5SuffixEe = new List<ALittleScriptOp5SuffixEeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp5SuffixEeElement)
                        m_list_Op5SuffixEe.Add(child as ALittleScriptOp5SuffixEeElement);
                }   
            }
            list.AddRange(m_list_Op5SuffixEe);
            return list;
        }

	}
}