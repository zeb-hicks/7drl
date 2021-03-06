using Microsoft.Xna.Framework;
using Hiveware;
using System.Collections.Generic;
using System;

namespace Hiveware {
  public class SceneNode: DrawableGameComponent {

    /// <summary>
    /// Set of child nodes for this SceneNode.
    /// </summary>
    public HashSet<SceneNode> Children = new HashSet<SceneNode>();
    public SceneNode Parent;
    public Vector3 Position;
    private bool positionDirty = true;
    private Vector3 worldPos;
    public Vector3 WorldPosition {
      get {
        if (positionDirty) {
          if (Parent != null) {
            worldPos = Parent.worldPos + Position;
          } else {
            worldPos = Position;
          }
        }
        return worldPos;
      }
      set {
        if (Parent != null) {
          Position = value - Parent.WorldPosition;
        } else {
          Position = value;
        }
      }
    }

    public SceneNode(): base(HWGame.globalInstance) {
      this.Parent = null;
      this.Position = new Vector3();
    }

    public SceneNode(SceneNode parent, Vector3 position = new Vector3()): base(HWGame.globalInstance) {
      this.Parent = parent;
      this.Position = position;
    }

    public void AddChildren(params SceneNode[] child) {
      for (int i = 0; i < child.Length; i++) {
        this.Children.Add(child[i]);
      }
    }

    public void AddChildren(IEnumerable<SceneNode> children) {
      foreach (SceneNode c in children) {
        this.Children.Add(c);
      }
    }

    public void RemoveChildren(params SceneNode[] child) {
      for (int i = 0; i < child.Length; i++) {
        if (this.Children.Contains(child[i])) {
          this.Children.Remove(child[i]);
          child[i].Enabled = false;
        }
      }
    }

    public override void Draw(GameTime time) {
      base.Draw(time);
      foreach (DrawableGameComponent child in Children) {
        child.Draw(time);
      }
    }

  }
}
