using System;

namespace Hiveware {
  public static class Program {
    [STAThread]
    static void Main() {
      Game.globalInstance = new Game();
      Game.globalInstance.Run();
    }
  }
}
