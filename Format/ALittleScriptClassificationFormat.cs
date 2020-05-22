
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ALittle
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptDefault")]
    [Name("ALittleScriptDefault")]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptDefaultClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptDefaultClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript默认";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 180;
                color.G = 180;
                color.B = 180;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Black;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptDefaultBlur")]
    [Name("ALittleScriptDefaultBlur")]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptDefaultBlurClassificationFormatDefinition : ALittleScriptDefaultClassificationFormatDefinition
    {
        public ALittleScriptDefaultBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript默认(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptKeyWord")]
    [Name("ALittleScriptKeyWord")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptKeyWordClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptKeyWordClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript关键字";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 86;
                color.G = 154;
                color.B = 214;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Blue;
            }       
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptKeyWordBlur")]
    [Name("ALittleScriptKeyWordBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptKeyWordBlurClassificationFormatDefinition : ALittleScriptKeyWordClassificationFormatDefinition
    {
        public ALittleScriptKeyWordBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript关键字(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptCtrlKeyWord")]
    [Name("ALittleScriptCtrlKeyWord")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCtrlKeyWordClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptCtrlKeyWordClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript控制关键字";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 216;
                color.G = 160;
                color.B = 223;
                ForegroundColor = color;
            }
            else
            {
                var color = new Color();
                color.A = 255;
                color.R = 143;
                color.G = 8;
                color.B = 196;
                ForegroundColor = color;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptCtrlKeyWordBlur")]
    [Name("ALittleScriptCtrlKeyWordBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCtrlKeyWordBlurClassificationFormatDefinition : ALittleScriptCtrlKeyWordClassificationFormatDefinition
    {
        public ALittleScriptCtrlKeyWordBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript控制关键字(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptDefineName")]
    [Name("ALittleScriptDefineName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptDefineNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptDefineNameClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript定义名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 78;
                color.G = 201;
                color.B = 176;
                ForegroundColor = color;
            }
            else
            {
                var color = new Color();
                color.A = 255;
                color.R = 43;
                color.G = 145;
                color.B = 175;
                ForegroundColor = color;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptDefineNameBlur")]
    [Name("ALittleScriptDefineNameBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptDefineNameBlurClassificationFormatDefinition : ALittleScriptDefineNameClassificationFormatDefinition
    {
        public ALittleScriptDefineNameBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript定义名(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptCustomName")]
    [Name("ALittleScriptCustomName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCustomNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptCustomNameClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript类型名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 255;
                color.G = 215;
                color.B = 0;
                ForegroundColor = color;
            }
            else
            {
                var color = new Color();
                color.A = 255;
                color.R = 33;
                color.G = 111;
                color.B = 133;
                ForegroundColor = color;
            }
        } 
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptCustomNameBlur")]
    [Name("ALittleScriptCustomNameBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCustomNameBlurClassificationFormatDefinition : ALittleScriptCustomNameClassificationFormatDefinition
    {
        public ALittleScriptCustomNameBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript类型名(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptVarName")]
    [Name("ALittleScriptVarName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptVarNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptVarNameClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript变量名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 189;
                color.G = 183;
                color.B = 107;
                ForegroundColor = color;
            }
            else
                ForegroundColor = Colors.Navy;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptVarNameBlur")]
    [Name("ALittleScriptVarNameBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptVarNameBlurClassificationFormatDefinition : ALittleScriptVarNameClassificationFormatDefinition
    {
        public ALittleScriptVarNameBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript变量名";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptComment")]
    [Name("ALittleScriptComment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCommentClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptCommentClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript注释"; 
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 87;
                color.G = 166;
                color.B = 74;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.Green;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptCommentBlur")]
    [Name("ALittleScriptCommentBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptCommentBlurClassificationFormatDefinition : ALittleScriptCommentClassificationFormatDefinition
    {
        public ALittleScriptCommentBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript注释(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptMethodName")]
    [Name("ALittleScriptMethodName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptMethodNameClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptMethodNameClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript方法名";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 255;
                color.G = 128;
                color.B = 0;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.DarkRed;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptMethodNameBlur")]
    [Name("ALittleScriptMethodNameBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptMethodNameBlurClassificationFormatDefinition : ALittleScriptMethodNameClassificationFormatDefinition
    {
        public ALittleScriptMethodNameBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript方法名(虚化)";
            ForegroundOpacity = 0.5;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptText")]
    [Name("ALittleScriptText")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptTextClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptTextClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript字符串";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 214;
                color.G = 157; 
                color.B = 113;
                ForegroundColor = color;
            }
            else
            {
                ForegroundColor = Colors.DarkRed;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptTextBlur")]
    [Name("ALittleScriptTextBlur")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal class ALittleScriptTextBlurClassificationFormatDefinition : ALittleScriptTextClassificationFormatDefinition
    {
        public ALittleScriptTextBlurClassificationFormatDefinition()
        {
            DisplayName = "ALittleScript字符串(虚化)";
            ForegroundOpacity = 0.5;
        }
    }
}
