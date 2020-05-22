
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp3SuffixElement : ABnfNodeElement
	{
		public ALittleScriptOp3SuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op3 = false;
        private ALittleScriptOp3Element m_cache_Op3 = null;
        public ALittleScriptOp3Element GetOp3()
        {
            if (m_flag_Op3) return m_cache_Op3;
            m_flag_Op3 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp3Element)
                {
                    m_cache_Op3 = child as ALittleScriptOp3Element;
                    break;
                }
            }
            return m_cache_Op3;
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

	}
}