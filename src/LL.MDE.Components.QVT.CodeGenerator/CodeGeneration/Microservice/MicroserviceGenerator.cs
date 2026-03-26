using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.AppSettingsTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ControllerTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.LaunchSettingsTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ProgramTemplate;
using LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ProjectTemplate;
using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using System.IO;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice
{
    public class MicroserviceGenerator
    {

        private IRelationalTransformation _transformation;
        private string _outputFolderAbsolute;

        private string _microserviceProjectName;

        public MicroserviceGenerator(IRelationalTransformation transformation, string outputFolderAbsolute) 
        {
            _transformation = transformation;
            _outputFolderAbsolute = outputFolderAbsolute;

            _microserviceProjectName = "MDD4All.QVT.Transformations." + transformation.Name + ".Microservice";
        }

        public void GenerateMicroservice()
        {
            GenerateFolderStructure();
            GenerateProjectFile();
            GenerateAppSettings();
            GenerateLaunchSettings();
            GenerateProgramFile();
            GenerateController();
        }

        private void GenerateFolderStructure()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;
            if (!Directory.Exists(projectDirectoryPath))
            {
                Directory.CreateDirectory(projectDirectoryPath);
            }

            string controllersDirectoryPath = projectDirectoryPath + "/Controllers";

            if (!Directory.Exists(controllersDirectoryPath))
            {
                Directory.CreateDirectory(controllersDirectoryPath);
            }

            string propertiesDirectoryPath = projectDirectoryPath + "/Properties";

            if (!Directory.Exists(propertiesDirectoryPath))
            {
                Directory.CreateDirectory(propertiesDirectoryPath);
            }
        }

        private void GenerateProjectFile()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;

            MicroserviceProjectMainTemplate template = new MicroserviceProjectMainTemplate(_transformation);

            string code = template.TransformText();

            File.WriteAllText(projectDirectoryPath + "/" + _microserviceProjectName + ".csproj", code);
        }

        private void GenerateAppSettings()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;

            AppSettingsMainTemplate template = new AppSettingsMainTemplate();
            string code = template.TransformText();

            File.WriteAllText(projectDirectoryPath + "appsettings.json", code);
        }

        private void GenerateLaunchSettings()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;

            LaunchSettingsMainTemplate template = new LaunchSettingsMainTemplate();
            string code = template.TransformText();

            File.WriteAllText(projectDirectoryPath + "/Properties/launchSettings.json", code);
        }

        private void GenerateProgramFile()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;

            ProgramMainTemplate template = new ProgramMainTemplate(_transformation);
            string code = template.TransformText();

            File.WriteAllText(projectDirectoryPath + "/Program.cs", code);
        }

        private void GenerateController()
        {
            string projectDirectoryPath = _outputFolderAbsolute + "/" + _microserviceProjectName;
            string controllersDirectoryPath = projectDirectoryPath + "/Controllers";

            ControllerMainTemplate template = new ControllerMainTemplate(_transformation);
            string code = template.TransformText();

            File.WriteAllText(controllersDirectoryPath + "/TransformationController.cs", code);
        }
    }
}
