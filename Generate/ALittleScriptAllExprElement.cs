
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptAllExprElement : ABnfNodeElement
	{
		public ALittleScriptAllExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptModifierElement> m_list_Modifier = null;
        public List<ALittleScriptModifierElement> GetModifierList()
        {
            var list = new List<ALittleScriptModifierElement>();
            if (m_list_Modifier == null)
            {
                m_list_Modifier = new List<ALittleScriptModifierElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptModifierElement)
                        m_list_Modifier.Add(child as ALittleScriptModifierElement);
                }   
            }
            list.AddRange(m_list_Modifier);
            return list;
        }
        private bool m_flag_IfExpr = false;
        private ALittleScriptIfExprElement m_cache_IfExpr = null;
        public ALittleScriptIfExprElement GetIfExpr()
        {
            if (m_flag_IfExpr) return m_cache_IfExpr;
            m_flag_IfExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptIfExprElement)
                {
                    m_cache_IfExpr = child as ALittleScriptIfExprElement;
                    break;
                }
            }
            return m_cache_IfExpr;
        }
        private bool m_flag_ForExpr = false;
        private ALittleScriptForExprElement m_cache_ForExpr = null;
        public ALittleScriptForExprElement GetForExpr()
        {
            if (m_flag_ForExpr) return m_cache_ForExpr;
            m_flag_ForExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForExprElement)
                {
                    m_cache_ForExpr = child as ALittleScriptForExprElement;
                    break;
                }
            }
            return m_cache_ForExpr;
        }
        private bool m_flag_WhileExpr = false;
        private ALittleScriptWhileExprElement m_cache_WhileExpr = null;
        public ALittleScriptWhileExprElement GetWhileExpr()
        {
            if (m_flag_WhileExpr) return m_cache_WhileExpr;
            m_flag_WhileExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptWhileExprElement)
                {
                    m_cache_WhileExpr = child as ALittleScriptWhileExprElement;
                    break;
                }
            }
            return m_cache_WhileExpr;
        }
        private bool m_flag_DoWhileExpr = false;
        private ALittleScriptDoWhileExprElement m_cache_DoWhileExpr = null;
        public ALittleScriptDoWhileExprElement GetDoWhileExpr()
        {
            if (m_flag_DoWhileExpr) return m_cache_DoWhileExpr;
            m_flag_DoWhileExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptDoWhileExprElement)
                {
                    m_cache_DoWhileExpr = child as ALittleScriptDoWhileExprElement;
                    break;
                }
            }
            return m_cache_DoWhileExpr;
        }
        private bool m_flag_ReturnExpr = false;
        private ALittleScriptReturnExprElement m_cache_ReturnExpr = null;
        public ALittleScriptReturnExprElement GetReturnExpr()
        {
            if (m_flag_ReturnExpr) return m_cache_ReturnExpr;
            m_flag_ReturnExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptReturnExprElement)
                {
                    m_cache_ReturnExpr = child as ALittleScriptReturnExprElement;
                    break;
                }
            }
            return m_cache_ReturnExpr;
        }
        private bool m_flag_FlowExpr = false;
        private ALittleScriptFlowExprElement m_cache_FlowExpr = null;
        public ALittleScriptFlowExprElement GetFlowExpr()
        {
            if (m_flag_FlowExpr) return m_cache_FlowExpr;
            m_flag_FlowExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptFlowExprElement)
                {
                    m_cache_FlowExpr = child as ALittleScriptFlowExprElement;
                    break;
                }
            }
            return m_cache_FlowExpr;
        }
        private bool m_flag_ThrowExpr = false;
        private ALittleScriptThrowExprElement m_cache_ThrowExpr = null;
        public ALittleScriptThrowExprElement GetThrowExpr()
        {
            if (m_flag_ThrowExpr) return m_cache_ThrowExpr;
            m_flag_ThrowExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptThrowExprElement)
                {
                    m_cache_ThrowExpr = child as ALittleScriptThrowExprElement;
                    break;
                }
            }
            return m_cache_ThrowExpr;
        }
        private bool m_flag_AssertExpr = false;
        private ALittleScriptAssertExprElement m_cache_AssertExpr = null;
        public ALittleScriptAssertExprElement GetAssertExpr()
        {
            if (m_flag_AssertExpr) return m_cache_AssertExpr;
            m_flag_AssertExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAssertExprElement)
                {
                    m_cache_AssertExpr = child as ALittleScriptAssertExprElement;
                    break;
                }
            }
            return m_cache_AssertExpr;
        }
        private bool m_flag_WrapExpr = false;
        private ALittleScriptWrapExprElement m_cache_WrapExpr = null;
        public ALittleScriptWrapExprElement GetWrapExpr()
        {
            if (m_flag_WrapExpr) return m_cache_WrapExpr;
            m_flag_WrapExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptWrapExprElement)
                {
                    m_cache_WrapExpr = child as ALittleScriptWrapExprElement;
                    break;
                }
            }
            return m_cache_WrapExpr;
        }
        private bool m_flag_Op1Expr = false;
        private ALittleScriptOp1ExprElement m_cache_Op1Expr = null;
        public ALittleScriptOp1ExprElement GetOp1Expr()
        {
            if (m_flag_Op1Expr) return m_cache_Op1Expr;
            m_flag_Op1Expr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp1ExprElement)
                {
                    m_cache_Op1Expr = child as ALittleScriptOp1ExprElement;
                    break;
                }
            }
            return m_cache_Op1Expr;
        }
        private bool m_flag_EmptyExpr = false;
        private ALittleScriptEmptyExprElement m_cache_EmptyExpr = null;
        public ALittleScriptEmptyExprElement GetEmptyExpr()
        {
            if (m_flag_EmptyExpr) return m_cache_EmptyExpr;
            m_flag_EmptyExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptEmptyExprElement)
                {
                    m_cache_EmptyExpr = child as ALittleScriptEmptyExprElement;
                    break;
                }
            }
            return m_cache_EmptyExpr;
        }
        private bool m_flag_VarAssignExpr = false;
        private ALittleScriptVarAssignExprElement m_cache_VarAssignExpr = null;
        public ALittleScriptVarAssignExprElement GetVarAssignExpr()
        {
            if (m_flag_VarAssignExpr) return m_cache_VarAssignExpr;
            m_flag_VarAssignExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptVarAssignExprElement)
                {
                    m_cache_VarAssignExpr = child as ALittleScriptVarAssignExprElement;
                    break;
                }
            }
            return m_cache_VarAssignExpr;
        }
        private bool m_flag_OpAssignExpr = false;
        private ALittleScriptOpAssignExprElement m_cache_OpAssignExpr = null;
        public ALittleScriptOpAssignExprElement GetOpAssignExpr()
        {
            if (m_flag_OpAssignExpr) return m_cache_OpAssignExpr;
            m_flag_OpAssignExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOpAssignExprElement)
                {
                    m_cache_OpAssignExpr = child as ALittleScriptOpAssignExprElement;
                    break;
                }
            }
            return m_cache_OpAssignExpr;
        }

	}
}