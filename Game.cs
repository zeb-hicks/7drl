using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Hiveware {
  public class HWGame : Microsoft.Xna.Framework.Game {
    public GraphicsDeviceManager graphics;
    public SpriteBatch SB;

    public static HWGame globalInstance;

    public Scene CurrentScene;

    public ContentManager CM;

    public HWGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize() {
      DeferredRenderer.Setup(GraphicsDevice);

      base.Initialize();
    }

    Effect fx, lfx, cfx;
    CachedTexture ct;
    int[] tiles;

    protected override void LoadContent() {
      SB = new SpriteBatch(GraphicsDevice);
      CM = Content;

      fx = Content.Load<Effect>("Shaders/TilemapEffect");
      lfx = Content.Load<Effect>("Shaders/Lighting");
      cfx = Content.Load<Effect>("Shaders/Composite");

      int i;
      (i, ct) = TextureCache.Get("Art/tiles");

      tiles = new int[256];

      for (i = 0; i < 256; i++) {
        tiles[i] = i;
      }
    }

    protected override void Update(GameTime gameTime) {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      CurrentScene?.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Transparent);

      fx.Parameters["Tiles"]?.SetValue(tiles);
      fx.Parameters["TileSize"]?.SetValue(16);
      fx.Parameters["MapSize"]?.SetValue(new Vector2(16f, 16f));

      fx.Parameters["diffuse"]?.SetValue(ct.Texture.Diffuse);
      fx.Parameters["normal"]?.SetValue(ct.Texture.Normal);
      fx.Parameters["specular"]?.SetValue(ct.Texture.Specular);
      fx.Parameters["emissive"]?.SetValue(ct.Texture.Emissive);
      fx.Parameters["depth"]?.SetValue(ct.Texture.Depth);

      fx.Parameters["gdiff"]?.SetValue(DeferredRenderer.rtDiffuse);
      fx.Parameters["gnorm"]?.SetValue(DeferredRenderer.rtNormal);
      fx.Parameters["gdse"]?.SetValue(DeferredRenderer.rtDSE);

      cfx.Parameters["gdiff"]?.SetValue(DeferredRenderer.rtDiffuse);
      cfx.Parameters["gnorm"]?.SetValue(DeferredRenderer.rtNormal);
      cfx.Parameters["gdse"]?.SetValue(DeferredRenderer.rtDSE);
      cfx.Parameters["glight"]?.SetValue(DeferredRenderer.rtLighting);

      GraphicsDevice.SetRenderTarget(DeferredRenderer.rtNormal);
      GraphicsDevice.Clear(ClearOptions.Target, new Vector4(0.5f, 0.5f, 1f, 1f), 0f, 0);

      GraphicsDevice.SetRenderTargets(
        DeferredRenderer.rtDiffuse,
        DeferredRenderer.rtNormal,
        DeferredRenderer.rtDSE
      );

      SB.Begin(effect: fx);
      SB.Draw(ct.Texture.Diffuse, new Rectangle(0, 0, 1024, 1024), Color.White);
      SB.End();
      // DeferredRenderer.DrawEffect(fx, new Rectangle(0, 0, 1024, 1024));

      int w = GraphicsDevice.Viewport.Width;
      int h = GraphicsDevice.Viewport.Height;

      GraphicsDevice.SetRenderTarget(null);

      SB.Begin();
      SB.Draw(DeferredRenderer.rtDiffuse, new Rectangle(0, 0, w / 2, h / 2), Color.White);
      SB.Draw(DeferredRenderer.rtNormal, new Rectangle(w / 2, 0, w / 2, h / 2), Color.White);
      SB.Draw(DeferredRenderer.rtDSE, new Rectangle(0, h / 2, w / 2, h / 2), Color.White);
      SB.End();

      // GraphicsDevice.SetRenderTarget(DeferredRenderer.rtLighting);

      // SB.Begin(effect: lfx);
      // SB.Draw(ct.Texture.Diffuse, new Rectangle(0, 0, w, h), Color.White);
      // SB.End();

      SB.Begin(effect: cfx);

      SB.Draw(DeferredRenderer.rtDiffuse, new Rectangle(w / 2, h / 2, w / 2, h / 2), Color.White);

      SB.End();

      base.Draw(gameTime);
    }
  }
}
