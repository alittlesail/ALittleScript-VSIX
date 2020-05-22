
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptForExprElement : ABnfNodeElement
	{
		public ALittleScriptForExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ForCondition = false;
        private ALittleScriptForConditionElement m_cache_ForCondition = null;
        public ALittleScriptForConditionElement GetForCondition()
        {
            if (m_flag_ForCondition) return m_cache_ForCondition;
            m_flag_ForCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForConditionElement)
                {
                    m_cache_ForCondition = child as ALittleScriptForConditionElement;
                    break;
                }
            }
            return m_cache_ForCondition;
        }
        private bool m_flag_ForBody = false;
        private ALittleScriptForBodyElement m_cache_ForBody = null;
        public ALittleScriptForBodyElement GetForBody()
        {
            if (m_flag_ForBody) return m_cache_ForBody;
            m_flag_ForBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForBodyElement)
                {
                    m_cache_ForBody = child as ALittleScriptForBodyElement;
                    break;
                }
            }
            return m_cache_ForBody;
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