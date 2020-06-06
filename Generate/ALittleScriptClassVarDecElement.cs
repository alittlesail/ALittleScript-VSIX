
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassVarDecElement : ABnfNodeElement
	{
		public ALittleScriptClassVarDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_AllType = false;
        private ALittleScriptAllTypeElement m_cache_AllType = null;
        public ALittleScriptAllTypeElement GetAllType()
        {
            if (m_flag_AllType) return m_cache_AllType;
            m_flag_AllType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAllTypeElement)
                {
                    m_cache_AllType = child as ALittleScriptAllTypeElement;
                    break;
                }
            }
            return m_cache_AllType;
        }
        private bool m_flag_ClassVarNameDec = false;
        private ALittleScriptClassVarNameDecElement m_cache_ClassVarNameDec = null;
        public ALittleScriptClassVarNameDecElement GetClassVarNameDec()
        {
            if (m_flag_ClassVarNameDec) return m_cache_ClassVarNameDec;
            m_flag_ClassVarNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassVarNameDecElement)
                {
                    m_cache_ClassVarNameDec = child as ALittleScriptClassVarNameDecElement;
                    break;
                }
            }
            return m_cache_ClassVarNameDec;
        }
        private bool m_flag_ClassVarValueDec = false;
        private ALittleScriptClassVarValueDecElement m_cache_ClassVarValueDec = null;
        public ALittleScriptClassVarValueDecElement GetClassVarValueDec()
        {
            if (m_flag_ClassVarValueDec) return m_cache_ClassVarValueDec;
            m_flag_ClassVarValueDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassVarValueDecElement)
                {
                    m_cache_ClassVarValueDec = child as ALittleScriptClassVarValueDecElement;
                    break;
                }
            }
            return m_cache_ClassVarValueDec;
        }
        private bool m_flag_String = false;
        private ALittleScriptStringElement m_cache_String = null;
        public ALittleScriptStringElement GetString()
        {
            if (m_flag_String) return m_cache_String;
            m_flag_String = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStringElement)
                {
                    m_cache_String = child as ALittleScriptStringElement;
                    break;
                }
            }
            return m_cache_String;
        }

	}
}