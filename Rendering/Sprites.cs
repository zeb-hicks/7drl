using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiveware {
  public struct SpriteInfo {
    
  }

  public struct SpriteSheet {
    public int texID;
    public SpriteElement[] elements;
  }

  /// <summary>
  /// Container for the data of a single sprite element from within a texture.
  /// </summary>
  public struct SpriteElement {
    public int sx;
    public int sy;
    public int sw;
    public int sh;
    public Rectangle rect { get { return new Rectangle(this.sx, this.sy, this.sw, this.sh); } }
  }

  public struct AnimatedSpriteInfo {

  }

  public partial class Sprite {
    
  }

  public class StaticSprite: Sprite {

  }

  public class AnimatedSprite: Sprite {
    public UniqueDictionary<string, SpriteElement> Frames = new UniqueDictionary<string, SpriteElement>();
    
    public AnimatedSprite() {

    }

    public void AddFrames(params (string, SpriteElement)[] frames) {
      foreach (var (name, frame) in frames) {
        Frames.TryAdd(name, frame);
      }
    }

    // public void Draw(SpriteBatch sb, Camera)
  }
}
