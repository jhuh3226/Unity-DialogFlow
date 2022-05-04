using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkRecordingStatus : MonoBehaviour {
    public GameObject blink;
    public float interval;
    void Start () {
        StartBlink ();
    }

    // Update is called once per frame
    void Update () { }

    void StartBlink () {
        InvokeRepeating ("ToggleRecordingStatusImg", 0, interval);
    }

    public void ToggleRecordingStatusImg () {
        if(blink.activeSelf) blink.SetActive(false);
        else if(!blink.activeSelf) blink.SetActive(true);
    }
}