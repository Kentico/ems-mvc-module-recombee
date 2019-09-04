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
            return Service.Resolve<ISettingsService>()[IDENTIFIER_KEY];
        }


        /// <summary>
        /// Returns Recombee database secret key.
        /// </summary>
        /// <returns></returns>
        public static string GetSecretToken()
        {
            return Service.Resolve<ISettingsService>()[SECRETTOKEN_KEY];
        }
    }
}
