
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.IO;

namespace ALittle
{
    public class StructReflectInfo
    {
        public string content = "";        // 最终的结果
        public bool generate = false;         // 是否生成

        public string name = "";           // 带命名域的结构体名
        public string ns_name = "";        // 命名域
        public string rl_name = "";        // 不再命名域的结构体名
        public int hash_code = 0;          // 哈希值
        public List<string> name_list = new List<string>();    // 成员名列表
        public List<string> type_list = new List<string>();    // 类型名列表
        public Dictionary<string, string> option_map = new Dictionary<string, string>();    // 附加信息列表
    }

    public class ALittleScriptTranslation
    {
        // 当前文件命名域
        protected string m_namespace_name = "";
        // 当前模块所在工程路径
        protected string m_project_path = "";
        // 当前文件路径
        protected string m_file_path = "";

        // 定义依赖
        private HashSet<string> m_define_rely = new HashSet<string>();
        // 运行依赖
        private HashSet<string> m_run_rely = new HashSet<string>();
        // 当前是否是定义依赖
        protected bool m_is_define_relay = false;

        public static ALittleScriptTranslation CreateTranslation()
        {
            if (GeneralOptions.Instance.TargetLanguage == TargetLanguages.Lua)
                return new ALittleScriptTranslationLua();
            else if (GeneralOptions.Instance.TargetLanguage == TargetLanguages.JavaScript)
                return new ALittleScriptTranslationJavaScript();
            return new ALittleScriptTranslation();
        }

        protected void AddRelay(ABnfElement element)
        {
            if (element == null) return;

            var full_path = element.GetFullPath();
            if (full_path == m_file_path) return;

            if (ALittleScriptUtility.IsRegister(element)) return;

            if (m_is_define_relay)
            {
                if (!m_define_rely.Contains(full_path))
                    m_define_rely.Add(full_path);
                return;
            }

            if (!m_run_rely.Contains(full_path))
                m_run_rely.Add(full_path);
        }

        // 检查语法错误
        protected ABnfGuessError CheckErrorElement(ABnfElement element, bool full_check)
        {
            if (element is ABnfErrorElement)
            {
                var error_element = element as ABnfErrorElement;
                return new ABnfGuessError(element, error_element.GetValue());
            }

            // 判断语义错误
            if (full_check)
            {
                var error = element.GetReference().CheckError();
                if (error != null) return error;
            }

            if (!(element is ABnfNodeElement node)) return null;

            foreach (var child in node.GetChilds())
            {
                var error = CheckErrorElement(child, full_check);
                if (error != null) return error;
            }

            return null;
        }

        public virtual ABnfGuessError Generate(ABnfFile file, bool full_check)
        {
            // 判断是否在工程中
            var project_info = file.GetProjectInfo();
            if (project_info == null) return new ABnfGuessError(null, "文件没有添加到工程中");

            // 解析失败
            var root_dec = file.GetRoot() as ALittleScriptRootElement;
            if (root_dec == null) return new ABnfGuessError(null, "文件还未解析");
            var namespace_dec = root_dec.GetNamespaceDec();
            if (namespace_dec == null) return new ABnfGuessError(null, "命名域没有定义名字");

            var name_dec = namespace_dec.GetNamespaceNameDec();
            if (name_dec == null) return new ABnfGuessError(null, "命名域没有定义名字");

            m_namespace_name = name_dec.GetElementText();

            m_project_path = file.GetRoot().GetProjectPath();
            m_file_path = file.GetRoot().GetFullPath();

            // 如果命名域有register标记，那么就不需要生成
            if (ALittleScriptUtility.IsRegister(namespace_dec.GetModifierList())) return null;
            if (!ALittleScriptUtility.IsLanguageEnable(namespace_dec.GetModifierList())) return null;

            // 获取语法错误
            var error = CheckErrorElement(file.GetRoot(), full_check);
            if (error != null) return error;

            // 获取工作目录
            string full_path = ALittleScriptUtility.CalcTargetFullPath(project_info.GetProjectPath(), file.GetFullPath(), GetExt(), out string path_error);
            if (full_path == null) return new ABnfGuessError(null, path_error);
            string full_dir = Path.GetDirectoryName(full_path);
            try
            {
                Directory.CreateDirectory(full_dir);
            }
            catch (Exception e)
            {
                return new ABnfGuessError(null, e.Message);
            }

            // 生成代码
            error = GenerateRoot(namespace_dec.GetNamespaceElementDecList(), out string content);
            if (error != null) return error;

            File.WriteAllText(full_path, content, System.Text.Encoding.UTF8);
            return null;
        }

        protected virtual ABnfGuessError GenerateRoot(List<ALittleScriptNamespaceElementDecElement> element_list, out string content)
        {
            content = "";
            return new ABnfGuessError(null, "未实现生成代码");
        }

        protected virtual string GetExt()
        {
            return "";
        }

        public static int StructReflectSort(StructReflectInfo a, StructReflectInfo b)
        {
            return a.hash_code - b.hash_code;
        }
    }
}

