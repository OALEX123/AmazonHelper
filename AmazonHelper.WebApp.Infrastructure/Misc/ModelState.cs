using System.Collections.Generic;
using System.Web.Mvc;

namespace AmazonHelper.WebApp.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static string ToJsonErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<object>();
            foreach (var state in modelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors.ToJsonModel();
        }
    }
}
