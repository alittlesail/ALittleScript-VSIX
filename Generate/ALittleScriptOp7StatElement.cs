
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp7StatElement : ABnfNodeElement
	{
		public ALittleScriptOp7StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        List<ALittleScriptOp7SuffixExElement> m_list_Op7SuffixEx = null;
        public List<ALittleScriptOp7SuffixExElement> GetOp7SuffixExList()
        {
            var list = new List<ALittleScriptOp7SuffixExElement>();
            if (m_list_Op7SuffixEx == null)
            {
                m_list_Op7SuffixEx = new List<ALittleScriptOp7SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp7SuffixExElement)
                        m_list_Op7SuffixEx.Add(child as ALittleScriptOp7SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op7SuffixEx);
            return list;
        }

	}
}