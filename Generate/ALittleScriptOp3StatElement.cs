
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp3StatElement : ABnfNodeElement
	{
		public ALittleScriptOp3StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        List<ALittleScriptOp3SuffixExElement> m_list_Op3SuffixEx = null;
        public List<ALittleScriptOp3SuffixExElement> GetOp3SuffixExList()
        {
            var list = new List<ALittleScriptOp3SuffixExElement>();
            if (m_list_Op3SuffixEx == null)
            {
                m_list_Op3SuffixEx = new List<ALittleScriptOp3SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp3SuffixExElement)
                        m_list_Op3SuffixEx.Add(child as ALittleScriptOp3SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op3SuffixEx);
            return list;
        }

	}
}