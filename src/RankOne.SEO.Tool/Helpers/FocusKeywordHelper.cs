using RankOne.Interfaces;
using RankOne.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RankOne.Helpers
{
    public class FocusKeywordHelper : IFocusKeywordHelper
    {
        private readonly IDashboardSettingsSerializer _dashboardSettingsSerializer;

        public FocusKeywordHelper() : this(RankOneContext.Instance)
        { }

        public FocusKeywordHelper(RankOneContext rankOneContext) : this(rankOneContext.DashboardSettingsSerializer.Value)
        { }

        public FocusKeywordHelper(IDashboardSettingsSerializer dashboardSettingsSerializer)
        {
            _dashboardSettingsSerializer = dashboardSettingsSerializer;
        }

        public string GetFocusKeyword(IPublishedContent node)
        {
            // Try property focusKeyword
            if (node.HasValue("focusKeyword"))
            {
                return node.GetPropertyValue<string>("focusKeyword");
            }

            return FindFocusKeywordInDashboardProperties(node);
        }

        private string FindFocusKeywordInDashboardProperties(IPublishedContent node)
        {
            // Try to figure out if there's a property of type RankOneDashboard on the node
            foreach (var property in node.Properties)
            {
                if (IsDashboardProperty(property))
                {
                    return GetFocusKeywordFromDashboardProperty(property);
                }
            }
            return null;
        }

        private bool IsDashboardProperty(IPublishedProperty property)
        {
            return property.HasValue && property.Value.ToString().Contains("focusKeyword");
        }

        private string GetFocusKeywordFromDashboardProperty(IPublishedProperty property)
        {
            var dashboardSettings = _dashboardSettingsSerializer.Deserialize(property.Value.ToString());
            if (dashboardSettings != null && !string.IsNullOrEmpty(dashboardSettings.FocusKeyword))
            {
                return dashboardSettings.FocusKeyword;
            }
            return null;
        }
    }
}