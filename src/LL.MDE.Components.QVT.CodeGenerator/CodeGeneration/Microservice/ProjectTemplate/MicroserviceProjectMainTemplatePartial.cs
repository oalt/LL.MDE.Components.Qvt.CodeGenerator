using LL.MDE.Components.Qvt.Metamodel.QVTRelation;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ProjectTemplate
{
    public partial class MicroserviceProjectMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly bool _useMetamodelInterface;

        public MicroserviceProjectMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            Transformation = transformation;
            _useMetamodelInterface = useMetamodelInterface;
        }
    }
}
