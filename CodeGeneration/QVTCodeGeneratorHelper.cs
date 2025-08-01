using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.RelationTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.Utils;

using TransformationMainTemplate = LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.TransformationTemplate.TransformationMainTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.FunctionsInterfaceTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration;
using SlnParser.Contracts;
using SlnParser;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.DescriptorTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.StarterTemplate;

namespace LL.MDE.Components.Qvt.QvtCodeGenerator.CodeGeneration
{
    public static class QVTCodeGeneratorHelper
    {
        private static string SolutionName { get; set; } = "MDD4All.QVT.Transformations-dev.sln";

        private static readonly IList<string> csharpKeywords = new List<string>()
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };

        public static string GenerateCode(IRelation relation, bool useMetamodelInterface = true)
        {
            RelationMainTemplate template = new RelationMainTemplate(relation, useMetamodelInterface);
            string code = template.TransformText();

            if (code.StartsWith("\r\n"))
            {
                code = code.Substring(2);
            }

            // We prefix members accessed with keywords by @
            //TODO find better fix: protected keywords could also be used as domain names, hence as method args in C#
            foreach (string csharpKeyword in csharpKeywords)
            {
                code = code.Replace("." + csharpKeyword, ".@" + csharpKeyword);
            }

            return code;
        }

        public static string GenerateCode(IRelationalTransformation transformation, bool useMetamodelInterface)
        {
            TransformationMainTemplate template = new TransformationMainTemplate(transformation, useMetamodelInterface);
            string code = template.TransformText();
            if (code.StartsWith("\r\n"))
            {
                code = code.Substring(2);
            }
            return code;
        }

        public static string GenerateStarterCode(IRelationalTransformation transformation, bool useMetamodelInterface)
        {
            StarterMainTemplate template = new StarterMainTemplate(transformation, useMetamodelInterface);
            string code = template.TransformText();

            return code;
        }

        public static void GenerateStarterCode(IRelationalTransformation transformation, string outputFolderAbsolute, bool useMetamodelInterface)
        {
            string code = GenerateStarterCode(transformation, useMetamodelInterface);
            try
            {
                //code = CodeFormatter.Format(code);
            }
            catch (Exception)
            {
                // TODO
            }
            if (!Directory.Exists(outputFolderAbsolute))
            {
                Directory.CreateDirectory(outputFolderAbsolute);
            }
            File.WriteAllText(PrepareOutputFolderString(outputFolderAbsolute) + "/TransformationStarter.cs", code);
        }

        public static string GenerateCodeFunctions(IRelationalTransformation transformation)
        {
            FunctionsInterfaceTemplate template = new FunctionsInterfaceTemplate(transformation);
            string code = template.TransformText();
            return code;
        }

        private static string PrepareOutputFolderString(string s)
        {
            if (s.Last() != '\\')
            {
                return s + '\\';
            }
            return s;
        }

        public static void GenerateCode(IRelationalTransformation transformation, string outputFolderAbsolute, bool useMetamodelInterface)
        {
            string code = GenerateCode(transformation, useMetamodelInterface);
            try
            {
                //code = CodeFormatter.Format(code);
            }
            catch (Exception)
            {
                // TODO
            }
            if (!Directory.Exists(outputFolderAbsolute))
            {
                Directory.CreateDirectory(outputFolderAbsolute);
            }
            File.WriteAllText(PrepareOutputFolderString(outputFolderAbsolute) + QvtCodeGeneratorStrings.GetFileName(transformation), code);
        }

        public static void GenerateCodeFunctions(IRelationalTransformation transformation, string outputFolderAbsolute)
        {
            string code = GenerateCodeFunctions(transformation);
            try
            {
                //code = CodeFormatter.Format(code);
            }
            catch (Exception)
            {
                // TODO
            }
            if (!Directory.Exists(outputFolderAbsolute))
            {
                Directory.CreateDirectory(outputFolderAbsolute);
            }
            File.WriteAllText(PrepareOutputFolderString(outputFolderAbsolute) + QvtCodeGeneratorStrings.GetFileNameFunctions(transformation), code);
        }

        public static void GenerateCode(IRelation relation, string outputFolderAbsolute, bool useMetamodelInterface = true)
        {
            string code = GenerateCode(relation, useMetamodelInterface);
            try
            {
                //code = CodeFormatter.Format(code);
            }
            catch (Exception)
            {
                // TODO
            }
            File.WriteAllText(PrepareOutputFolderString(outputFolderAbsolute) + QvtCodeGeneratorStrings.GetFileName(relation), code);
        }

