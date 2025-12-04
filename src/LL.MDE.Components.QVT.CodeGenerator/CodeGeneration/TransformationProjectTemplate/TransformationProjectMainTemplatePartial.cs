using LL.MDE.Components.Qvt.Metamodel.CustomExtensions.EMOFExtensions;
using LL.MDE.Components.Qvt.Metamodel.EMOF;
using LL.MDE.Components.Qvt.Metamodel.QVTBase;
using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using System.Collections.Generic;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.TransformationProjectTemplate
{
    public partial class TransformationProjectMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly ISet<ITypedModel> validCheckTargetParams = new HashSet<ITypedModel>();
        private readonly ISet<ITypedModel> validEnforceTargetParams = new HashSet<ITypedModel>();
        private readonly bool _useMetamodelInterface;

        public TransformationProjectMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            Transformation = transformation;
            _useMetamodelInterface = useMetamodelInterface;

            transformation.GetMetaModelPackagesForTransformation();

            //foreach (IRelation relation in transformation.Rule.OfType<IRelation>())
            //{
            //    foreach (IRelationDomain domain in relation.Domain.OfType<IRelationDomain>())
            //    {
            //        if (Validator.IsValidTargetDomain(domain) && transformation.ModelParameter.Contains(domain.TypedModel))
            //        {
            //            if (domain.IsCheckable.HasValue && domain.IsCheckable.Value)
            //                validCheckTargetParams.Add(domain.TypedModel);
            //            if (domain.IsEnforceable.HasValue && domain.IsEnforceable.Value)
            //                validEnforceTargetParams.Add(domain.TypedModel);
            //        }
            //    }
            //}

            
        }
    }
}
