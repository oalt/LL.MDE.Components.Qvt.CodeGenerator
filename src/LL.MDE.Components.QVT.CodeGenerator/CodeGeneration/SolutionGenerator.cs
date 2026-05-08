using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using SlnParser;
using SlnParser.Contracts;
using System;
using System.IO;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration
{
    internal class SolutionGenerator
    {
        private const string CSHARP_PROJECT_TYPE_GUID = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
        private const string SOLUTION_FOLDER_TYPE_GUID = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        private string _path;
        private IRelationalTransformation _transformation;
        private bool _generateMicroservice;

        public SolutionGenerator(string path, IRelationalTransformation transformation, bool generateMicroservice) 
        {
            _path = path;
            _transformation = transformation;
            _generateMicroservice = generateMicroservice;
        }

        public string SolutionName { get; set; } = "MDD4All.QVT.Transformations-dev.sln";

        public string TransformationsSolutionFolderName { get; set; } = "1_Transformations";

        public void CreateOrModifySolution()
        {
            bool changed = false;

            string solutionFilename = _path + "/" + SolutionName;

            string transformationProjectName = QvtCodeGeneratorStrings.TransformationProjectName(_transformation);
            string uiProjectName = QvtCodeGeneratorStrings.UserInterfaceProjectName(_transformation);
            string configurationProjectName = QvtCodeGeneratorStrings.ConfigurationProjectName(_transformation);
            string microserviceProjectName = QvtCodeGeneratorStrings.MicroserviceProjectName(_transformation);

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
                changed = true;
            }
            else
            {
                SolutionParser solutionParser = new SolutionParser();
                solution = solutionParser.Parse(solutionFilename) as Solution;
            }

            IProject transformationsFolder = solution.AllProjects.Find(project => project.Name == TransformationsSolutionFolderName
                                             && project.Type == ProjectType.SolutionFolder);

            // 1_Transformations solution folder
            if (transformationsFolder == null)
            {
                Guid transformationsFolderGuid = Guid.NewGuid();
                transformationsFolder = new SolutionFolder(transformationsFolderGuid,
                                                           TransformationsSolutionFolderName,
                                                           new Guid(SOLUTION_FOLDER_TYPE_GUID),
                                                           ProjectType.SolutionFolder);

                solution.AllProjects.Add(transformationsFolder);
                changed = true;
            }


            IProject transformationFolder = solution.AllProjects.Find(project => project.Name == transformationProjectName
                                          && project.Type == ProjectType.SolutionFolder);

            // transformation solution folder
            if (transformationFolder == null)
            {
                Guid solutionFolderGuid = Guid.NewGuid();

                SolutionFolder transformationProjectSolutionFolder = new SolutionFolder(solutionFolderGuid,
                                                                   transformationProjectName,
                                                                   new Guid(SOLUTION_FOLDER_TYPE_GUID),
                                                                   ProjectType.SolutionFolder);

                solution.AllProjects.Add(transformationProjectSolutionFolder);

                SolutionFolder transformationsSolutionFolder = (SolutionFolder)transformationsFolder;

                transformationsSolutionFolder.Projects.Add(transformationProjectSolutionFolder);

                transformationFolder = transformationProjectSolutionFolder;
                changed = true;
            }

            // transformation rule project
            if(solution.AllProjects.Find(project => project.Name == transformationProjectName
                                                    && project.Type == ProjectType.CSharp) == null)
            {
                SolutionFolder transformationProjectSolutionFolder = (SolutionFolder)transformationFolder;

                Guid projectGUID = Guid.NewGuid();


                SolutionProject project = CreateSolutionProject(projectGUID, transformationProjectName,
                                                                new FileInfo(transformationProjectName + "/" + transformationProjectName + ".csproj"));


                transformationProjectSolutionFolder.Projects.Add(project);

                solution.AllProjects.Add(project);
                changed = true;
            }

            // UI project
            if (solution.AllProjects.Find(project => project.Name == uiProjectName
                                                    && project.Type == ProjectType.CSharp) == null)
            {
                SolutionFolder transformationProjectSolutionFolder = (SolutionFolder)transformationFolder;

                Guid projectGUID = Guid.NewGuid();


                SolutionProject project = CreateSolutionProject(projectGUID, uiProjectName,
                                                                new FileInfo(uiProjectName + "/" + uiProjectName + ".csproj"));


                transformationProjectSolutionFolder.Projects.Add(project);

                solution.AllProjects.Add(project);
                changed = true;
            }

            // configuration project
            if (solution.AllProjects.Find(project => project.Name == configurationProjectName
                                                     && project.Type == ProjectType.CSharp) == null)
            {
                SolutionFolder transformationProjectSolutionFolder = (SolutionFolder)transformationFolder;

                Guid projectGUID = Guid.NewGuid();


                SolutionProject project = CreateSolutionProject(projectGUID, configurationProjectName,
                                                                new FileInfo(configurationProjectName + "/" + configurationProjectName + ".csproj"));


                transformationProjectSolutionFolder.Projects.Add(project);

                solution.AllProjects.Add(project);
                changed = true;
            }

            // microservice project
            if (_generateMicroservice)
            {
                if (solution.AllProjects.Find(project => project.Name == microserviceProjectName
                                                         && project.Type == ProjectType.CSharp) == null)
                {
                    SolutionFolder transformationProjectSolutionFolder = (SolutionFolder)transformationFolder;

                    Guid projectGUID = Guid.NewGuid();


                    SolutionProject project = CreateSolutionProject(projectGUID, microserviceProjectName,
                                                                    new FileInfo(microserviceProjectName + "/" + microserviceProjectName + ".csproj"));


                    transformationProjectSolutionFolder.Projects.Add(project);

                    solution.AllProjects.Add(project);
                    changed = true;
                }
            }

            if (changed)
            {
                SolutionWriter solutionWriter = new SolutionWriter();

                solutionWriter.WriteSolutionFile(solution, solutionFilename);
            }
        }

        private SolutionProject CreateSolutionProject(Guid guid, string name, FileInfo relativePath)
        {
            SolutionProject project = new SolutionProject(guid,
                                                          name,
                                                          new Guid(CSHARP_PROJECT_TYPE_GUID),
                                                          ProjectType.CSharp,
                                                          relativePath
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

            return project;
        }
    }
}
