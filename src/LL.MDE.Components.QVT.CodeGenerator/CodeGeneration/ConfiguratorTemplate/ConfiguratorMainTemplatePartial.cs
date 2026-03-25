using LL.MDE.Components.Qvt.Metamodel.QVTBase;
using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.ConfiguratorTemplate
{
    public partial class ConfiguratorMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly ISet<ITypedModel> validCheckTargetParams = new HashSet<ITypedModel>();
        private readonly ISet<ITypedModel> validEnforceTargetParams = new HashSet<ITypedModel>();
        private readonly bool useMetamodelInterface;

        public ConfiguratorMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            this.Transformation = transformation;
            this.useMetamodelInterface = useMetamodelInterface;

           

        }
    }
}
