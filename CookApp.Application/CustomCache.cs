using Microsoft.Extensions.Caching.Memory;

public class CustomCache
{
      public MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions{SizeLimit=1024});
}