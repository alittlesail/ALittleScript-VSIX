
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptReflectValueElement : ABnfNodeElement
	{
		public ALittleScriptReflectValueElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ReflectCustomType = false;
        private ALittleScriptReflectCustomTypeElement m_cache_ReflectCustomType = null;
        public ALittleScriptReflectCustomTypeElement GetReflectCustomType()
        {
            if (m_flag_ReflectCustomType) return m_cache_ReflectCustomType;
            m_flag_ReflectCustomType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptReflectCustomTypeElement)
                {
                    m_cache_ReflectCustomType = child as ALittleScriptReflectCustomTypeElement;
                    break;
                }
            }
            return m_cache_ReflectCustomType;
        }
        private bool m_flag_ReflectValueStat = false;
        private ALittleScriptReflectValueStatElement m_cache_ReflectValueStat = null;
        public ALittleScriptReflectValueStatElement GetReflectValueStat()
        {
            if (m_flag_ReflectValueStat) return m_cache_ReflectValueStat;
            m_flag_ReflectValueStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptReflectValueStatElement)
                {
                    m_cache_ReflectValueStat = child as ALittleScriptReflectValueStatElement;
                    break;
                }
            }
            return m_cache_ReflectValueStat;
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