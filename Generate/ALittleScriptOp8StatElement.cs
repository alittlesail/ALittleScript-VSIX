
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp8StatElement : ABnfNodeElement
	{
		public ALittleScriptOp8StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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
        List<ALittleScriptOp8SuffixExElement> m_list_Op8SuffixEx = null;
        public List<ALittleScriptOp8SuffixExElement> GetOp8SuffixExList()
        {
            var list = new List<ALittleScriptOp8SuffixExElement>();
            if (m_list_Op8SuffixEx == null)
            {
                m_list_Op8SuffixEx = new List<ALittleScriptOp8SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp8SuffixExElement)
                        m_list_Op8SuffixEx.Add(child as ALittleScriptOp8SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op8SuffixEx);
            return list;
        }

	}
}