
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassElementDecElement : ABnfNodeElement
	{
		public ALittleScriptClassElementDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_ClassCtorDec = false;
        private ALittleScriptClassCtorDecElement m_cache_ClassCtorDec = null;
        public ALittleScriptClassCtorDecElement GetClassCtorDec()
        {
            if (m_flag_ClassCtorDec) return m_cache_ClassCtorDec;
            m_flag_ClassCtorDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassCtorDecElement)
                {
                    m_cache_ClassCtorDec = child as ALittleScriptClassCtorDecElement;
                    break;
                }
            }
            return m_cache_ClassCtorDec;
        }
        private bool m_flag_ClassGetterDec = false;
        private ALittleScriptClassGetterDecElement m_cache_ClassGetterDec = null;
        public ALittleScriptClassGetterDecElement GetClassGetterDec()
        {
            if (m_flag_ClassGetterDec) return m_cache_ClassGetterDec;
            m_flag_ClassGetterDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassGetterDecElement)
                {
                    m_cache_ClassGetterDec = child as ALittleScriptClassGetterDecElement;
                    break;
                }
            }
            return m_cache_ClassGetterDec;
        }
        private bool m_flag_ClassSetterDec = false;
        private ALittleScriptClassSetterDecElement m_cache_ClassSetterDec = null;
        public ALittleScriptClassSetterDecElement GetClassSetterDec()
        {
            if (m_flag_ClassSetterDec) return m_cache_ClassSetterDec;
            m_flag_ClassSetterDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassSetterDecElement)
                {
                    m_cache_ClassSetterDec = child as ALittleScriptClassSetterDecElement;
                    break;
                }
            }
            return m_cache_ClassSetterDec;
        }
        private bool m_flag_ClassStaticDec = false;
        private ALittleScriptClassStaticDecElement m_cache_ClassStaticDec = null;
        public ALittleScriptClassStaticDecElement GetClassStaticDec()
        {
            if (m_flag_ClassStaticDec) return m_cache_ClassStaticDec;
            m_flag_ClassStaticDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassStaticDecElement)
                {
                    m_cache_ClassStaticDec = child as ALittleScriptClassStaticDecElement;
                    break;
                }
            }
            return m_cache_ClassStaticDec;
        }
        private bool m_flag_ClassMethodDec = false;
        private ALittleScriptClassMethodDecElement m_cache_ClassMethodDec = null;
        public ALittleScriptClassMethodDecElement GetClassMethodDec()
        {
            if (m_flag_ClassMethodDec) return m_cache_ClassMethodDec;
            m_flag_ClassMethodDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassMethodDecElement)
                {
                    m_cache_ClassMethodDec = child as ALittleScriptClassMethodDecElement;
                    break;
                }
            }
            return m_cache_ClassMethodDec;
        }
        private bool m_flag_ClassVarDec = false;
        private ALittleScriptClassVarDecElement m_cache_ClassVarDec = null;
        public ALittleScriptClassVarDecElement GetClassVarDec()
        {
            if (m_flag_ClassVarDec) return m_cache_ClassVarDec;
            m_flag_ClassVarDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassVarDecElement)
                {
                    m_cache_ClassVarDec = child as ALittleScriptClassVarDecElement;
                    break;
                }
            }
            return m_cache_ClassVarDec;
        }

	}
}