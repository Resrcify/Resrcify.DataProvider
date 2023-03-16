using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;

namespace Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated
{
    public sealed class LocalizationDataUpdatedEventHandler : IDomainEventHandler<LocalizationDataUpdatedEvent>
    {
        private readonly ICachingService _caching;

        public LocalizationDataUpdatedEventHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task Handle(LocalizationDataUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var localizations = await _caching.GetAsync<LocalizationBundleResponse>(
                nameof(LocalizationBundleResponse), cancellationToken);
            if (localizations?.LocalizationBundle is null) return;

            await CacheLocalization(localizations.LocalizationBundle, cancellationToken);
        }

        private async Task CacheLocalization(byte[] localization, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream(localization);
            using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

            foreach (var entry in archive.Entries)
                await _caching.SetAsync(entry.Name, GetContents(entry).ToList(), cancellationToken);
        }

        private static IEnumerable<string> GetContents(ZipArchiveEntry e)
        {
            using StreamReader stm = new(e.Open(), Encoding.UTF8);
            if (stm == null) yield break;
            string line;
            while ((line = stm.ReadLine()!) != null)
                yield return line;
        }
    }
}