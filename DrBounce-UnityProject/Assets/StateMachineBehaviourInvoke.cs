using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachineBehaviourInvoke : StateMachineBehaviour
{
    // some of these cause the animations to not play and unity is being too slow for me to test each one

    public UnityEvent onStateEnter;
    //public UnityEvent onStateUpdate;
    public UnityEvent onStateExit;
    //public UnityEvent onStateMove;
    //public UnityEvent onStateIK;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnter.Invoke();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    onStateUpdate.Invoke();
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateExit.Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //    onStateMove.Invoke();
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Implement code that sets up animation IK(inverse kinematics)
    //    onStateIK.Invoke();
    //}
}
