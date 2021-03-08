using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Hiveware {
  /// <summary>Finite state machine.</summary>
  public class StateMachine: IUpdateable {

#region Members
    /// <summary>Child state nodes.</summary>
    public HashSet<StateMachineNode> Nodes = new HashSet<StateMachineNode>();
    /// <summary>The current state of this StateMachine.</summary>
    public StateMachineNode currentNode;
    /// <summary>Time (in seconds) the current state has been active.</summary>
    public float nodeTime = 0f;
#endregion

#region Event Handlers
    private bool _enabled = true;
    private int _updateOrder = 0;
    public bool Enabled { get { return _enabled; } set { _enabled = value; EnabledChanged.Invoke(this, null); } }
    public int UpdateOrder { get { return _updateOrder; } set { _updateOrder = value; UpdateOrderChanged.Invoke(this, null); } }
    public event EventHandler<EventArgs> EnabledChanged, UpdateOrderChanged;

    public class TransitionEventArgs: EventArgs {
      public StateMachineNode from, to;
      public StateMachineLink link;
      public TransitionEventArgs(StateMachineNode from, StateMachineNode to, StateMachineLink link): base() {
        this.from = from;
        this.to = to;
        this.link = link;
      }
    }

    public event EventHandler<TransitionEventArgs> OnTransition;
#endregion

#region Constructors

    public StateMachine() {}
    public StateMachine(params StateMachineNode[] nodes) {
      Nodes.UnionWith(nodes);
    }

#endregion

#region Node Management

    /// <summary>Add a unique state node to this StateMachine.</summary>
    /// <param name="node">The unique state node to add.</param>
    public void AddNode(StateMachineNode node) {
      Nodes.Add(node);
    }

    /// <summary>Link two states together with a specified link.</summary>
    /// <param name="a">The state to transition from.</param>
    /// <param name="b">The linked state to transition to.</param>
    /// <param name="link">The link defining the transition.</param>
    public void LinkNodes(StateMachineNode a, StateMachineNode b, StateMachineLink link) {
      if (a == b) throw new NotSupportedException("Cannot link a node to itself.");
      if (!Nodes.Contains(a) || !Nodes.Contains(b)) throw new NotSupportedException("Cannot link nodes that are not contained within the state machine.");
      if (!a.Links.Contains((b, link))) a.Links.Add((b, link));
    }

    /// <summary>Link two states together with a simple test function.</summary>
    /// <param name="a">The state to transition from.</param>
    /// <param name="b">The linked state to transition to.</param>
    /// <param name="test"></param>
    public void LinkNodes(StateMachineNode a, StateMachineNode b, StateMachineLink.TestDelegate test) {
      if (a == b) throw new NotSupportedException("Cannot link a node to itself.");
      if (!Nodes.Contains(a) || !Nodes.Contains(b)) throw new NotSupportedException("Cannot link nodes that are not contained within the state machine.");
      var link = new StateMachineLink(b, test);
      a.Links.Add((b, link));
    }

    /// <summary>Add a link to a state.</summary>
    /// <param name="a">The state to transition from.</param>
    /// <param name="link">The link defining the transition.</param>
    public void LinkNodes(StateMachineNode a, StateMachineLink link) {
      LinkNodes(a, link.Target, link);
    }

#endregion

#region State Management

    public void Update(GameTime gameTime) {
      if (!Enabled) return;

      nodeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

      StateMachineNode t = null;
      do {
        t = currentNode?.TryTransition(this);
      } while (t != null);

      currentNode.Update(new StateUpdateEventArgs(this, gameTime));
    }

    /// <summary>Triggers a transition to a new state via a specified link.</summary>
    /// <param name="node">The state node to transition to.</param>
    /// <param name="link">The link which triggered this new state.</param>
    public void Transition(StateMachineNode node, StateMachineLink link) {
      var e = new TransitionEventArgs(this.currentNode, node, link);
      nodeTime = 0f;
      this.currentNode = node;
      OnTransition?.Invoke(this, e);
    }

#endregion

  }
}