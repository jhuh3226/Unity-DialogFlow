using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script gets the input output of the system(dialogflow)
// Applies that to change of animation
public class DialogFlowSystemOutput : MonoBehaviour {
    public string userInput, previousUserInput = null;
    public string systemOutput, previousSystemOutput = null; // systemOutput is the dialogflow's response
    string responseHello, responseName, responseNearestDolphin, responseBeFriended, responseShowTank, responseMessageToShare, responsePetition, responseSwimRound, responseCallDolphin, responseSizeUp, responseSizeDown, responseShowTrickJumpHigh, responseShowTrickTurn, responseShowPlane, responseScannedFloor, responseHidePlane, responsePetitionGuide, responseHidePetitionGuide, responseShowPetition, responsePetitionOutcome, responseHeadOut, responseBye = "";
    string responseNameRecheck, responseLiveClose, responseShowTankRecheck, responseReadyToSeeInRealSize, responseDoFavor, responseWhatDolphinDo, responseAskDolphinShowExperience, responseAskHelpPetition, responseAskIfHaveMessageToShare = "";

    public GameObject dolphin, arSessionOrigin, dfClient, textPetition, textName, eventSystem, firebaseManager;
    private Vector3 scaleChange;
    float area;

    bool playOtherAnimation = false;

    // Start is called before the first frame update
    void Start () { }

    void Awake () {
        scaleChange = new Vector3 (3f, 3f, 3f);
        ChatbotResponseString ();
    }

