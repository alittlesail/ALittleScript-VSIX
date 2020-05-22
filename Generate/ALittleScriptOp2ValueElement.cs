
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp2ValueElement : ABnfNodeElement
	{
		public ALittleScriptOp2ValueElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op2 = false;
        private ALittleScriptOp2Element m_cache_Op2 = null;
        public ALittleScriptOp2Element GetOp2()
        {
            if (m_flag_Op2) return m_cache_Op2;
            m_flag_Op2 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp2Element)
                {
                    m_cache_Op2 = child as ALittleScriptOp2Element;
                    break;
                }
            }
            return m_cache_Op2;
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

	}
}