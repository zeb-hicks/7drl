using Microsoft.Xna.Framework;

namespace Hiveware {
  public static class Utils {
    public static Vector2 From3to2(Vector3 v) {
      return new Vector2(v.X, v.Y);
    }

    public static Vector3 From2to3(Vector2 v) {
      return new Vector3(v.X, v.Y, 0f);
    }
  }
}
