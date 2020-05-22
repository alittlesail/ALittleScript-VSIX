
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptModifierElement : ABnfNodeElement
	{
		public ALittleScriptModifierElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_AttributeModifier = false;
        private ALittleScriptAttributeModifierElement m_cache_AttributeModifier = null;
        public ALittleScriptAttributeModifierElement GetAttributeModifier()
        {
            if (m_flag_AttributeModifier) return m_cache_AttributeModifier;
            m_flag_AttributeModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAttributeModifierElement)
                {
                    m_cache_AttributeModifier = child as ALittleScriptAttributeModifierElement;
                    break;
                }
            }
            return m_cache_AttributeModifier;
        }
        private bool m_flag_AccessModifier = false;
        private ALittleScriptAccessModifierElement m_cache_AccessModifier = null;
        public ALittleScriptAccessModifierElement GetAccessModifier()
        {
            if (m_flag_AccessModifier) return m_cache_AccessModifier;
            m_flag_AccessModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAccessModifierElement)
                {
                    m_cache_AccessModifier = child as ALittleScriptAccessModifierElement;
                    break;
                }
            }
            return m_cache_AccessModifier;
        }
        private bool m_flag_CoroutineModifier = false;
        private ALittleScriptCoroutineModifierElement m_cache_CoroutineModifier = null;
        public ALittleScriptCoroutineModifierElement GetCoroutineModifier()
        {
            if (m_flag_CoroutineModifier) return m_cache_CoroutineModifier;
            m_flag_CoroutineModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCoroutineModifierElement)
                {
                    m_cache_CoroutineModifier = child as ALittleScriptCoroutineModifierElement;
                    break;
                }
            }
            return m_cache_CoroutineModifier;
        }
        private bool m_flag_RegisterModifier = false;
        private ALittleScriptRegisterModifierElement m_cache_RegisterModifier = null;
        public ALittleScriptRegisterModifierElement GetRegisterModifier()
        {
            if (m_flag_RegisterModifier) return m_cache_RegisterModifier;
            m_flag_RegisterModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptRegisterModifierElement)
                {
                    m_cache_RegisterModifier = child as ALittleScriptRegisterModifierElement;
                    break;
                }
            }
            return m_cache_RegisterModifier;
        }

	}
}