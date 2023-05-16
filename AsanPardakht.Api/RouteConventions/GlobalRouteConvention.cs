using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AsanPardakht.Api.RouteConventions
{
    public class GlobalRouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _globalPrefix;

        public GlobalRouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _globalPrefix = new AttributeRouteModel(routeTemplateProvider);

        }
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();

                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_globalPrefix, selectorModel.AttributeRouteModel);
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();

                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                        selectorModel.AttributeRouteModel = _globalPrefix;
                }
            }
        }
    }
}
