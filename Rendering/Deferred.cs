using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiveware {
  public static class DeferredRenderer {
    public static RenderTarget2D rtDiffuse;
    public static RenderTarget2D rtNormal;
    public static RenderTarget2D rtDSE;
    public static RenderTarget2D rtLighting;

    public static SpriteBatch spriteBatch;
    public static GraphicsDevice graphics;

    public static Effect fxSprite;
    public static Effect fxTilemap;
    public static Effect fxLighting;
    public static Effect fxComposite;

    public enum RenderPhase {
      Background,
      Sprites,
      Ships,
      Decals,
      Particles,
      Lighting,

      Compositing
    }

    public static RenderPhase Phase;

    public static void Setup(GraphicsDevice g) {
      graphics = g;
      spriteBatch = new SpriteBatch(graphics);

      Phase = RenderPhase.Compositing;

      rtDiffuse  = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height, true, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
      rtDSE      = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height, true, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
      rtNormal   = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height, true, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
      rtLighting = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height, true, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
    }

    public static void Draw(GameTime gameTime) {
      var mat = Matrix.Identity;
      mat.Translation = new Vector3(0f, 0f, 0f);

      Phase = RenderPhase.Ships;

      graphics.SetRenderTargets(
        rtDiffuse,
        rtNormal,
        rtDSE
      );

      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, fxTilemap, mat);
      HWGame.globalInstance.CurrentScene.Draw(gameTime);
      spriteBatch.End();

      Phase = RenderPhase.Sprites;

      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, fxSprite, mat);
      HWGame.globalInstance.CurrentScene.Draw(gameTime);
      spriteBatch.End();
    }

    public static void Composite() {
      Phase = RenderPhase.Compositing;

      spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, fxComposite, null);
    }

    private static VertexPositionColorTexture[] varr = new VertexPositionColorTexture[4];
    public static void DrawEffect(Effect fx, Rectangle rect) {
      float x = rect.X / graphics.Viewport.Width;
      float y = rect.Y / graphics.Viewport.Height;
      float w = rect.Width / graphics.Viewport.Width;
      float h = rect.Height / graphics.Viewport.Height;
      varr[0] = new VertexPositionColorTexture(new Vector3(x, y, 0.0f), Color.White, new Vector2(0.0f, 0.0f));
      varr[1] = new VertexPositionColorTexture(new Vector3(x + w, y, 0.0f), Color.White, new Vector2(1.0f, 0.0f));
      varr[2] = new VertexPositionColorTexture(new Vector3(x, y + h, 0.0f), Color.White, new Vector2(0.0f, 1.0f));
      varr[3] = new VertexPositionColorTexture(new Vector3(x + w, y + h, 0.0f), Color.White, new Vector2(1.0f, 1.0f));

      foreach (var pass in fx.CurrentTechnique.Passes) {
        pass.Apply();

        graphics.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, varr, 0, 4);
      }
    }
  }
}
