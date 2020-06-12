
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;
using System.IO;

namespace ALittle
{
    public class ALittleScriptPathsValueReference : ALittleScriptReferenceTemplate<ALittleScriptPathsValueElement>
    {
        public ALittleScriptPathsValueReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            guess_list.Add(new ALittleScriptGuessList(ALittleScriptIndex.inst.sStringGuess, false, false));
            return null;
        }

		public override ABnfGuessError CheckError()
		{
            var text = m_element.GetText();
            if (text == null) return new ABnfGuessError(m_element, "请填写路径来获取子文件夹以及文件的路径");

            // 检查路径是否存在
            var path = m_element.GetProjectPath() + text.GetElementString().Trim();
            var info = new DirectoryInfo(path);
            if (!info.Exists)
                return new ABnfGuessError(m_element, "填写的路径不存在:" + path);
            return null;
        }
	}
}

