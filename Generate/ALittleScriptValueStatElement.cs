
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptValueStatElement : ABnfNodeElement
	{
		public ALittleScriptValueStatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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
        private bool m_flag_OpNewListStat = false;
        private ALittleScriptOpNewListStatElement m_cache_OpNewListStat = null;
        public ALittleScriptOpNewListStatElement GetOpNewListStat()
        {
            if (m_flag_OpNewListStat) return m_cache_OpNewListStat;
            m_flag_OpNewListStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOpNewListStatElement)
                {
                    m_cache_OpNewListStat = child as ALittleScriptOpNewListStatElement;
                    break;
                }
            }
            return m_cache_OpNewListStat;
        }
        private bool m_flag_BindStat = false;
        private ALittleScriptBindStatElement m_cache_BindStat = null;
        public ALittleScriptBindStatElement GetBindStat()
        {
            if (m_flag_BindStat) return m_cache_BindStat;
            m_flag_BindStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptBindStatElement)
                {
                    m_cache_BindStat = child as ALittleScriptBindStatElement;
                    break;
                }
            }
            return m_cache_BindStat;
        }
        private bool m_flag_TcallStat = false;
        private ALittleScriptTcallStatElement m_cache_TcallStat = null;
        public ALittleScriptTcallStatElement GetTcallStat()
        {
            if (m_flag_TcallStat) return m_cache_TcallStat;
            m_flag_TcallStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTcallStatElement)
                {
                    m_cache_TcallStat = child as ALittleScriptTcallStatElement;
                    break;
                }
            }
            return m_cache_TcallStat;
        }
        private bool m_flag_Op2Stat = false;
        private ALittleScriptOp2StatElement m_cache_Op2Stat = null;
        public ALittleScriptOp2StatElement GetOp2Stat()
        {
            if (m_flag_Op2Stat) return m_cache_Op2Stat;
            m_flag_Op2Stat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp2StatElement)
                {
                    m_cache_Op2Stat = child as ALittleScriptOp2StatElement;
                    break;
                }
            }
            return m_cache_Op2Stat;
        }
        private bool m_flag_ValueOpStat = false;
        private ALittleScriptValueOpStatElement m_cache_ValueOpStat = null;
        public ALittleScriptValueOpStatElement GetValueOpStat()
        {
            if (m_flag_ValueOpStat) return m_cache_ValueOpStat;
            m_flag_ValueOpStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptValueOpStatElement)
                {
                    m_cache_ValueOpStat = child as ALittleScriptValueOpStatElement;
                    break;
                }
            }
            return m_cache_ValueOpStat;
        }

	}
}