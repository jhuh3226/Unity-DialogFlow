using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script gets the input output of the system(dialogflow)
// Applies that to change of animation
public class DialogFlowSystemOutput : MonoBehaviour {
    public string userInput, previousUserInput = null;
    public string systemOutput, previousSystemOutput = null;

    public GameObject dolphin;
    private Vector3 scaleChange;
    Animator m_Animator;

    // Start is called before the first frame update
    void Start () {
        m_Animator = dolphin.GetComponent<Animator> ();
    }

    void Awake () {
        scaleChange = new Vector3 (3f, 3f, 3f);
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

            if (systemOutput == "Oh no! I may look small now but in fact gigantic. There's the way I can show you, wait!") {
                Debug.Log ("Change the size of the dolphin");

                // dolphin.transform.localScale = new Vector3 (2f, 2f, 2f);
                Invoke("ScaleUp", 6f);
            }

            previousSystemOutput = systemOutput;
        }
    }

    void ScaleUp () {
        if (dolphin.transform.localScale.x < 3f) {
            dolphin.transform.localScale += scaleChange;
        }
    }
}