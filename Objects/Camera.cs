using Microsoft.Xna.Framework;

namespace Hiveware {
  public class Camera : SceneNode {
    public readonly Matrix ViewMatrix;

    public float Width;
    public float Height;

    public Camera(float Width, float Height) : base() {
      ViewMatrix = Matrix.CreateOrthographic(Width, Height, 1f, 256f);
      ViewMatrix.Translation = -Position;
    }
  }
}
