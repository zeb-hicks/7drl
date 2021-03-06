using System;

namespace Hiveware {
  public static class Program {
    [STAThread]
    static void Main() {
      HWGame.globalInstance = new HWGame();
      HWGame.globalInstance.Run();
    }
  }
}
