using SILDMS.Utillity.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model;

namespace SILDMS.Service
{
    public class LocalizationService : ILocalizationService
    {
        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey)
        {
            return Resources.ResourceManager.GetString(resourceKey, Resources.Culture) ?? "";
        }
    }
}
