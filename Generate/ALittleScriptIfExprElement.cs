
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptIfExprElement : ABnfNodeElement
	{
		public ALittleScriptIfExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_IfCondition = false;
        private ALittleScriptIfConditionElement m_cache_IfCondition = null;
        public ALittleScriptIfConditionElement GetIfCondition()
        {
            if (m_flag_IfCondition) return m_cache_IfCondition;
            m_flag_IfCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptIfConditionElement)
                {
                    m_cache_IfCondition = child as ALittleScriptIfConditionElement;
                    break;
                }
            }
            return m_cache_IfCondition;
        }
        private bool m_flag_IfBody = false;
        private ALittleScriptIfBodyElement m_cache_IfBody = null;
        public ALittleScriptIfBodyElement GetIfBody()
        {
            if (m_flag_IfBody) return m_cache_IfBody;
            m_flag_IfBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptIfBodyElement)
                {
                    m_cache_IfBody = child as ALittleScriptIfBodyElement;
                    break;
                }
            }
            return m_cache_IfBody;
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
        List<ALittleScriptElseIfExprElement> m_list_ElseIfExpr = null;
        public List<ALittleScriptElseIfExprElement> GetElseIfExprList()
        {
            var list = new List<ALittleScriptElseIfExprElement>();
            if (m_list_ElseIfExpr == null)
            {
                m_list_ElseIfExpr = new List<ALittleScriptElseIfExprElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptElseIfExprElement)
                        m_list_ElseIfExpr.Add(child as ALittleScriptElseIfExprElement);
                }   
            }
            list.AddRange(m_list_ElseIfExpr);
            return list;
        }
        private bool m_flag_ElseExpr = false;
        private ALittleScriptElseExprElement m_cache_ElseExpr = null;
        public ALittleScriptElseExprElement GetElseExpr()
        {
            if (m_flag_ElseExpr) return m_cache_ElseExpr;
            m_flag_ElseExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptElseExprElement)
                {
                    m_cache_ElseExpr = child as ALittleScriptElseExprElement;
                    break;
                }
            }
            return m_cache_ElseExpr;
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