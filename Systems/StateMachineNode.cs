using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Hiveware {
  /// <summary>Represents a discrete state possible within the parent StateMachine, along with any links to other states.</summary>
  public class StateMachineNode {
    public HashSet<(StateMachineNode, StateMachineLink)> Links = new HashSet<(StateMachineNode, StateMachineLink)>();

    /// <param name="links">Tuple of StateMachineNode and Link pairs that define transitions from this state.</param>
    public StateMachineNode(params (StateMachineNode, StateMachineLink)[] links) {
      Links.UnionWith(links);
    }

    /// <summary>Update event for when this is the current state.</summary>
    public event EventHandler<StateUpdateEventArgs> OnUpdate;
    /// <summary>Called when the state machine switches to this state.</summary>
    public event EventHandler<StateEnterEventArgs> OnEnter;
    /// <summary>Called when the state machine switches away from this state.</summary>
    public event EventHandler<StateLeaveEventArgs> OnLeave;

    /// <summary>Update event for when this is the current state.</summary>
    public void Update (StateUpdateEventArgs e) { this.OnUpdate?.Invoke(this, e); }
    /// <summary>Called when the state machine switches to this state.</summary>
    public void Enter (StateEnterEventArgs e) { this.OnEnter?.Invoke(this, e); }
    /// <summary>Called when the state machine switches away from this state.</summary>
    public void Leave (StateLeaveEventArgs e) { this.OnLeave?.Invoke(this, e); }

    /// <summary>If this node has a valid link to follow, returns the node that should be transitioned to.</summary>
    public StateMachineNode TryTransition(StateMachine parent) {
      foreach (var (node, link) in Links) {
        if (link.CanTransition(parent)) {
          return link.Target;
        }
      }
      return null;
    }
  }

  public class StateUpdateEventArgs: EventArgs {
    /// <summary>The StateMachine this state belongs to.</summary>
    public StateMachine parent;
    /// <summary>The GameTime containing the elapsed time for this update cycle.</summary>
    public GameTime gameTime;

    public StateUpdateEventArgs(StateMachine parent, GameTime gameTime): base() {
      this.parent = parent;
      this.gameTime = gameTime;
    }
  }

  public class StateEnterEventArgs: EventArgs {
    /// <summary>The StateMachine this state belongs to.</summary>
    public StateMachine parent;
    /// <summary>The previous state node that the state machine was in before entering this one.</summary>
    public StateMachineNode prev;

    public StateEnterEventArgs(StateMachine parent, StateMachineNode prev): base() {
      this.parent = parent;
      this.prev = prev;
    }
  }

  public class StateLeaveEventArgs: EventArgs {
    /// <summary>The StateMachine this state belongs to.</summary>
    public StateMachine parent;
    /// <summary>The state node that the state machine just entered after leaving this one.</summary>
    public StateMachineNode next;

    public StateLeaveEventArgs(StateMachine parent, StateMachineNode next): base() {
      this.parent = parent;
      this.next = next;
    }
  }
}