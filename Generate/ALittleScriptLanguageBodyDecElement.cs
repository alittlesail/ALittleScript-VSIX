
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptLanguageBodyDecElement : ABnfNodeElement
	{
		public ALittleScriptLanguageBodyDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptLanguageNameDecElement> m_list_LanguageNameDec = null;
        public List<ALittleScriptLanguageNameDecElement> GetLanguageNameDecList()
        {
            var list = new List<ALittleScriptLanguageNameDecElement>();
            if (m_list_LanguageNameDec == null)
            {
                m_list_LanguageNameDec = new List<ALittleScriptLanguageNameDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptLanguageNameDecElement)
                        m_list_LanguageNameDec.Add(child as ALittleScriptLanguageNameDecElement);
                }   
            }
            list.AddRange(m_list_LanguageNameDec);
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