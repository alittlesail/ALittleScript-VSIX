
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptVarAssignDecElement : ABnfNodeElement
	{
		public ALittleScriptVarAssignDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_VarAssignNameDec = false;
        private ALittleScriptVarAssignNameDecElement m_cache_VarAssignNameDec = null;
        public ALittleScriptVarAssignNameDecElement GetVarAssignNameDec()
        {
            if (m_flag_VarAssignNameDec) return m_cache_VarAssignNameDec;
            m_flag_VarAssignNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptVarAssignNameDecElement)
                {
                    m_cache_VarAssignNameDec = child as ALittleScriptVarAssignNameDecElement;
                    break;
                }
            }
            return m_cache_VarAssignNameDec;
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