    // once the string value changes, check the value and respond
    void Update () {
        StringCleaner ();

        // Debug.Log ("systemInput:" + userInput);

        if (userInput != previousUserInput) {
            Debug.Log ("new user input detected");
            previousUserInput = userInput;
        }

        /*------animation controll------*/
        AnimationControl ();

        if (systemOutput != previousSystemOutput) {
            playOtherAnimation = false; // whenever there is new reponse set it as false to enable talk/ default animtion

            Debug.Log ("new system output detected");

            /*------capture name------*/
            if (systemOutput.Contains (responseName)) {
                // Parse only name which is after Hi and before ,
                string[] words = systemOutput.Split (' ');
                int counter = 0;
                foreach (var word in words) {
                    if (counter == 1) {
                        Debug.Log (word);
                        string userName = word;
                        string comma = ",";
                        userName = userName.Replace (comma, "");
                        firebaseManager.GetComponent<FirebaseMananger> ().name = userName; // send name to firebase manager
                        textName.GetComponent<OutputName> ().name = userName; // pass the saved data
                    }
                    counter++;
                }
            }

            /*------initial greeting------*/
            if (systemOutput.Contains (responseHello)) {
                Debug.Log ("Call dolphin");
                dolphin.GetComponent<DolphinInteraction> ().callTriggered = true;
                eventSystem.GetComponent<UiController> ().firstSpeechDetected = true;
            }

            /*-----initial greeting failed but user have tried something----*/
            // if (eventSystem.GetComponent<UiController> ().squeakBtnClickCount == 2) {
            //     dfClient.GetComponent<DF2ClientAudioTester>().SendStartIntent();        // call event
            //     dolphin.GetComponent<DolphinInteraction> ().callTriggered = true;
            // }

            /*------show tank------*/
            if (systemOutput.Contains (responseShowTank)) {
                dolphin.GetComponent<DolphinInteraction> ().tankTriggered = true;
            }

            /*------Swim------*/
            if (systemOutput.Contains (responseSwimRound)) {
                Debug.Log ("Dolphin swim");
                dolphin.GetComponent<DolphinInteraction> ().swimTriggered = true;
            }

            /*------call dolphin------*/
            if (systemOutput.Contains (responseCallDolphin)) {
                Debug.Log ("Call dolphin back");
                dolphin.GetComponent<DolphinInteraction> ().callTriggered = true;
                dolphin.GetComponent<DolphinInteraction> ().tankTriggered = true; // hide tank
            }

            /*------size up------*/
            if (systemOutput.Contains (responseSizeUp) || systemOutput.Contains (responseSizeDown)) {
                Debug.Log ("Dolphin size up");
                dolphin.GetComponent<DolphinInteraction> ().sizeTriggered = true;
            }

            /*------floor dection------*/
            // enable canvas
            if (systemOutput.Contains (responseShowPlane)) {
                Debug.Log ("Enable floor dection");
                arSessionOrigin.GetComponent<PlaneDetectionController> ().TogglePlaneDetection (); // enable plane detection
                GetArea ();
            }

            // send the number
            if (systemOutput.Contains (responseScannedFloor)) {
                Debug.Log ("Pass the number");
            }

            // hide the raycast
            if (systemOutput.Contains (responseHidePlane)) {
                arSessionOrigin.GetComponent<PlaneDetectionController> ().TogglePlaneDetection (); // enable plane detection
            }

            /*------petition------*/
            // petition guide
            if (systemOutput.Contains (responsePetitionGuide)) {
                Debug.Log ("Show petition guide");
                dolphin.GetComponent<DolphinInteraction> ().petitionGuideTriggered = true;
            }

            // hide petition guide
            if (systemOutput.Contains (responseHidePetitionGuide)) {
                Debug.Log ("hide guide");
                dolphin.GetComponent<DolphinInteraction> ().petitionGuideTriggered = true; // hide the UI
            }

            // save pledge string
            if (systemOutput.Contains (responseMessageToShare)) {
                Debug.Log ("Petition: " + dfClient.GetComponent<DF2ClientAudioTester> ().petitionText);
                string myPetition = dfClient.GetComponent<DF2ClientAudioTester> ().petitionText;
                // remove "I'd like to say" from what user said
                string stringToDelete = "i'd like to say";
                myPetition = myPetition.Replace (stringToDelete, "");
                firebaseManager.GetComponent<FirebaseMananger> ().petitionMessage = myPetition; // send petition to firebase
                textPetition.GetComponent<OutputPetition> ().petition = myPetition; // pass the saved data to out put petition
            }

            // show petition
            if (systemOutput.Contains (responseShowPetition)) {
                Debug.Log ("Show petition");
                dolphin.GetComponent<DolphinInteraction> ().petitionTriggerd = true;
                firebaseManager.GetComponent<FirebaseMananger> ().CreateUser (); // push data to firebase
            }

            // dolphin goodbye
            if (systemOutput.Contains (responseBye)) {
                Debug.Log ("Dolphin swims away and dissapear");
                dolphin.GetComponent<DolphinInteraction> ().swimTriggered = true;
                eventSystem.GetComponent<UiController> ().enablePortal = true;
            }

            /*------animation apart from default/talk------*/
            if (systemOutput.Contains (responseNearestDolphin) || systemOutput.Contains (responseShowPetition) || systemOutput.Contains (responseShowTrickJumpHigh) || systemOutput.Contains (responseShowTrickTurn)) {
                Debug.Log ("Contains response");
                playOtherAnimation = true;
            }

            previousSystemOutput = systemOutput;
        }
    }

    // once the user clicks the button, then it sends the size to the dialogflow and play the certain intent?
    void GetArea () {
        var area = arSessionOrigin.GetComponent<PlaneAreaManager> ().area;
        Debug.Log (area);
        dfClient.GetComponent<DF2ClientAudioTester> ().SendResponse (area);
    }

    /*------chatbot's response is showing <speak> and </speak>, get rid of them------*/
    void StringCleaner () {
        string speak = "<speak>";
        string speakEnd = "</speak>";
        systemOutput = systemOutput.Replace (speak, "");
        systemOutput = systemOutput.Replace (speakEnd, "");
    }

