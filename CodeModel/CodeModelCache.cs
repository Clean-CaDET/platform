using CodeModel.CaDETModel;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace CodeModel
{
    // Should be wired as a singleton service by DE and ST.
    public class CodeModelCache
    {
        private readonly MemoryCache _cache;
        public CodeModelCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 10 });
        }

        public CaDETProject GetOrCreate(string repoAndCommitUrl, string repoFolder, LanguageEnum language = LanguageEnum.CSharp)
        {
            if (_cache.TryGetValue(repoAndCommitUrl, out CaDETProject cacheEntry)) return cacheEntry;

            cacheEntry = new CodeModelFactory(language).CreateProjectWithCodeFileLinks(repoFolder);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                Size = 1,
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            _cache.Set(repoAndCommitUrl, cacheEntry, cacheEntryOptions);
            return cacheEntry;
        }
    }
}
