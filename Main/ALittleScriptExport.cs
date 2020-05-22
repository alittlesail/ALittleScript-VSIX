
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Tagging;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Classification;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;

namespace ALittle
{
    // 文件绑定
    public class ALittleScriptContentTypeDefinition
    {
        [Export]
        [Name("alittle")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition ALittleScriptContentType = null;

        [Export]
        [FileExtension(".alittle")]
        [ContentType("alittle")]
        internal static FileExtensionToContentTypeDefinition ALittleScriptFileType = null;
    }

    // 编辑器管理
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("alittle")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public class ALittleScriptVsTextViewCreationListener : ALanguageVsTextViewCreationListener
    {
        public ALittleScriptVsTextViewCreationListener()
        {
            m_factory = ALittleScriptFactoryClass.inst;
        }
    }

    // 预测列表
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("alittle")]
    [Name("ALittleScriptCompletionSourceProvider")]
    public sealed class ALittleScriptCompletionSourceProvider : ALanguageCompletionSourceProvider { }

    // 函数调用参数预览
    [Export(typeof(ISignatureHelpSourceProvider))]
    [ContentType("alittle")]
    [Name("ALittleScriptSignatureHelpSourceProvider")]
    public sealed class ALittleScriptSignatureHelpSourceProvider : ALanguageSignatureHelpSourceProvider { }

    // 控制器
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("ALittleScriptControllerProvider")]
    [ContentType("alittle")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Analyzable)]
    public class ALittleScriptControllerProvider : ALanguageControllerProvider { }

    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType("alittle")]
    [Name("ALittleScriptQuickInfoSourceProvider")]
    public class ALittleScriptQuickInfoSourceProvider : ALanguageQuickInfoSourceProvider { }

    // 配色
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("alittle")]
    [TagType(typeof(ClassificationTag))]
    public class ALittleScriptClassifierProvider : ALanguageClassifierProvider { public ALittleScriptClassifierProvider() : base("ALittleScriptGotoDefinition") { } }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ALittleScriptGotoDefinition")]
    [Name("ALittleScriptGotoDefinition")]
    [Order(After = Priority.High)]
    public class ALittleScriptGotoDefinitionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public ALittleScriptGotoDefinitionClassificationFormatDefinition()
        {
            this.DisplayName = "ALittleScriptGotoDefinition";
            this.TextDecorations = System.Windows.TextDecorations.Underline;

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
                this.ForegroundColor = Colors.Blue;
            }
        }
    }

    public class ALanguageClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptGotoDefinition")]
        internal static ClassificationTypeDefinition GOTODEFINITION = null;
    }

    // 缩进
    [Export(typeof(ISmartIndentProvider))]
    [ContentType("alittle")]
    public class ALittleScriptSmartIndentProvider : ALanguageSmartIndentProvider { }

    // 错误元素提示
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(IErrorTag))]
    [ContentType("alittle")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Analyzable)]
    public class ALittleScriptErrorTaggerProvider : ALanguageErrorTaggerProvider { }

    // 引用个数提示
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("alittle")]
    [TagType(typeof(IntraTextAdornmentTag))]
    public class ALittleScriptReferenceTaggerProvider : IViewTaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
        {
            if (!view.Properties.TryGetProperty(nameof(ALanguageReferenceTagger), out ALanguageReferenceTagger tagger))
            {
                tagger = new ALanguageReferenceTagger(view);
                view.Properties.AddProperty(nameof(ALanguageReferenceTagger), tagger);
            }
            return tagger as ITagger<T>;
        }
    }

    // 高亮元素提示
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("alittle")]
    [TagType(typeof(TextMarkerTag))]
    public class ALittleScriptHighlightWordTaggerProvider : ALanguageHighlightWordTaggerProvider { }

    [Export(typeof(EditorFormatDefinition))]
    [Name("ALittleScriptHighlightWordFormatDefinition")]
    [UserVisible(true)]
    public class ALittleScriptHighlightWordFormatDefinition : MarkerFormatDefinition
    {
        public ALittleScriptHighlightWordFormatDefinition()
        {
            DisplayName = "ALittleScript高亮";
            if (ALanguageUtility.IsDarkTheme())
            {
                var color = new Color();
                color.A = 255;
                color.R = 14;
                color.G = 69;
                color.B = 131;
                BackgroundColor = color;

                color = new Color();
                color.A = 255;
                color.R = 173;
                color.G = 192;
                color.B = 211;
                ForegroundColor = color;
            }
            else
            {
                BackgroundColor = Colors.LightBlue;
            }
        }
    }

    public class ALittleScriptHighlightWordTag : TextMarkerTag
    {
        public ALittleScriptHighlightWordTag() : base("ALittleScriptHighlightWordFormatDefinition") { }
    }

    // 鼠标按键处理
    [Export(typeof(IMouseProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
    [ContentType("alittle")]
    [Name("ALittleScriptGoToDefinitionMouseHandlerProvider")]
    [Order(Before = "WordSelection")]
    public class ALittleScriptGoToDefinitionMouseHandlerProvider : ALanguageGoToDefinitionMouseHandlerProvider { }

    // 键盘按键处理
    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
    [ContentType("alittle")]
    [Name("ALittleScriptGoToDefinitionKeyProcessorProvider")]
    [Order(Before = "VisualStudioKeyboardProcessor")]
    public class ALittleScriptGoToDefinitionKeyProcessorProvider : ALanguageGoToDefinitionKeyProcessorProvider { }
}

