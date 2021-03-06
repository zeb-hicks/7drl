using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Hiveware {
  public class CachedTexture {
    public Texture2D Texture;
    public CachedTexture(Texture2D texture) {
      Texture = texture;
    }
  }
  public static class TextureCache {
    private static UniqueDictionary<int, CachedTexture> cache = new UniqueDictionary<int, CachedTexture>();
    private static Dictionary<string, int> lookup = new Dictionary<string, int>();
    private static int nextID = 0;
    public static CachedTexture Get(int id) {
      return cache[id];
    }
    public static (int, CachedTexture) Load(string path) {
      Texture2D tex = Game.globalInstance.CM.Load<Texture2D>(path);

      int i = nextID++;
      var ct = new CachedTexture(tex);
      lookup.Add(path, i);

      cache.Add(i, ct);

      return (i, ct);
    }
    public static (int, CachedTexture) Get(string path) {
      int i;
      CachedTexture t;
      if (lookup.ContainsKey(path)) {
        i = lookup[path];
        t = cache[lookup[path]];
      } else {
        (i, t) = Load(path);
      }
      return (i, t);
    }
  }
}