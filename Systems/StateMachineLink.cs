namespace Hiveware {
  /// <summary>Represents a link between the parent StateMachineNode, and the StateMachineNode Target.</summary>
  public class StateMachineLink {

    public delegate bool TestDelegate(StateMachine parent);

    /// <summary>The test to run when checking whether or not to switch to the Target state.</summary>
    public TestDelegate Test;
    /// <summary>The Target state to transition to if the Test condition is met.</summary>
    public StateMachineNode Target;

    /// <summary>Runs the test associated with the state machine link and returns true if the link condition is satisfied.</summary>
    /// <param name="parent">The StateMachine that contains this link.</param>
    /// <returns></returns>
    public bool CanTransition(StateMachine parent) {
      return Test?.Invoke(parent) ?? true;
    }

    /// <param name="target">StateMachineNode this link will transition to if the test condition passes.</param>
    public StateMachineLink(StateMachineNode target) {
      this.Target = target;
    }

    /// <param name="target">StateMachineNode this link will transition to if the test condition passes.</param>
    /// <param name="test">Function for the test to perform to determine whether or not to transition to the linked state.</param>
    public StateMachineLink(StateMachineNode target, TestDelegate test) {
      this.Target = target;
      this.Test = test;
    }
  }
}