
using System;
using System.IO;
using System.Windows;

namespace ALittle
{
    public class ALittleScriptFile : ABnfFileInfo
    {
        public ALittleScriptFile(string full_path, ABnf abnf, string text) : base(full_path, abnf, text)
        {
        }

        //编译部分//////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool CompileDocument()
        {
            var project_info = GetProjectInfo();
            if (project_info == null)
            {
                MessageBox.Show("请将当前文件加入到工程后再进行编译");
                return true;
            }

            var generator = ALittleScriptTranslation.CreateTranslation();
            try
            {
                var error = generator.Generate(this, true);

                if (error != null)
                {
                    var message = error.GetError();
                    if (error.GetElement() != null)
                    {
                        message += ", file:" + error.GetElement().GetFullPath();
                        message += ", line:" + (error.GetElement().GetStartLine() + 1);
                    }
                    MessageBox.Show(message);
                    return true;
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
                return true;
            }

            MessageBox.Show("生成完毕");
            return true;
        }

        public override bool CompileProject()
        {
            var project_info = GetProjectInfo();
            if (project_info == null)
            {
                MessageBox.Show("请将当前文件加入到工程后再进行编译");
                return true;
            }

            var target_path = ALittleScriptUtility.CalcRootFullPath(project_info.GetProjectPath(), "lua");
            if (GeneralOptions.Instance.TargetLanguage == TargetLanguages.JavaScript)
                target_path = ALittleScriptUtility.CalcRootFullPath(project_info.GetProjectPath(), "js");

            try
            {
                ALittleScriptUtility.DeleteDirectory(new DirectoryInfo(target_path));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return true;
            }

            var all_file = project_info.GetAllFile();
            try
            {
                foreach (var pair in all_file)
                {
                    var generator = ALittleScriptTranslation.CreateTranslation();
                    var error = generator.Generate(pair.Value.GetFile(), true);
                    if (error == null) continue;

                    var result = MessageBox.Show(pair.Value.GetFullPath() + "\n" + error.GetError() + "\n是否打开错误文件?", "生成失败", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        int start = 0;
                        int length = 0;
                        if (error.GetElement() != null)
						{
                            start = error.GetElement().GetStart();
                            length = error.GetElement().GetLength();
                            if (length <= 0) length = 1;
						}
                        string full_path = pair.Value.GetFullPath();

                        try
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ALanguageUtility.OpenFile(null, ALittleScriptVsTextViewCreationListener.s_adapters_factory, full_path, start, length);
                            });
                        }
                        catch (System.Exception)
                        {

                        }
                    }
                    return true;
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
                return true;
            }

            MessageBox.Show("生成完毕");
            return true;
        }

        public override void OnSave()
        {
            var generator = ALittleScriptTranslation.CreateTranslation();
            try
            {
                generator.Generate(this, true);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //格式化部分//////////////////////////////////////////////////////////////////////////////////////////////////
        public override string FormatDocument()
        {
            if (HasError())
            {
                // 提示错误
                MessageBox.Show("当前有语法错误，请修正后再格式化");
                return null;
            }

            return null;
        }

        // 更新分析内容
        public override void UpdateAnalysis()
        {
            m_root = null;
            if (m_abnf == null) return;

            // var time_1 = DateTime.Now;
            m_root = m_abnf.Analysis(this);
            // var time_2 = DateTime.Now;
            // System.Diagnostics.Debug.WriteLine(m_full_path + ": UpdateAnalysis " + (time_2 - time_1).TotalMilliseconds + "毫秒");
        }

        public override void UpdateError()
        {
            CollectError(m_root);
            // var time_1 = DateTime.Now;
            if (m_root != null) AnalysisError(m_root);
            // var time_2 = DateTime.Now;
            // System.Diagnostics.Debug.WriteLine(m_full_path + ": UpdateError " + (time_2 - time_1).TotalMilliseconds + "毫秒");
        }
    }
}