    /*------set string that will used as detection phrases to spark dolphin's interaction------*/
    void ChatbotResponseString () {
        responseHello = "Hi stranger";
        responseName = "Great to meet you!"; // capture name
        responseNearestDolphin = "What a coincidence!"; // partial string
        responseBeFriended = "Yay! We are friends!";
        responseSwimRound = "I'm going to swim around";
        responseCallDolphin = "I'm back! How was it?"; // partial string. check if string has folloing string
        responseSizeUp = "I'll show you. Move away!";
        responseSizeDown = "Let me get smaller to talk to you.";
        responseScannedFloor = "I got the number"; // partial string
        responsePetitionGuide = "These are the messages.";
        responseHidePetitionGuide = "Superb! You said";
        responsePetition = "Please start your sentence with";
        responseMessageToShare = "what you want to share with others?";
        responseShowPetition = "It's your message and also your first"; // partial string
        responseShowTrickJumpHigh = "I've been learning these tricks since I was one";
        responseShowTrickTurn = "I have to do difficult tricks like this one";
        responsePetitionOutcome = "Also, the life of aquarium dolphins will change faster";
        responseHeadOut = "Please say goodbye to close the portal";
        responseBye = "See you later";

        // YES or NO trigger replies from chatbot
        responseNameRecheck = "Did I get your name right?";
        responseLiveClose = "because I'm the dolphin living closest to you";
        responseShowTank = "Can you see it?"; // YES OR NO
        responseShowTankRecheck = "Do you see it?";
        responseReadyToSeeInRealSize = "ready for that";
        responseDoFavor = "Can you do something for me";
        responseShowPlane = "Please point your phone towards the ground and scan to measure the area."; // YES OR NO
        responseHidePlane = "Have you ever been to the sea?"; // YES OR NO
        responseWhatDolphinDo = "Do you know what I do at";
        responseAskDolphinShowExperience = "Have you seen any dolphins shows";
        responseAskHelpPetition = "can you help me spread our stories";
        responseAskIfHaveMessageToShare = "Do you have a message you want to share";
    }

    /*------animation control------*/
    void AnimationControl () {
        // default
        if (dfClient.GetComponent<DF2ClientAudioTester> ().dolphinSpeaking == false && !playOtherAnimation) {
            // Debug.Log ("Default animation");
            dolphin.GetComponent<DolphinInteraction> ().IsDefault = true;
            dolphin.GetComponent<DolphinInteraction> ().IsTalk = false;
        }

        // speak
        else if (dfClient.GetComponent<DF2ClientAudioTester> ().dolphinSpeaking == true && !playOtherAnimation) {
            // Debug.Log ("Speak animation");
            dolphin.GetComponent<DolphinInteraction> ().IsTalk = true;
            dolphin.GetComponent<DolphinInteraction> ().IsDefault = false;
        }

        // jump
        else if (systemOutput.Contains (responseNearestDolphin) || systemOutput.Contains (responseShowPetition)) {
            Debug.Log ("Jump smooth animation");
            dolphin.GetComponent<DolphinInteraction> ().IsTalk = false;
            dolphin.GetComponent<DolphinInteraction> ().IsDefault = false;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpSmooth = true;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpHigh = false;
            dolphin.GetComponent<DolphinInteraction> ().IsTurn = false;
        }

        // jump high
        else if (systemOutput.Contains (responseShowTrickJumpHigh)) {
            Debug.Log ("Jump high animation");
            dolphin.GetComponent<DolphinInteraction> ().IsTalk = false;
            dolphin.GetComponent<DolphinInteraction> ().IsDefault = false;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpSmooth = false;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpHigh = true;
            dolphin.GetComponent<DolphinInteraction> ().IsTurn = false;
        }

        // turn
        else if (systemOutput.Contains (responseShowTrickTurn)) {
            Debug.Log ("Jump high animation");
            dolphin.GetComponent<DolphinInteraction> ().IsTalk = false;
            dolphin.GetComponent<DolphinInteraction> ().IsDefault = false;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpSmooth = false;
            dolphin.GetComponent<DolphinInteraction> ().IsJumpHigh = false;
            dolphin.GetComponent<DolphinInteraction> ().IsTurn = true;
        }
    }
}