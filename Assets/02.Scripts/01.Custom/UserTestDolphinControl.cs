using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestDolphinControl : MonoBehaviour {
    [SerializeField] Animator animator;
    bool IsJumpHigh, IsJumpSmooth, IsTurn, IsTalk, IsNodeHead = false;

    bool dolphinComeBack, dolphinSwim, dolphinAdjustSize = false;
    public Canvas textPetition, petitionTopics;

    public GameObject TankCircle;
    int countKeyP, countKeyS, countKeyG, countKeyT = 0;

    private Vector3 scaleChange;

    void Start () {
        animator = gameObject.GetComponent<Animator> ();
        textPetition.enabled = false;
        petitionTopics.enabled = false;
        TankCircle.SetActive (false);
    }

    void Awake () {
        scaleChange = new Vector3 (0.05f, 0.05f, 0.05f);
    }

    // Update is called once per frame
    void Update () {
        ControlAnimation ();

        // call dolphin
        if (Input.GetKeyDown (KeyCode.Space)) {
            Debug.Log ("call dolphin");
            dolphinComeBack = true;
            dolphinSwim = false;
            dolphinAdjustSize = false;
        }

        // dolphin swim in circle
        if (Input.GetKeyDown (KeyCode.Return)) {
            Debug.Log ("swim");
            dolphinSwim = true;
            dolphinComeBack = false;
            dolphinAdjustSize = false;
            GetComponent<BeizerCurve> ().resetOn = true;
        }

        // petition guide show/hide
        if (Input.GetKeyDown (KeyCode.G)) {
            countKeyG++;
        }

        // petition show/hide
        if (Input.GetKeyDown (KeyCode.P)) {
            countKeyP++;
        }

        // resize dolphin
        if (Input.GetKeyDown (KeyCode.S)) {
            countKeyS++;
            dolphinAdjustSize = true;
        }

        // show tank
        if (Input.GetKeyDown (KeyCode.T)) {
            countKeyT++;
        }

        if (dolphinComeBack) Call (); // call dolphin
        if (dolphinSwim) Swim (); // dolphi rotate around beizer curve
        ShowPetitionGuide (); // show/hide petition guides
        ShowPetition (); // show/hide petition
        ShowTank (); // show/hide tank
        if (dolphinAdjustSize) AdjustSize (); // resize dolphin
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
        if (GetComponent<BeizerCurve> ().resetOn) {
            GetComponent<BeizerCurve> ().RotateDesiredAngle ();
        }
    }

    /*------petition------*/
    // allow petition by enabling the canvas

    void ShowPetitionGuide () {
        if (countKeyG % 2 != 0) petitionTopics.enabled = true;
        else petitionTopics.enabled = false;
    }

    void ShowPetition () {
        if (countKeyP % 2 != 0) textPetition.enabled = true;
        else textPetition.enabled = false;
    }

    /*------dolphin gets bigger------*/
    void AdjustSize () {
        if (countKeyS % 2 != 0) {
            if (this.transform.localScale.x < 3f) {
                Debug.Log ("Adjust size bigger");
                this.transform.localScale += scaleChange;
            }
        } else {
            if (this.transform.localScale.x > 0.3f) {
                Debug.Log ("Adjust size smaller");
                this.transform.localScale -= scaleChange;
            }
        }
    }

    /*------visible tank------*/
    void ShowTank () {
        if (countKeyT % 2 != 0) TankCircle.SetActive (true);
        else TankCircle.SetActive (false);
    }

}