        public static void GenerateAllCode(IRelationalTransformation transformation, string outputFolderAbsolute, bool useMetamodelInterface = true)
        {

            string transformationPath = GenerateSolutionStructure(outputFolderAbsolute, transformation);


            GenerateCode(transformation, transformationPath, useMetamodelInterface);
            if (transformation.OwnedOperation.Count > 0)
            {
                GenerateCodeFunctions(transformation, transformationPath);
            }
            foreach (IRelation relation in transformation.Rule.OfType<IRelation>())
            {
                GenerateCode(relation, transformationPath, useMetamodelInterface);
            }

            GenerateStarterCode(transformation, transformationPath, useMetamodelInterface);
        }

        private static string GenerateSolutionStructure(string path, IRelationalTransformation transformation)
        {
            string result = path;

            string transformationProjectName = QvtCodeGeneratorStrings.TransformationProjectName(transformation);
            string uiProjectName = QvtCodeGeneratorStrings.UserInterfaceProjectName(transformation);

            if (!Directory.Exists(path + "/" + transformationProjectName))
            {
                Directory.CreateDirectory(path + "/" + transformationProjectName);
            }

            if (!Directory.Exists(uiProjectName))
            {
                Directory.CreateDirectory(path + "/" + uiProjectName);
            }

            result = Path.Combine(path, transformationProjectName);

            // CreateOrModifySolution(path, transformation);

            return result;
        }

        private static void CreateOrModifySolution(string path, IRelationalTransformation transformation)
        {
            

            string solutionFilename = path + "/" + SolutionName;

            string transformationProjectName = QvtCodeGeneratorStrings.TransformationProjectName(transformation);
            string uiProjectName = QvtCodeGeneratorStrings.UserInterfaceProjectName(transformation);


            Solution solution;

            if (!File.Exists(solutionFilename))
            {
                solution = new Solution()
                {
                    FileFormatVersion = "12.00",
                    VisualStudioVersion = new VisualStudioVersion
                    {
                        Version = "17.13.35931.197 d17.13",
                        MinimumVersion = "10.0.40219.1"
                    }


                };

                ConfigurationPlatform debugConfig = new ConfigurationPlatform("Debug|Any CPU", "Debug", "Any CPU");
                solution.ConfigurationPlatforms.Add(debugConfig);

                ConfigurationPlatform releaseConfig = new ConfigurationPlatform("Release|Any CPU", "Release", "Any CPU");
                solution.ConfigurationPlatforms.Add(releaseConfig);

            }
            else
            {
                SolutionParser solutionParser = new SolutionParser();
                solution = solutionParser.Parse(solutionFilename) as Solution;
            }

            if (solution.AllProjects.Find(project => project.Name == transformationProjectName && project.TypeGuid.ToString() == "{2150E333-8FDC-42A3-9474-1A3956D46DE8}") == null)
            {
                Guid solutionFolderGuid = Guid.NewGuid();

                SolutionFolder solutionFolder = new SolutionFolder(solutionFolderGuid,
                                                                   transformationProjectName,
                                                                   new Guid("{2150E333-8FDC-42A3-9474-1A3956D46DE8}"),
                                                                   ProjectType.SolutionFolder);

                solution.AllProjects.Add(solutionFolder);




                Guid projectGUID = Guid.NewGuid();

                

                SolutionProject project = new SolutionProject(projectGUID,
                                                              transformationProjectName,
                                                              new Guid("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"),
                                                              ProjectType.CSharp,
                                                              new FileInfo(transformationProjectName + "/" + transformationProjectName + ".csproj")
                                                              );


                ConfigurationPlatform debugConfig = new ConfigurationPlatform("Debug|Any CPU.ActiveCfg", 
                                                                              "Debug", 
                                                                              "Any CPU");
                project.ConfigurationPlatforms.Add(debugConfig);

                ConfigurationPlatform debugConfig2 = new ConfigurationPlatform("Debug|Any CPU.Build.0",
                                                                              "Debug",
                                                                              "Any CPU");
                project.ConfigurationPlatforms.Add(debugConfig2);

                ConfigurationPlatform releaseConfig = new ConfigurationPlatform("Release|Any CPU.ActiveCfg",
                                                                              "Release",
                                                                              "Any CPU");
                project.ConfigurationPlatforms.Add(releaseConfig);

                ConfigurationPlatform releaseConfig2 = new ConfigurationPlatform("Release|Any CPU.Build.0",
                                                                              "Release",
                                                                              "Any CPU");
                project.ConfigurationPlatforms.Add(releaseConfig2);


                solutionFolder.Projects.Add(project);

                solution.AllProjects.Add(project);
            }
            
            SolutionWriter solutionWriter = new SolutionWriter();

            solutionWriter.WriteSolutionFile(solution, solutionFilename);


            
        }
    }
}