
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptWhileExprElement : ABnfNodeElement
	{
		public ALittleScriptWhileExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_WhileCondition = false;
        private ALittleScriptWhileConditionElement m_cache_WhileCondition = null;
        public ALittleScriptWhileConditionElement GetWhileCondition()
        {
            if (m_flag_WhileCondition) return m_cache_WhileCondition;
            m_flag_WhileCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptWhileConditionElement)
                {
                    m_cache_WhileCondition = child as ALittleScriptWhileConditionElement;
                    break;
                }
            }
            return m_cache_WhileCondition;
        }
        private bool m_flag_WhileBody = false;
        private ALittleScriptWhileBodyElement m_cache_WhileBody = null;
        public ALittleScriptWhileBodyElement GetWhileBody()
        {
            if (m_flag_WhileBody) return m_cache_WhileBody;
            m_flag_WhileBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptWhileBodyElement)
                {
                    m_cache_WhileBody = child as ALittleScriptWhileBodyElement;
                    break;
                }
            }
            return m_cache_WhileBody;
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