using ContosoInsurance.Common;
using Microsoft.Azure.Mobile.Server.Files;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContosoInsurance.API.Helpers
{
    public class DefaultContainerNameResolver : IContainerNameResolver
    {
        public static readonly string DefaultContainerSuffix = "images";
        private static readonly string ImageKindPattern = "^" + string.Join("|", ImageKinds.AllImageKinds.Select(i => Regex.Escape(i)));

        public Task<string> GetFileContainerNameAsync(string tableName, string recordId, string fileName)
        {
            var kind = Regex.Match(fileName, ImageKindPattern).Value;
            var containerName = $"{kind}-{DefaultContainerSuffix}";
            return Task.FromResult(containerName);
        }

        public Task<IEnumerable<string>> GetRecordContainerNames(string tableName, string recordId)
        {
            var names = GetRecordContainerNamesCore(tableName, recordId);
            return Task.FromResult(names);
        }

        private IEnumerable<string> GetRecordContainerNamesCore(string tableName, string recordId)
        {
            if (tableName == "Claim")
            {
                foreach (var kind in ImageKinds.AllClaimImageKinds)
                    yield return GetRecordContainerName(ImageKinds.Vehicle, recordId);
            }
            else if (tableName == "Vehicle")
                yield return GetRecordContainerName(ImageKinds.Vehicle, recordId);
            else
                yield break;
        }

        private string GetRecordContainerName(string kind, string recordId)
        {
            var container = $"{kind}-{DefaultContainerSuffix}";
            var recordContainer = $"{container}/{kind}-{recordId}";
            return recordContainer;
        }
    }
}