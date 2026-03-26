using LL.MDE.Components.Qvt.Metamodel.QVTRelation;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.Microservice.ProgramTemplate
{
    public partial class ProgramMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly bool _useMetamodelInterface;

        public ProgramMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            Transformation = transformation;
            _useMetamodelInterface = useMetamodelInterface;
        }
    }
}
