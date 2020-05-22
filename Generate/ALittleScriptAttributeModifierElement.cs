
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptAttributeModifierElement : ABnfNodeElement
	{
		public ALittleScriptAttributeModifierElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_NativeModifier = false;
        private ALittleScriptNativeModifierElement m_cache_NativeModifier = null;
        public ALittleScriptNativeModifierElement GetNativeModifier()
        {
            if (m_flag_NativeModifier) return m_cache_NativeModifier;
            m_flag_NativeModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNativeModifierElement)
                {
                    m_cache_NativeModifier = child as ALittleScriptNativeModifierElement;
                    break;
                }
            }
            return m_cache_NativeModifier;
        }
        private bool m_flag_LanguageModifier = false;
        private ALittleScriptLanguageModifierElement m_cache_LanguageModifier = null;
        public ALittleScriptLanguageModifierElement GetLanguageModifier()
        {
            if (m_flag_LanguageModifier) return m_cache_LanguageModifier;
            m_flag_LanguageModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptLanguageModifierElement)
                {
                    m_cache_LanguageModifier = child as ALittleScriptLanguageModifierElement;
                    break;
                }
            }
            return m_cache_LanguageModifier;
        }
        private bool m_flag_ConstModifier = false;
        private ALittleScriptConstModifierElement m_cache_ConstModifier = null;
        public ALittleScriptConstModifierElement GetConstModifier()
        {
            if (m_flag_ConstModifier) return m_cache_ConstModifier;
            m_flag_ConstModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptConstModifierElement)
                {
                    m_cache_ConstModifier = child as ALittleScriptConstModifierElement;
                    break;
                }
            }
            return m_cache_ConstModifier;
        }
        private bool m_flag_NullableModifier = false;
        private ALittleScriptNullableModifierElement m_cache_NullableModifier = null;
        public ALittleScriptNullableModifierElement GetNullableModifier()
        {
            if (m_flag_NullableModifier) return m_cache_NullableModifier;
            m_flag_NullableModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNullableModifierElement)
                {
                    m_cache_NullableModifier = child as ALittleScriptNullableModifierElement;
                    break;
                }
            }
            return m_cache_NullableModifier;
        }
        private bool m_flag_ProtocolModifier = false;
        private ALittleScriptProtocolModifierElement m_cache_ProtocolModifier = null;
        public ALittleScriptProtocolModifierElement GetProtocolModifier()
        {
            if (m_flag_ProtocolModifier) return m_cache_ProtocolModifier;
            m_flag_ProtocolModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptProtocolModifierElement)
                {
                    m_cache_ProtocolModifier = child as ALittleScriptProtocolModifierElement;
                    break;
                }
            }
            return m_cache_ProtocolModifier;
        }
        private bool m_flag_CommandModifier = false;
        private ALittleScriptCommandModifierElement m_cache_CommandModifier = null;
        public ALittleScriptCommandModifierElement GetCommandModifier()
        {
            if (m_flag_CommandModifier) return m_cache_CommandModifier;
            m_flag_CommandModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCommandModifierElement)
                {
                    m_cache_CommandModifier = child as ALittleScriptCommandModifierElement;
                    break;
                }
            }
            return m_cache_CommandModifier;
        }
        List<ALittleScriptStringElement> m_list_String = null;
        public List<ALittleScriptStringElement> GetStringList()
        {
            var list = new List<ALittleScriptStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<ALittleScriptStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStringElement)
                        m_list_String.Add(child as ALittleScriptStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}