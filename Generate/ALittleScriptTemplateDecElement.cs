
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptTemplateDecElement : ABnfNodeElement
	{
		public ALittleScriptTemplateDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptTemplatePairDecElement> m_list_TemplatePairDec = null;
        public List<ALittleScriptTemplatePairDecElement> GetTemplatePairDecList()
        {
            var list = new List<ALittleScriptTemplatePairDecElement>();
            if (m_list_TemplatePairDec == null)
            {
                m_list_TemplatePairDec = new List<ALittleScriptTemplatePairDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptTemplatePairDecElement)
                        m_list_TemplatePairDec.Add(child as ALittleScriptTemplatePairDecElement);
                }   
            }
            list.AddRange(m_list_TemplatePairDec);
            return list;
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