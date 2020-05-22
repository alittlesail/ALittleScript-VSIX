
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ALittle
{
    internal static class ALittleScriptClassificationTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptDefault")]
        internal static ClassificationTypeDefinition DEFAULT = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptDefaultBlur")]
        internal static ClassificationTypeDefinition DEFAULT_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptKeyWord")]
        internal static ClassificationTypeDefinition KEYWORD = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptKeyWordBlur")]
        internal static ClassificationTypeDefinition KEYWORD_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptCtrlKeyWord")]
        internal static ClassificationTypeDefinition CTRLKEYWORD = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptCtrlKeyWordBlur")]
        internal static ClassificationTypeDefinition CTRLKEYWORD_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptCustomName")]
        internal static ClassificationTypeDefinition CUSTOMNAME = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptCustomNameBlur")]
        internal static ClassificationTypeDefinition CUSTOMNAME_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptDefineName")]
        internal static ClassificationTypeDefinition DEFINENAME = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptDefineNameBlur")]
        internal static ClassificationTypeDefinition DEFINENAME_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptVarName")]
        internal static ClassificationTypeDefinition VARNAME = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptVarNameBlur")]
        internal static ClassificationTypeDefinition VARNAME_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptComment")]
        internal static ClassificationTypeDefinition COMMENT = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptCommentBlur")]
        internal static ClassificationTypeDefinition COMMENT_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptMethodName")]
        internal static ClassificationTypeDefinition METHODNAME = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptMethodNameBlur")]
        internal static ClassificationTypeDefinition METHODNAME_BLUR = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptText")]
        internal static ClassificationTypeDefinition TEXT = null;
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ALittleScriptTextBlur")]
        internal static ClassificationTypeDefinition TEXT_BLUR = null;
    }
}
