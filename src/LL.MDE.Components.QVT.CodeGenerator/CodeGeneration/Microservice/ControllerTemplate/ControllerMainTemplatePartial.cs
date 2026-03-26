using LL.MDE.Components.Qvt.Metamodel.QVTRelation;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ControllerTemplate
{
    public partial class ControllerMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly bool _useMetamodelInterface;

        public ControllerMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            Transformation = transformation;
            _useMetamodelInterface = useMetamodelInterface;
        }

    }

}
