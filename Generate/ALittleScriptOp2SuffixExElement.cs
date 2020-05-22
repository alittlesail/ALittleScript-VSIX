
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp2SuffixExElement : ABnfNodeElement
	{
		public ALittleScriptOp2SuffixExElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op3Suffix = false;
        private ALittleScriptOp3SuffixElement m_cache_Op3Suffix = null;
        public ALittleScriptOp3SuffixElement GetOp3Suffix()
        {
            if (m_flag_Op3Suffix) return m_cache_Op3Suffix;
            m_flag_Op3Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp3SuffixElement)
                {
                    m_cache_Op3Suffix = child as ALittleScriptOp3SuffixElement;
                    break;
                }
            }
            return m_cache_Op3Suffix;
        }
        private bool m_flag_Op4Suffix = false;
        private ALittleScriptOp4SuffixElement m_cache_Op4Suffix = null;
        public ALittleScriptOp4SuffixElement GetOp4Suffix()
        {
            if (m_flag_Op4Suffix) return m_cache_Op4Suffix;
            m_flag_Op4Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp4SuffixElement)
                {
                    m_cache_Op4Suffix = child as ALittleScriptOp4SuffixElement;
                    break;
                }
            }
            return m_cache_Op4Suffix;
        }
        private bool m_flag_Op5Suffix = false;
        private ALittleScriptOp5SuffixElement m_cache_Op5Suffix = null;
        public ALittleScriptOp5SuffixElement GetOp5Suffix()
        {
            if (m_flag_Op5Suffix) return m_cache_Op5Suffix;
            m_flag_Op5Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp5SuffixElement)
                {
                    m_cache_Op5Suffix = child as ALittleScriptOp5SuffixElement;
                    break;
                }
            }
            return m_cache_Op5Suffix;
        }
        private bool m_flag_Op6Suffix = false;
        private ALittleScriptOp6SuffixElement m_cache_Op6Suffix = null;
        public ALittleScriptOp6SuffixElement GetOp6Suffix()
        {
            if (m_flag_Op6Suffix) return m_cache_Op6Suffix;
            m_flag_Op6Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp6SuffixElement)
                {
                    m_cache_Op6Suffix = child as ALittleScriptOp6SuffixElement;
                    break;
                }
            }
            return m_cache_Op6Suffix;
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