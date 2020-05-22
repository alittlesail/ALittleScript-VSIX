
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptValueOpStatElement : ABnfNodeElement
	{
		public ALittleScriptValueOpStatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ValueFactorStat = false;
        private ALittleScriptValueFactorStatElement m_cache_ValueFactorStat = null;
        public ALittleScriptValueFactorStatElement GetValueFactorStat()
        {
            if (m_flag_ValueFactorStat) return m_cache_ValueFactorStat;
            m_flag_ValueFactorStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptValueFactorStatElement)
                {
                    m_cache_ValueFactorStat = child as ALittleScriptValueFactorStatElement;
                    break;
                }
            }
            return m_cache_ValueFactorStat;
        }
        private bool m_flag_Op3Stat = false;
        private ALittleScriptOp3StatElement m_cache_Op3Stat = null;
        public ALittleScriptOp3StatElement GetOp3Stat()
        {
            if (m_flag_Op3Stat) return m_cache_Op3Stat;
            m_flag_Op3Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp3StatElement)
                {
                    m_cache_Op3Stat = child as ALittleScriptOp3StatElement;
                    break;
                }
            }
            return m_cache_Op3Stat;
        }
        private bool m_flag_Op4Stat = false;
        private ALittleScriptOp4StatElement m_cache_Op4Stat = null;
        public ALittleScriptOp4StatElement GetOp4Stat()
        {
            if (m_flag_Op4Stat) return m_cache_Op4Stat;
            m_flag_Op4Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp4StatElement)
                {
                    m_cache_Op4Stat = child as ALittleScriptOp4StatElement;
                    break;
                }
            }
            return m_cache_Op4Stat;
        }
        private bool m_flag_Op5Stat = false;
        private ALittleScriptOp5StatElement m_cache_Op5Stat = null;
        public ALittleScriptOp5StatElement GetOp5Stat()
        {
            if (m_flag_Op5Stat) return m_cache_Op5Stat;
            m_flag_Op5Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp5StatElement)
                {
                    m_cache_Op5Stat = child as ALittleScriptOp5StatElement;
                    break;
                }
            }
            return m_cache_Op5Stat;
        }
        private bool m_flag_Op6Stat = false;
        private ALittleScriptOp6StatElement m_cache_Op6Stat = null;
        public ALittleScriptOp6StatElement GetOp6Stat()
        {
            if (m_flag_Op6Stat) return m_cache_Op6Stat;
            m_flag_Op6Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp6StatElement)
                {
                    m_cache_Op6Stat = child as ALittleScriptOp6StatElement;
                    break;
                }
            }
            return m_cache_Op6Stat;
        }
        private bool m_flag_Op7Stat = false;
        private ALittleScriptOp7StatElement m_cache_Op7Stat = null;
        public ALittleScriptOp7StatElement GetOp7Stat()
        {
            if (m_flag_Op7Stat) return m_cache_Op7Stat;
            m_flag_Op7Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp7StatElement)
                {
                    m_cache_Op7Stat = child as ALittleScriptOp7StatElement;
                    break;
                }
            }
            return m_cache_Op7Stat;
        }
        private bool m_flag_Op8Stat = false;
        private ALittleScriptOp8StatElement m_cache_Op8Stat = null;
        public ALittleScriptOp8StatElement GetOp8Stat()
        {
            if (m_flag_Op8Stat) return m_cache_Op8Stat;
            m_flag_Op8Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp8StatElement)
                {
                    m_cache_Op8Stat = child as ALittleScriptOp8StatElement;
                    break;
                }
            }
            return m_cache_Op8Stat;
        }

	}
}