
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptForStepConditionElement : ABnfNodeElement
	{
		public ALittleScriptForStepConditionElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ForStartStat = false;
        private ALittleScriptForStartStatElement m_cache_ForStartStat = null;
        public ALittleScriptForStartStatElement GetForStartStat()
        {
            if (m_flag_ForStartStat) return m_cache_ForStartStat;
            m_flag_ForStartStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForStartStatElement)
                {
                    m_cache_ForStartStat = child as ALittleScriptForStartStatElement;
                    break;
                }
            }
            return m_cache_ForStartStat;
        }
        private bool m_flag_ForEndStat = false;
        private ALittleScriptForEndStatElement m_cache_ForEndStat = null;
        public ALittleScriptForEndStatElement GetForEndStat()
        {
            if (m_flag_ForEndStat) return m_cache_ForEndStat;
            m_flag_ForEndStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForEndStatElement)
                {
                    m_cache_ForEndStat = child as ALittleScriptForEndStatElement;
                    break;
                }
            }
            return m_cache_ForEndStat;
        }
        private bool m_flag_ForStepStat = false;
        private ALittleScriptForStepStatElement m_cache_ForStepStat = null;
        public ALittleScriptForStepStatElement GetForStepStat()
        {
            if (m_flag_ForStepStat) return m_cache_ForStepStat;
            m_flag_ForStepStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForStepStatElement)
                {
                    m_cache_ForStepStat = child as ALittleScriptForStepStatElement;
                    break;
                }
            }
            return m_cache_ForStepStat;
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