using Microsoft.VisualStudio.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ALittle
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        public HashSet<string> TargetLanguageNameSet = new HashSet<string>();
        public string m_target_lang_string = "";

        public GeneralOptions()
        {
            foreach (var name in System.Enum.GetNames(typeof(TargetLanguages)))
                TargetLanguageNameSet.Add(name);
        }

        [Category("定制功能")]
        [DisplayName("目标语言")]
        [Description("选择你需要生成的目标语言")]
        [DefaultValue(TargetLanguages.Lua)]
        [TypeConverter(typeof(EnumConverter))]
        public TargetLanguages TargetLanguage { get; set; }
        public string GetTargetLanguageString() { return m_target_lang_string; }

        protected override void LoadProperty(SettingsStore store)
        {
            var value = store.GetString(CollectionName, nameof(TargetLanguage), "Lua");
            if (value == "Lua") TargetLanguage = TargetLanguages.Lua;
            else if (value == "JavaScript") TargetLanguage = TargetLanguages.JavaScript;
            else TargetLanguage = TargetLanguages.Lua;

            m_target_lang_string = value;

            // 纠正
            if (TargetLanguage == TargetLanguages.Lua)
                m_target_lang_string = "Lua";
        }
        protected override void SaveProperty(WritableSettingsStore store)
        {
            if (TargetLanguage == TargetLanguages.Lua)
                m_target_lang_string = "Lua";
            else if (TargetLanguage == TargetLanguages.JavaScript)
                m_target_lang_string = "JavaScript";
            else
                m_target_lang_string = "Lua";

            store.SetString(CollectionName, nameof(TargetLanguage), m_target_lang_string);
        }
    }

    public enum TargetLanguages
    {
        Lua,
        JavaScript
    }
}
