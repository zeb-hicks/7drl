using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Hiveware {
  public class Entity: IUpdateable, IDrawable {
    public Sprite Sprite;
    public Vector3 Position;

    public StateMachine State;

    public Entity() {
      
    }

    private bool _Enabled;
    public bool Enabled {
      get => _Enabled;
      set {
        _Enabled = value;
        EnabledChanged?.Invoke(this, null);
      }
    }

    private int _UpdateOrder;
    public int UpdateOrder {
      get => _UpdateOrder;
      set {
        _UpdateOrder = value;
        UpdateOrderChanged?.Invoke(this, null);
      }
    }

    private int _DrawOrder;
    public int DrawOrder {
      get => _DrawOrder;
      set {
        _DrawOrder = value;
        DrawOrderChanged?.Invoke(this, null);
      }
    }

    private bool _Visible;
    public bool Visible {
      get => _Visible;
      set {
        _Visible = value;
        VisibleChanged?.Invoke(this, null);
      }
    }
    
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;

    public void Update(GameTime gameTime) {
      if (!this.Enabled) return;
      this.State.Update(gameTime);
    }

    public void Draw(GameTime gameTime) {
      throw new NotImplementedException();
    }
  }

  public class PhysicsEntity : Entity {

  }
}
