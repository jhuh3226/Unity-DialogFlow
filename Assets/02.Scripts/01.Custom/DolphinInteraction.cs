using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinInteraction : MonoBehaviour {

    [SerializeField] Animator animator;
    public bool IsJumpHigh, IsJumpSmooth, IsTurn, IsTalk, IsNodeHead, IsDefault = false;
    public bool dolphinComeBack, dolphinSwim, dolphinAdjustSize = false;
    public bool swimTriggered, callTriggered, tankTriggered, sizeTriggered, petitionGuideTriggered, petitionTriggerd; // bool adjusted from DialogFlowSystemOutput
    public float scaleNum = 0.03f;
    public Canvas textPetition, petitionTopics;

    int countTankTrigger, countSizeTrigger, countPetitionGuideTrigger, countPetitionTrigger = 0;
    public GameObject tank, dolphin, portal, eventSystem;

    private Vector3 scaleChange;

    void Start () {
        animator = gameObject.GetComponent<Animator> ();
        tank.SetActive (false);
        swimTriggered = true;
        callTriggered = false;
        tankTriggered = false;
        sizeTriggered = false;
        petitionGuideTriggered = false;
        petitionTriggerd = false;
    }

    void Awake () {
        scaleChange = new Vector3 (scaleNum, scaleNum, scaleNum);
    }

    // Update is called once per frame
    void Update () {
        ControlAnimation ();

        // dolphin swim in circle
        if (swimTriggered) {
            Debug.Log ("swim");
            dolphinSwim = true;
            dolphinComeBack = false;
            dolphinAdjustSize = false;
            GetComponent<BeizerCurve> ().resetOn = true;
            swimTriggered = !swimTriggered;
        }
        // call dolphin
        if (callTriggered) {
            Debug.Log ("call dolphin");
            dolphinComeBack = true;
            dolphinSwim = false;
            dolphinAdjustSize = false;
            callTriggered = !callTriggered;
        }

        // tank
        if (tankTriggered) {
            countTankTrigger++;
            tankTriggered = !tankTriggered;
        }

        // size  
        if (sizeTriggered) {
            countSizeTrigger++;
            dolphinAdjustSize = true;
            sizeTriggered = !sizeTriggered;
        }

        // petition guide
        if (petitionGuideTriggered) {
            countPetitionGuideTrigger++;
            petitionGuideTriggered = !petitionGuideTriggered;
        }

        // petition
        if (petitionTriggerd) {
            countPetitionTrigger++;
            petitionTriggerd = !petitionTriggerd;
        }

        /*-----------------------*/
        if (dolphinSwim) Swim (); // dolphi rotate around beizer curve
        if (dolphinComeBack) Call ();
        ShowTank ();
        if (dolphinAdjustSize) AdjustSize ();
        ShowPetitionGuide ();
        ShowPetition (); // show/hide petition
    }

    /*------call dolphin------*/
    void Call () {
        // Debug.Log ("call dolphin");
        GetComponent<BeizerCurve> ().beizerCurveOn = false;
        GetComponent<ControlDolphinLocation> ().MoveTowardsTarget (); // move dolphin's position to initial starting point
    }

    /*------call dolphin and start rerotating------*/
    void Swim () {
        if (GetComponent<BeizerCurve> ().resetOn) {
            GetComponent<BeizerCurve> ().RotateDesiredAngle ();
        }
    }

    /*------visible tank------*/
    void ShowTank () {
        if (countTankTrigger % 2 != 0) tank.SetActive (true);
        else tank.SetActive (false);
    }

    /*------dolphin gets bigger------*/
    void AdjustSize () {
        Debug.Log (countSizeTrigger);
        if (countSizeTrigger % 2 != 0) {
            if (this.transform.localScale.x < 3f) {
                Debug.Log (this.transform.localScale.x);
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

    /*------petition------*/
    // petition guide
    void ShowPetitionGuide () {
        if (countPetitionGuideTrigger % 2 != 0) petitionTopics.enabled = true;
        else petitionTopics.enabled = false;
    }

    // petition
    void ShowPetition () {
        if (countPetitionTrigger % 2 != 0) textPetition.enabled = true;
        else textPetition.enabled = false;
    }

    /*------dolphin swims away or dissapear------*/
    void DolphinSwimAway () {
        // if (countKeyB % 2 != 0) dolphin.SetActive (false);
        // else dolphin.SetActive (true);
    }

    /*------dolphin animation------*/
    void ControlAnimation () {
        // jump low
        if (IsJumpSmooth) {
            animator.SetBool ("IsJumpSmooth", true);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
            animator.SetBool ("IsDefault", false);
        }

        // jump high
        if (IsJumpHigh) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", true);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
            animator.SetBool ("IsDefault", false);
        }

        // rotate
        if (IsTurn) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", true);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
            animator.SetBool ("IsDefault", false);
        }

        // speak
        if (IsTalk) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", true);
            animator.SetBool ("IsNodHead", false);
            animator.SetBool ("IsDefault", false);
        }

        // shake head
        if (IsNodeHead) {
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", true);
            animator.SetBool ("IsDefault", false);
        }

        // non, just swim
        if (IsDefault) {
            // Debug.Log ("Non, just swim");
            animator.SetBool ("IsJumpSmooth", false);
            animator.SetBool ("IsJumpHigh", false);
            animator.SetBool ("IsTurn", false);
            animator.SetBool ("IsTalk", false);
            animator.SetBool ("IsNodHead", false);
            animator.SetBool ("IsDefault", true);
        }
    }
    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Portal") {
            Debug.Log ("Collider with portal");
            eventSystem.GetComponent<UiController> ().scalePortalOn = true;
            dolphin.SetActive (false);
        }
    }
}