
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ALittle
{
    public class ALittleScriptFactory : ABnfFactory
    {   
        Dictionary<string, Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement>> m_create_map = new Dictionary<string, Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement>>();

        public ALittleScriptFactory()
        {
            m_create_map["Root"] = (factory, file, line, col, offset, type) => { return new ALittleScriptRootElement(factory, file, line, col, offset, type); };
            m_create_map["LineComment"] = (factory, file, line, col, offset, type) => { return new ALittleScriptLineCommentElement(factory, file, line, col, offset, type); };
            m_create_map["BlockComment"] = (factory, file, line, col, offset, type) => { return new ALittleScriptBlockCommentElement(factory, file, line, col, offset, type); };
            m_create_map["Text"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTextElement(factory, file, line, col, offset, type); };
            m_create_map["Id"] = (factory, file, line, col, offset, type) => { return new ALittleScriptIdElement(factory, file, line, col, offset, type); };
            m_create_map["Number"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNumberElement(factory, file, line, col, offset, type); };
            m_create_map["RegisterModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptRegisterModifierElement(factory, file, line, col, offset, type); };
            m_create_map["AccessModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAccessModifierElement(factory, file, line, col, offset, type); };
            m_create_map["CoroutineModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCoroutineModifierElement(factory, file, line, col, offset, type); };
            m_create_map["ProtocolModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptProtocolModifierElement(factory, file, line, col, offset, type); };
            m_create_map["CommandBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCommandBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["CommandModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCommandModifierElement(factory, file, line, col, offset, type); };
            m_create_map["NullableModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNullableModifierElement(factory, file, line, col, offset, type); };
            m_create_map["LanguageNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptLanguageNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["LanguageBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptLanguageBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["LanguageModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptLanguageModifierElement(factory, file, line, col, offset, type); };
            m_create_map["ConstModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptConstModifierElement(factory, file, line, col, offset, type); };
            m_create_map["NativeModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNativeModifierElement(factory, file, line, col, offset, type); };
            m_create_map["AttributeModifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAttributeModifierElement(factory, file, line, col, offset, type); };
            m_create_map["Modifier"] = (factory, file, line, col, offset, type) => { return new ALittleScriptModifierElement(factory, file, line, col, offset, type); };
            m_create_map["NamespaceDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNamespaceDecElement(factory, file, line, col, offset, type); };
            m_create_map["NamespaceElementDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNamespaceElementDecElement(factory, file, line, col, offset, type); };
            m_create_map["NamespaceNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptNamespaceNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplateDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplateDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplatePairDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplatePairDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplateNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplateNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplateExtendsDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplateExtendsDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplateExtendsClassDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplateExtendsClassDecElement(factory, file, line, col, offset, type); };
            m_create_map["TemplateExtendsStructDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTemplateExtendsStructDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassExtendsDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassExtendsDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassElementDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassElementDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassVarDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassVarDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassVarNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassVarNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructExtendsDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructExtendsDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructVarDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructVarDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructVarNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructVarNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructOptionDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructOptionDecElement(factory, file, line, col, offset, type); };
            m_create_map["StructOptionNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptStructOptionNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["EnumDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEnumDecElement(factory, file, line, col, offset, type); };
            m_create_map["EnumNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEnumNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["EnumBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEnumBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["EnumVarDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEnumVarDecElement(factory, file, line, col, offset, type); };
            m_create_map["EnumVarNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEnumVarNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["InstanceDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptInstanceDecElement(factory, file, line, col, offset, type); };
            m_create_map["UsingDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptUsingDecElement(factory, file, line, col, offset, type); };
            m_create_map["UsingNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptUsingNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodParamTailDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodParamTailDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodParamOneDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodParamOneDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodParamNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodParamNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodParamDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodParamDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodGetterParamDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodGetterParamDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodSetterParamDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodSetterParamDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodBodyDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodBodyDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodReturnTailDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodReturnTailDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodReturnOneDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodReturnOneDecElement(factory, file, line, col, offset, type); };
            m_create_map["MethodReturnDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptMethodReturnDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassCtorDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassCtorDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassGetterDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassGetterDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassSetterDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassSetterDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassMethodDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassMethodDecElement(factory, file, line, col, offset, type); };
            m_create_map["ClassStaticDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptClassStaticDecElement(factory, file, line, col, offset, type); };
            m_create_map["GlobalMethodDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGlobalMethodDecElement(factory, file, line, col, offset, type); };
            m_create_map["AllExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAllExprElement(factory, file, line, col, offset, type); };
            m_create_map["EmptyExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptEmptyExprElement(factory, file, line, col, offset, type); };
            m_create_map["ForExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForExprElement(factory, file, line, col, offset, type); };
            m_create_map["ForCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForConditionElement(factory, file, line, col, offset, type); };
            m_create_map["ForBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForBodyElement(factory, file, line, col, offset, type); };
            m_create_map["ForStepCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForStepConditionElement(factory, file, line, col, offset, type); };
            m_create_map["ForStartStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForStartStatElement(factory, file, line, col, offset, type); };
            m_create_map["ForEndStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForEndStatElement(factory, file, line, col, offset, type); };
            m_create_map["ForStepStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForStepStatElement(factory, file, line, col, offset, type); };
            m_create_map["ForInCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForInConditionElement(factory, file, line, col, offset, type); };
            m_create_map["ForPairDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptForPairDecElement(factory, file, line, col, offset, type); };
            m_create_map["WhileExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptWhileExprElement(factory, file, line, col, offset, type); };
            m_create_map["WhileCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptWhileConditionElement(factory, file, line, col, offset, type); };
            m_create_map["WhileBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptWhileBodyElement(factory, file, line, col, offset, type); };
            m_create_map["DoWhileExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptDoWhileExprElement(factory, file, line, col, offset, type); };
            m_create_map["DoWhileCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptDoWhileConditionElement(factory, file, line, col, offset, type); };
            m_create_map["DoWhileBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptDoWhileBodyElement(factory, file, line, col, offset, type); };
            m_create_map["IfExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptIfExprElement(factory, file, line, col, offset, type); };
            m_create_map["IfCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptIfConditionElement(factory, file, line, col, offset, type); };
            m_create_map["IfBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptIfBodyElement(factory, file, line, col, offset, type); };
            m_create_map["ElseExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptElseExprElement(factory, file, line, col, offset, type); };
            m_create_map["ElseBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptElseBodyElement(factory, file, line, col, offset, type); };
            m_create_map["ElseIfExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptElseIfExprElement(factory, file, line, col, offset, type); };
            m_create_map["ElseIfCondition"] = (factory, file, line, col, offset, type) => { return new ALittleScriptElseIfConditionElement(factory, file, line, col, offset, type); };
            m_create_map["ElseIfBody"] = (factory, file, line, col, offset, type) => { return new ALittleScriptElseIfBodyElement(factory, file, line, col, offset, type); };
            m_create_map["WrapExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptWrapExprElement(factory, file, line, col, offset, type); };
            m_create_map["ReturnExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptReturnExprElement(factory, file, line, col, offset, type); };
            m_create_map["ReturnYield"] = (factory, file, line, col, offset, type) => { return new ALittleScriptReturnYieldElement(factory, file, line, col, offset, type); };
            m_create_map["FlowExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptFlowExprElement(factory, file, line, col, offset, type); };
            m_create_map["VarAssignExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptVarAssignExprElement(factory, file, line, col, offset, type); };
            m_create_map["VarAssignDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptVarAssignDecElement(factory, file, line, col, offset, type); };
            m_create_map["VarAssignNameDec"] = (factory, file, line, col, offset, type) => { return new ALittleScriptVarAssignNameDecElement(factory, file, line, col, offset, type); };
            m_create_map["OpAssign"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOpAssignElement(factory, file, line, col, offset, type); };
            m_create_map["OpAssignExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOpAssignExprElement(factory, file, line, col, offset, type); };
            m_create_map["Op1Expr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp1ExprElement(factory, file, line, col, offset, type); };
            m_create_map["ThrowExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptThrowExprElement(factory, file, line, col, offset, type); };
            m_create_map["AssertExpr"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAssertExprElement(factory, file, line, col, offset, type); };
            m_create_map["AllType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAllTypeElement(factory, file, line, col, offset, type); };
            m_create_map["AllTypeConst"] = (factory, file, line, col, offset, type) => { return new ALittleScriptAllTypeConstElement(factory, file, line, col, offset, type); };
            m_create_map["CustomType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCustomTypeElement(factory, file, line, col, offset, type); };
            m_create_map["CustomTypeName"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCustomTypeNameElement(factory, file, line, col, offset, type); };
            m_create_map["CustomTypeDotId"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCustomTypeDotIdElement(factory, file, line, col, offset, type); };
            m_create_map["CustomTypeDotIdName"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCustomTypeDotIdNameElement(factory, file, line, col, offset, type); };
            m_create_map["CustomTypeTemplate"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCustomTypeTemplateElement(factory, file, line, col, offset, type); };
            m_create_map["GenericType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericMapType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericMapTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericListType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericListTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorParamOneType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorParamOneTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorParamTail"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorParamTailElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorParamType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorParamTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorReturnTail"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorReturnTailElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorReturnOneType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorReturnOneTypeElement(factory, file, line, col, offset, type); };
            m_create_map["GenericFunctorReturnType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptGenericFunctorReturnTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PrimitiveType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPrimitiveTypeElement(factory, file, line, col, offset, type); };
            m_create_map["ValueStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptValueStatElement(factory, file, line, col, offset, type); };
            m_create_map["ValueFactorStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptValueFactorStatElement(factory, file, line, col, offset, type); };
            m_create_map["ValueOpStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptValueOpStatElement(factory, file, line, col, offset, type); };
            m_create_map["OpNewStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOpNewStatElement(factory, file, line, col, offset, type); };
            m_create_map["OpNewListStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOpNewListStatElement(factory, file, line, col, offset, type); };
            m_create_map["BindStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptBindStatElement(factory, file, line, col, offset, type); };
            m_create_map["TcallStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptTcallStatElement(factory, file, line, col, offset, type); };
            m_create_map["WrapValueStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptWrapValueStatElement(factory, file, line, col, offset, type); };
            m_create_map["ConstValue"] = (factory, file, line, col, offset, type) => { return new ALittleScriptConstValueElement(factory, file, line, col, offset, type); };
            m_create_map["CoroutineStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptCoroutineStatElement(factory, file, line, col, offset, type); };
            m_create_map["ReflectValue"] = (factory, file, line, col, offset, type) => { return new ALittleScriptReflectValueElement(factory, file, line, col, offset, type); };
            m_create_map["ReflectCustomType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptReflectCustomTypeElement(factory, file, line, col, offset, type); };
            m_create_map["ReflectValueStat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptReflectValueStatElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValue"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueFirstType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueFirstTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueCastType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueCastTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueCustomType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueCustomTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueThisType"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueThisTypeElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueSuffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueSuffixElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueDotId"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueDotIdElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueDotIdName"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueDotIdNameElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueBracketValue"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueBracketValueElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueMethodCall"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueMethodCallElement(factory, file, line, col, offset, type); };
            m_create_map["PropertyValueMethodTemplate"] = (factory, file, line, col, offset, type) => { return new ALittleScriptPropertyValueMethodTemplateElement(factory, file, line, col, offset, type); };
            m_create_map["Op8"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp8Element(factory, file, line, col, offset, type); };
            m_create_map["Op8Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp8StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op8Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp8SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op8SuffixEe"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp8SuffixEeElement(factory, file, line, col, offset, type); };
            m_create_map["Op8SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp8SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op7"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp7Element(factory, file, line, col, offset, type); };
            m_create_map["Op7Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp7StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op7Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp7SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op7SuffixEe"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp7SuffixEeElement(factory, file, line, col, offset, type); };
            m_create_map["Op7SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp7SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op6"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp6Element(factory, file, line, col, offset, type); };
            m_create_map["Op6Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp6StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op6Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp6SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op6SuffixEe"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp6SuffixEeElement(factory, file, line, col, offset, type); };
            m_create_map["Op6SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp6SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op5"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp5Element(factory, file, line, col, offset, type); };
            m_create_map["Op5Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp5StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op5Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp5SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op5SuffixEe"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp5SuffixEeElement(factory, file, line, col, offset, type); };
            m_create_map["Op5SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp5SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op4"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp4Element(factory, file, line, col, offset, type); };
            m_create_map["Op4Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp4StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op4Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp4SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op4SuffixEe"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp4SuffixEeElement(factory, file, line, col, offset, type); };
            m_create_map["Op4SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp4SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op3"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp3Element(factory, file, line, col, offset, type); };
            m_create_map["Op3Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp3StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op3Suffix"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp3SuffixElement(factory, file, line, col, offset, type); };
            m_create_map["Op3SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp3SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op2"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp2Element(factory, file, line, col, offset, type); };
            m_create_map["Op2Stat"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp2StatElement(factory, file, line, col, offset, type); };
            m_create_map["Op2Value"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp2ValueElement(factory, file, line, col, offset, type); };
            m_create_map["Op2SuffixEx"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp2SuffixExElement(factory, file, line, col, offset, type); };
            m_create_map["Op1"] = (factory, file, line, col, offset, type) => { return new ALittleScriptOp1Element(factory, file, line, col, offset, type); };

        }

        public override ABnfNodeElement CreateNodeElement(ABnfFile file, int line, int col, int offset, string type)
        {
            Func<ABnfFactory, ABnfFile, int, int, int, string, ABnfNodeElement> func;
            if (!m_create_map.TryGetValue(type, out func)) return null;
            return func(this, file, line, col, offset, type);
        }

        public override ABnfKeyElement CreateKeyElement(ABnfFile file, int line, int col, int offset, string type)
        {
            return new ALittleScriptKeyElement(this, file, line, col, offset, type);
        }

        public override ABnfStringElement CreateStringElement(ABnfFile file, int line, int col, int offset, string type)
        {
            return new ALittleScriptStringElement(this, file, line, col, offset, type);
        }

        public override ABnfRegexElement CreateRegexElement(ABnfFile file, int line, int col, int offset, string type, Regex regex)
        {
            return new ALittleScriptRegexElement(this, file, line, col, offset, type, regex);
        }
    }
}

