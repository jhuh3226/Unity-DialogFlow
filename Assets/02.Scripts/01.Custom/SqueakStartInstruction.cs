using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueakStartInstruction : MonoBehaviour {
    public GameObject instruction0, instruction1, instruction2, btnGuideNext, btnGuideBefore;
    int counter = 0;
    public int length;
    int instruction = 0;
    int squeakBtnClickCount = 0;
    void Start () {
        instruction0.SetActive (true);
        instruction1.SetActive (false);
        instruction2.SetActive (false);
        length = 50;

        btnGuideNext.SetActive (true);
        btnGuideBefore.SetActive (false);
    }

    void Update () {
        if (squeakBtnClickCount == 0) {
            Debug.Log ("Run ShowGuideText0");
            instruction0.SetActive (true);
            instruction1.SetActive (false);
            instruction2.SetActive (false);
            btnGuideNext.SetActive (true);
            btnGuideBefore.SetActive (false);

        } else if (squeakBtnClickCount == 1) {
            instruction0.SetActive (false);
            instruction1.SetActive (true);
            instruction2.SetActive (false);
            btnGuideNext.SetActive (true);
            btnGuideBefore.SetActive (true);
        } else if (squeakBtnClickCount == 2) {
            instruction0.SetActive (false);
            instruction1.SetActive (false);
            instruction2.SetActive (true);
            btnGuideNext.SetActive (false);
            btnGuideBefore.SetActive (true);
        }
        /*
        counter++;
        // Debug.Log (counter);
        // change the mode
        if (counter % length == 0) {
            instruction++;
            instruction = instruction % 3;
        }

        if (instruction == 0) {
            instruction0.SetActive (true);
            instruction1.SetActive (false);
            instruction2.SetActive (false);
        } else if (instruction == 1) {
            instruction0.SetActive (false);
            instruction1.SetActive (true);
            instruction2.SetActive (false);
        } else if (instruction == 2) {
            instruction0.SetActive (false);
            instruction1.SetActive (false);
            instruction2.SetActive (true);
        }
        */
    }

    public void ShowNextGuide () {
        squeakBtnClickCount++;
    }

    public void ShowPreviousGuide () {
        squeakBtnClickCount--;
    }

    // public void ShowGuideText2 () {
    //     instruction0.SetActive (false);
    //     instruction1.SetActive (false);
    //     instruction2.SetActive (true);
    //     btnGuideNext.SetActive (false);
    //     btnGuideBefore.SetActive (true);
    // }
}