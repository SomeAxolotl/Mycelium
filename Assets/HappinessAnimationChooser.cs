using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  NOT BEING USED RIGHT NOW. I'M USING AnimationOverrideController ASSETS INSTEAD IN EDITOR (they're being referenced in CharacterStats.cs)
//

public class HappinessAnimationChooser : StateMachineBehaviour
{
    [SerializeField] AnimationClip defaultClip;
    [SerializeField] AnimationClip happyClip;
    [SerializeField] AnimationClip sadClip;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        AnimationClip chosenIdle;
        float happiness = animator.GetFloat("Happiness");

        if (happiness > 0.5f)
        {
            chosenIdle = happyClip;
        }
        else if (happiness < 0.5f)
        {
            chosenIdle = sadClip;
        }
        else
        {
            chosenIdle = defaultClip;
        }

        // Create a list to hold the overrides
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
        overrideController.GetOverrides(overrides);

        // Find the matching state and replace its clip
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == defaultClip.name)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, chosenIdle);
                break;
            }
        }

        // Apply the new overrides
        overrideController.ApplyOverrides(overrides);

        // Assign the override controller to the animator
        animator.runtimeAnimatorController = overrideController;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
