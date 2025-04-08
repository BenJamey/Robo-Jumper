using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationScript : MonoBehaviour
{
    Animator CharAnimator;
    CharacterMovement CharMovement;
    CharacterMovement CharacterController;


    void Start() {
        CharAnimator = GetComponent<Animator>();
        CharMovement = GetComponent<CharacterMovement>();
    }

    
    void Update() {
        if (CharMovement.Direction.magnitude > 0)
        {
            if (CharMovement.Speed > 10) {
                CharAnimator.SetBool("IsRunning", true);
            }
            else if (CharMovement.Speed < 10) {
                CharAnimator.SetBool("IsRunning", false);
            }
            CharAnimator.SetBool("IsWalking", true);
        } else {
            CharAnimator.SetBool("IsWalking", false);
            CharAnimator.SetBool("IsRunning", false);
            //RunTime = 0;
        }


        if (CharMovement.JumpPressed) {
            CharAnimator.SetBool("Jumped", true);
        } else {
            CharAnimator.SetBool("Jumped", false);
        }

        if (!CharMovement.CurrentlyGrounded)
        {
            CharAnimator.SetBool("Falling", true);
        } else
        {
            CharAnimator.SetBool("Falling", false);
        }

        if (CharacterMovement.isDead) {
            CharAnimator.SetBool("HasDied", true);
        }

        if (CharacterMovement.LevelComplete) {
            CharAnimator.SetBool("Victory", true);
        }
    }
}
