
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassVarValueDecElement : ABnfNodeElement
	{
		public ALittleScriptClassVarValueDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ConstValue = false;
        private ALittleScriptConstValueElement m_cache_ConstValue = null;
        public ALittleScriptConstValueElement GetConstValue()
        {
            if (m_flag_ConstValue) return m_cache_ConstValue;
            m_flag_ConstValue = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptConstValueElement)
                {
                    m_cache_ConstValue = child as ALittleScriptConstValueElement;
                    break;
                }
            }
            return m_cache_ConstValue;
        }
        private bool m_flag_OpNewStat = false;
        private ALittleScriptOpNewStatElement m_cache_OpNewStat = null;
        public ALittleScriptOpNewStatElement GetOpNewStat()
        {
            if (m_flag_OpNewStat) return m_cache_OpNewStat;
            m_flag_OpNewStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOpNewStatElement)
                {
                    m_cache_OpNewStat = child as ALittleScriptOpNewStatElement;
                    break;
                }
            }
            return m_cache_OpNewStat;
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