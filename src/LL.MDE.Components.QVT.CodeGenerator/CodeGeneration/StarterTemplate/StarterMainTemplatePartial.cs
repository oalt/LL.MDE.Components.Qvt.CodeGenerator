using LL.MDE.Components.Qvt.CodeGenerator.Analysis;
using LL.MDE.Components.Qvt.Metamodel.QVTBase;
using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using System.Collections.Generic;
using System.Linq;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.StarterTemplate
{
    public partial class StarterMainTemplate
    {
        private IRelationalTransformation Transformation { get; }
        private readonly ISet<ITypedModel> validCheckTargetParams = new HashSet<ITypedModel>();
        private readonly ISet<ITypedModel> validEnforceTargetParams = new HashSet<ITypedModel>();
        private readonly bool useMetamodelInterface;

        public StarterMainTemplate(IRelationalTransformation transformation, bool useMetamodelInterface = true)
        {
            this.Transformation = transformation;
            this.useMetamodelInterface = useMetamodelInterface;

            foreach (IRelation relation in transformation.Rule.OfType<IRelation>())
            {
                foreach (IRelationDomain domain in relation.Domain.OfType<IRelationDomain>())
                {
                    if (Validator.IsValidTargetDomain(domain) && transformation.ModelParameter.Contains(domain.TypedModel))
                    {
                        if (domain.IsCheckable.HasValue && domain.IsCheckable.Value)
                            validCheckTargetParams.Add(domain.TypedModel);
                        if (domain.IsEnforceable.HasValue && domain.IsEnforceable.Value)
                            validEnforceTargetParams.Add(domain.TypedModel);
                    }
                }
            }

            //IRelation relation = null;
            //IEnumerable<IRelationDomain> relationDomains = relation.Domain.OfType<IRelationDomain>();

            ////foreach (IRelationDomain relationDomain in relationDomains)
            ////{
            ////    relationDomain.RootVariable.GetRealTypeName();
            ////}

        }
    }
}
