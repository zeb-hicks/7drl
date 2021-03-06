using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Hiveware {
  public class HWGame : Microsoft.Xna.Framework.Game {
    public GraphicsDeviceManager graphics;
    public SpriteBatch SB;

    public static HWGame globalInstance;

    public SceneNode SceneRoot;

    public ContentManager CM;

    public HWGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize() {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    protected override void LoadContent() {
      SB = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime) {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Transparent);

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
