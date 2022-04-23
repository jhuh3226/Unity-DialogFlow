using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestDolphinControl : MonoBehaviour {
    [SerializeField] Animator animator;
    bool IsJumpHigh, IsJumpSmooth, IsTurn, IsTalk, IsNodeHead = false;

    bool dolphinComeBack, dolphinSwim = false;
    bool swimTriggerOnce = true;
    void Start () {
        animator = gameObject.GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update () {
        ControlAnimation ();

        if (Input.GetKeyDown (KeyCode.Space)) {
            Debug.Log ("call dolphin");
            dolphinComeBack = true;
            dolphinSwim = false;
        }
        if (Input.GetKeyDown (KeyCode.Return)) {
            Debug.Log ("swim");
            dolphinSwim = true;
            dolphinComeBack = false;
            swimTriggerOnce = true;
            GetComponent<BeizerCurve> ().resetOn = true;
        }

        if (dolphinComeBack) Call ();
        if (dolphinSwim) Swim ();
    }

    /*------animation------*/
    void ControlAnimation () {
        // jump low
        if (Input.GetKeyDown (KeyCode.Alpha1)) {
            animator.SetBool ("IsJumpSmooth", true);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
        }

        // jump high
        if (Input.GetKeyDown (KeyCode.Alpha2)) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", true);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
        }

        // rotate
        if (Input.GetKeyDown (KeyCode.Alpha3)) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", true);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
        }

        // speak
        if (Input.GetKeyDown (KeyCode.Alpha4)) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", true);
            animator.SetBool ("IsNodHead", false);
        }
        // shake head
        if (Input.GetKeyDown (KeyCode.Alpha5)) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", true);
        }
        // non, just swim
        if (Input.GetKeyDown (KeyCode.Q)) {
            Debug.Log ("Non, just swim");
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
        }

    }

    /*------call dolphin------*/
    void Call () {
        GetComponent<BeizerCurve> ().beizerCurveOn = false;
        GetComponent<ControlDolphinLocation> ().MoveTowardsTarget (); // move dolphin's position to initial starting point
    }

    /*------call dolphin and start rerotating------*/
    void Swim () {
        // if (swimTriggerOnce) {
        // GetComponent<ControlDolphinLocation> ().MoveTowardsTarget (); // move dolphin's position to initial starting point
        // GetComponent<BeizerCurve> ().beizerCurveOn = true;
        if (GetComponent<BeizerCurve> ().resetOn) {
            GetComponent<BeizerCurve> ().RotateDesiredAngle ();
        }
        // swimTriggerOnce = false;
    }
}

/*------dolphin gets bigger------*/

/*------petition------*/
// allow petition by enabling the canvas

/*------visible tank------*/