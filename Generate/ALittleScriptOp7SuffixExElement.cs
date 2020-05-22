
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp7SuffixExElement : ABnfNodeElement
	{
		public ALittleScriptOp7SuffixExElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op7Suffix = false;
        private ALittleScriptOp7SuffixElement m_cache_Op7Suffix = null;
        public ALittleScriptOp7SuffixElement GetOp7Suffix()
        {
            if (m_flag_Op7Suffix) return m_cache_Op7Suffix;
            m_flag_Op7Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp7SuffixElement)
                {
                    m_cache_Op7Suffix = child as ALittleScriptOp7SuffixElement;
                    break;
                }
            }
            return m_cache_Op7Suffix;
        }
        private bool m_flag_Op8Suffix = false;
        private ALittleScriptOp8SuffixElement m_cache_Op8Suffix = null;
        public ALittleScriptOp8SuffixElement GetOp8Suffix()
        {
            if (m_flag_Op8Suffix) return m_cache_Op8Suffix;
            m_flag_Op8Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp8SuffixElement)
                {
                    m_cache_Op8Suffix = child as ALittleScriptOp8SuffixElement;
                    break;
                }
            }
            return m_cache_Op8Suffix;
        }

	}
}