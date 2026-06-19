using LL.MDE.Components.Qvt.Metamodel.CustomExtensions.EMOFExtensions;
using LL.MDE.Components.Qvt.Metamodel.QVTRelation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.MDE.Components.Qvt.CodeGenerator.CodeGeneration.ExtensionMethods
{
    public static class TransformationExtensions
    {
        public static bool EnforceToEA(this IRelationalTransformation transformation)
        {
            bool result = false;

            IEnumerable<IRelation> topRelations = transformation.Rule.OfType<IRelation>().Where(r => r.IsTopLevel.GetValueOrDefault(false));

            foreach (IRelation topRelation in topRelations)
            {
                List<IRelationDomain> targetDomains = topRelation.Domain.Where(d => d.IsEnforceable.GetValueOrDefault()).OfType<IRelationDomain>().ToList();
            
                foreach (IRelationDomain domain in targetDomains)
                {
                    if(domain.RootVariable.Type.GetRealTypeName().StartsWith("EA."))
                    {
                        result = true;
                        break;
                    }
                }
                
            }
            return result;
        }
    }
}
