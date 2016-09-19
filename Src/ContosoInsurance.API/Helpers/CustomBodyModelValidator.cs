using System;
using System.Data.Entity.Spatial;
using System.Web.Http.Validation;

namespace ContosoInsurance.API.Helpers
{
    public class CustomBodyModelValidator : DefaultBodyModelValidator
    {
        public override bool ShouldValidateType(Type type)
        {
            return type != typeof(DbGeography) && base.ShouldValidateType(type);
        }
    }
}