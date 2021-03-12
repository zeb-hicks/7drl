using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hiveware {
  public static class AssetLoader {

    private static Dictionary<string, TextureAsset> AssetCache = new Dictionary<string, TextureAsset>();

    public static T ParseJSON<T>(string path) {
      var data = System.IO.File.ReadAllText(path);
      using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(data))) {
        var ds = new DataContractJsonSerializer(typeof(T));
        return (T)ds.ReadObject(ms);
      }
    }

    public static TextureSet GetTexture(string path) {
      TextureAsset asset;
      if (!AssetCache.ContainsKey(path)) {
        asset = ParseJSON<Hiveware.TextureAsset>($"./Content/{path}.json");
      } else {
        asset = AssetCache[path];
      }
      return new TextureSet {
        Diffuse = HWGame.globalInstance.CM.Load<Texture2D>($"{path}_diffuse"),

        Depth = asset.HasDepth ? HWGame.globalInstance.CM.Load<Texture2D>($"{path}_depth") : null,
        Emissive = asset.HasEmissive ? HWGame.globalInstance.CM.Load<Texture2D>($"{path}_emissive") : null,
        Normal = asset.HasNormal ? HWGame.globalInstance.CM.Load<Texture2D>($"{path}_normal") : null,
        Specular = asset.HasSpecular ? HWGame.globalInstance.CM.Load<Texture2D>($"{path}_specular") : null
      };
    }
  }

  public struct TextureAsset {
    public string Path;
    public bool HasDepth;
    public bool HasEmissive;
    public bool HasNormal;
    public bool HasSpecular;
  }
}
