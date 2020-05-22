
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp1ExprElement : ABnfNodeElement
	{
		public ALittleScriptOp1ExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op1 = false;
        private ALittleScriptOp1Element m_cache_Op1 = null;
        public ALittleScriptOp1Element GetOp1()
        {
            if (m_flag_Op1) return m_cache_Op1;
            m_flag_Op1 = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp1Element)
                {
                    m_cache_Op1 = child as ALittleScriptOp1Element;
                    break;
                }
            }
            return m_cache_Op1;
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
        private bool m_flag_String = false;
        private ALittleScriptStringElement m_cache_String = null;
        public ALittleScriptStringElement GetString()
        {
            if (m_flag_String) return m_cache_String;
            m_flag_String = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStringElement)
                {
                    m_cache_String = child as ALittleScriptStringElement;
                    break;
                }
            }
            return m_cache_String;
        }

	}
}