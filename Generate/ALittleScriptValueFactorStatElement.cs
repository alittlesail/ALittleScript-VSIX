
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptValueFactorStatElement : ABnfNodeElement
	{
		public ALittleScriptValueFactorStatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_WrapValueStat = false;
        private ALittleScriptWrapValueStatElement m_cache_WrapValueStat = null;
        public ALittleScriptWrapValueStatElement GetWrapValueStat()
        {
            if (m_flag_WrapValueStat) return m_cache_WrapValueStat;
            m_flag_WrapValueStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptWrapValueStatElement)
                {
                    m_cache_WrapValueStat = child as ALittleScriptWrapValueStatElement;
                    break;
                }
            }
            return m_cache_WrapValueStat;
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
        private bool m_flag_ReflectValue = false;
        private ALittleScriptReflectValueElement m_cache_ReflectValue = null;
        public ALittleScriptReflectValueElement GetReflectValue()
        {
            if (m_flag_ReflectValue) return m_cache_ReflectValue;
            m_flag_ReflectValue = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptReflectValueElement)
                {
                    m_cache_ReflectValue = child as ALittleScriptReflectValueElement;
                    break;
                }
            }
            return m_cache_ReflectValue;
        }
        private bool m_flag_PathsValue = false;
        private ALittleScriptPathsValueElement m_cache_PathsValue = null;
        public ALittleScriptPathsValueElement GetPathsValue()
        {
            if (m_flag_PathsValue) return m_cache_PathsValue;
            m_flag_PathsValue = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPathsValueElement)
                {
                    m_cache_PathsValue = child as ALittleScriptPathsValueElement;
                    break;
                }
            }
            return m_cache_PathsValue;
        }
        private bool m_flag_PropertyValue = false;
        private ALittleScriptPropertyValueElement m_cache_PropertyValue = null;
        public ALittleScriptPropertyValueElement GetPropertyValue()
        {
            if (m_flag_PropertyValue) return m_cache_PropertyValue;
            m_flag_PropertyValue = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueElement)
                {
                    m_cache_PropertyValue = child as ALittleScriptPropertyValueElement;
                    break;
                }
            }
            return m_cache_PropertyValue;
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
        private bool m_flag_CoroutineStat = false;
        private ALittleScriptCoroutineStatElement m_cache_CoroutineStat = null;
        public ALittleScriptCoroutineStatElement GetCoroutineStat()
        {
            if (m_flag_CoroutineStat) return m_cache_CoroutineStat;
            m_flag_CoroutineStat = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCoroutineStatElement)
                {
                    m_cache_CoroutineStat = child as ALittleScriptCoroutineStatElement;
                    break;
                }
            }
            return m_cache_CoroutineStat;
        }

	}
}