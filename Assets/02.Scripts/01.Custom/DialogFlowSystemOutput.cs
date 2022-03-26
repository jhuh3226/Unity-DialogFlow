using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script gets the input output of the system(dialogflow)
// Applies that to change of animation
public class DialogFlowSystemOutput : MonoBehaviour {
    public string userInput, previousUserInput = null;

    public string systemOutput, previousSystemOutput = null;

    public GameObject dolphin;
    Animator m_Animator;

    // Start is called before the first frame update
    void Start () {
        m_Animator = dolphin.GetComponent<Animator> ();
    }

    // once the string value changes, check the value
    void Update () {
        if (userInput != previousUserInput) {
            Debug.Log ("new user input detected");
            // if(userInput == ""){
            //     Debug.Log ("Change animation");
            // }

            previousUserInput = userInput;
        }

        if (systemOutput != previousSystemOutput) {
            Debug.Log ("new system output detected");
            if (systemOutput == "Hi Jung, You are testing it out") {
                Debug.Log ("Change animation");

                m_Animator.SetBool ("IsNodHead", true);
            }

            previousSystemOutput = systemOutput;
        }
    }
}