
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptNamespaceElementDecElement : ABnfNodeElement
	{
		public ALittleScriptNamespaceElementDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_GlobalMethodDec = false;
        private ALittleScriptGlobalMethodDecElement m_cache_GlobalMethodDec = null;
        public ALittleScriptGlobalMethodDecElement GetGlobalMethodDec()
        {
            if (m_flag_GlobalMethodDec) return m_cache_GlobalMethodDec;
            m_flag_GlobalMethodDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGlobalMethodDecElement)
                {
                    m_cache_GlobalMethodDec = child as ALittleScriptGlobalMethodDecElement;
                    break;
                }
            }
            return m_cache_GlobalMethodDec;
        }
        private bool m_flag_ClassDec = false;
        private ALittleScriptClassDecElement m_cache_ClassDec = null;
        public ALittleScriptClassDecElement GetClassDec()
        {
            if (m_flag_ClassDec) return m_cache_ClassDec;
            m_flag_ClassDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassDecElement)
                {
                    m_cache_ClassDec = child as ALittleScriptClassDecElement;
                    break;
                }
            }
            return m_cache_ClassDec;
        }
        private bool m_flag_EnumDec = false;
        private ALittleScriptEnumDecElement m_cache_EnumDec = null;
        public ALittleScriptEnumDecElement GetEnumDec()
        {
            if (m_flag_EnumDec) return m_cache_EnumDec;
            m_flag_EnumDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptEnumDecElement)
                {
                    m_cache_EnumDec = child as ALittleScriptEnumDecElement;
                    break;
                }
            }
            return m_cache_EnumDec;
        }
        private bool m_flag_StructDec = false;
        private ALittleScriptStructDecElement m_cache_StructDec = null;
        public ALittleScriptStructDecElement GetStructDec()
        {
            if (m_flag_StructDec) return m_cache_StructDec;
            m_flag_StructDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructDecElement)
                {
                    m_cache_StructDec = child as ALittleScriptStructDecElement;
                    break;
                }
            }
            return m_cache_StructDec;
        }
        private bool m_flag_UsingDec = false;
        private ALittleScriptUsingDecElement m_cache_UsingDec = null;
        public ALittleScriptUsingDecElement GetUsingDec()
        {
            if (m_flag_UsingDec) return m_cache_UsingDec;
            m_flag_UsingDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptUsingDecElement)
                {
                    m_cache_UsingDec = child as ALittleScriptUsingDecElement;
                    break;
                }
            }
            return m_cache_UsingDec;
        }
        private bool m_flag_InstanceDec = false;
        private ALittleScriptInstanceDecElement m_cache_InstanceDec = null;
        public ALittleScriptInstanceDecElement GetInstanceDec()
        {
            if (m_flag_InstanceDec) return m_cache_InstanceDec;
            m_flag_InstanceDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptInstanceDecElement)
                {
                    m_cache_InstanceDec = child as ALittleScriptInstanceDecElement;
                    break;
                }
            }
            return m_cache_InstanceDec;
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