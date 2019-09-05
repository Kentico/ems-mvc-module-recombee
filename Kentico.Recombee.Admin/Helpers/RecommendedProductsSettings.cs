using CMS.Base;
using CMS.Core;

namespace Kentico.Recombee.Helpers
{
    /// <summary>
    /// Encapsulates Recombee settings.
    /// </summary>
    public class RecommendedProductsSettings
    {
        private const string IDENTIFIER_KEY = "RecombeeDatabaseIdentifier";
        private const string SECRETTOKEN_KEY = "RecombeeDatabaseSecretToken";

        /// <summary>
        /// Returns Recombee database identifier.
        /// </summary>
        /// <returns></returns>
        public static string GetDatabaseId()
        {
            var settingsService = Service.Resolve<ISettingsService>();
            var siteService = Service.Resolve<ISiteService>();

            return settingsService[$"{siteService.CurrentSite?.SiteName}.{IDENTIFIER_KEY}"];
        }


        /// <summary>
        /// Returns Recombee database secret key.
        /// </summary>
        /// <returns></returns>
        public static string GetSecretToken()
        {
            var settingsService = Service.Resolve<ISettingsService>();
            var siteService = Service.Resolve<ISiteService>();

            return settingsService[$"{siteService.CurrentSite?.SiteName}.{SECRETTOKEN_KEY}"];
        }
    }
}
