
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptElseIfExprElement : ABnfNodeElement
	{
		public ALittleScriptElseIfExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ElseIfCondition = false;
        private ALittleScriptElseIfConditionElement m_cache_ElseIfCondition = null;
        public ALittleScriptElseIfConditionElement GetElseIfCondition()
        {
            if (m_flag_ElseIfCondition) return m_cache_ElseIfCondition;
            m_flag_ElseIfCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptElseIfConditionElement)
                {
                    m_cache_ElseIfCondition = child as ALittleScriptElseIfConditionElement;
                    break;
                }
            }
            return m_cache_ElseIfCondition;
        }
        private bool m_flag_ElseIfBody = false;
        private ALittleScriptElseIfBodyElement m_cache_ElseIfBody = null;
        public ALittleScriptElseIfBodyElement GetElseIfBody()
        {
            if (m_flag_ElseIfBody) return m_cache_ElseIfBody;
            m_flag_ElseIfBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptElseIfBodyElement)
                {
                    m_cache_ElseIfBody = child as ALittleScriptElseIfBodyElement;
                    break;
                }
            }
            return m_cache_ElseIfBody;
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