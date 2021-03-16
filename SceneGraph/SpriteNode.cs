using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Hiveware {
  public class SpriteNode: SceneNode {

    public Sprite Sprite;

    public override void Draw(GameTime time) {
      base.Draw(time);

      var (i, tex) = TextureCache.Get("");

      HWGame.globalInstance.SB.Draw(tex.Texture.Diffuse, new Vector2(this.WorldPosition.X, this.WorldPosition.Y), Color.White);
    }
  }
}
