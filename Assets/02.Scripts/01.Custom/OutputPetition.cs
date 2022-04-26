using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class OutputPetition : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject dfClient;
    public string petition;
    TextInfo textInfo;      // https://docs.microsoft.com/en-us/dotnet/api/system.globalization.textinfo.totitlecase?view=net-6.0
    void Start () {
        textInfo = new CultureInfo ("en-US", false).TextInfo;

        petition = "Shout out your first petition!";
    }

    // get the petition data and upload the string
    void Update () {
        if (textMeshPro.text != null) OutputPetitionText ();
    }

    /* get voice input and show it in the petition textbox
    Call event 
    Bot: are you open to sharing our stories to your friends?
    User: yes!
    Bot: instruction
    User: click the button and say something
    Petition shows up
    */
    public void OutputPetitionText () {
        // petition = dfClient.GetComponent<DF2ClientAudioTester> ().petitionText;
        textMeshPro.text = textInfo.ToTitleCase (petition);
    }
}