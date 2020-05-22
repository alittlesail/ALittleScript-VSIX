
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp4StatElement : ABnfNodeElement
	{
		public ALittleScriptOp4StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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
        List<ALittleScriptOp4SuffixExElement> m_list_Op4SuffixEx = null;
        public List<ALittleScriptOp4SuffixExElement> GetOp4SuffixExList()
        {
            var list = new List<ALittleScriptOp4SuffixExElement>();
            if (m_list_Op4SuffixEx == null)
            {
                m_list_Op4SuffixEx = new List<ALittleScriptOp4SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp4SuffixExElement)
                        m_list_Op4SuffixEx.Add(child as ALittleScriptOp4SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op4SuffixEx);
            return list;
        }

	}
}