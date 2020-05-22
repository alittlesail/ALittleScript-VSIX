
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptElseExprElement : ABnfNodeElement
	{
		public ALittleScriptElseExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ElseBody = false;
        private ALittleScriptElseBodyElement m_cache_ElseBody = null;
        public ALittleScriptElseBodyElement GetElseBody()
        {
            if (m_flag_ElseBody) return m_cache_ElseBody;
            m_flag_ElseBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptElseBodyElement)
                {
                    m_cache_ElseBody = child as ALittleScriptElseBodyElement;
                    break;
                }
            }
            return m_cache_ElseBody;
        }
        private bool m_flag_AllExpr = false;
        private ALittleScriptAllExprElement m_cache_AllExpr = null;
        public ALittleScriptAllExprElement GetAllExpr()
        {
            if (m_flag_AllExpr) return m_cache_AllExpr;
            m_flag_AllExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAllExprElement)
                {
                    m_cache_AllExpr = child as ALittleScriptAllExprElement;
                    break;
                }
            }
            return m_cache_AllExpr;
        }
        private bool m_flag_Key = false;
        private ALittleScriptKeyElement m_cache_Key = null;
        public ALittleScriptKeyElement GetKey()
        {
            if (m_flag_Key) return m_cache_Key;
            m_flag_Key = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptKeyElement)
                {
                    m_cache_Key = child as ALittleScriptKeyElement;
                    break;
                }
            }
            return m_cache_Key;
        }

	}
}