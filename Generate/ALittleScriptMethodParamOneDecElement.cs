
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptMethodParamOneDecElement : ABnfNodeElement
	{
		public ALittleScriptMethodParamOneDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MethodParamTailDec = false;
        private ALittleScriptMethodParamTailDecElement m_cache_MethodParamTailDec = null;
        public ALittleScriptMethodParamTailDecElement GetMethodParamTailDec()
        {
            if (m_flag_MethodParamTailDec) return m_cache_MethodParamTailDec;
            m_flag_MethodParamTailDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodParamTailDecElement)
                {
                    m_cache_MethodParamTailDec = child as ALittleScriptMethodParamTailDecElement;
                    break;
                }
            }
            return m_cache_MethodParamTailDec;
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
        private bool m_flag_MethodParamNameDec = false;
        private ALittleScriptMethodParamNameDecElement m_cache_MethodParamNameDec = null;
        public ALittleScriptMethodParamNameDecElement GetMethodParamNameDec()
        {
            if (m_flag_MethodParamNameDec) return m_cache_MethodParamNameDec;
            m_flag_MethodParamNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodParamNameDecElement)
                {
                    m_cache_MethodParamNameDec = child as ALittleScriptMethodParamNameDecElement;
                    break;
                }
            }
            return m_cache_MethodParamNameDec;
        }

	}
}