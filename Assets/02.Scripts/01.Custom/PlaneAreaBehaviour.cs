using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent (typeof (ARRaycastManager), typeof (AudioSource), typeof (ARPlaneManager))]
public class PlaneAreaBehaviour : MonoBehaviour {
    public TextMeshPro areaText;
    public ARPlane arPlane;

    // Start is called before the first frame update
    void Start () {
        Debug.Log ("PlaneAreaBehaviour initialized");
    }

    private void Update () {
        // Set the areaText gameobject transform to always look at the MainCamera
        areaText.transform.rotation = Quaternion.LookRotation (areaText.transform.position - Camera.main.transform.position);
    }

    private void ArPlane_BoundaryChanged (ARPlaneBoundaryChangedEventArgs obj) {
        Debug.Log ("ArPlane_BoundaryChanged");
        areaText.text = CalculatePlaneArea (arPlane).ToString ();
    }
    private float CalculatePlaneArea (ARPlane plane) {
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView () {
        areaText.enabled = true;

        // if (areaText.enabled) {
        //     areaText.enabled = false;
        // } else
        //     areaText.enabled = true;
    }

    public void ArPlane_AskCalculation () {
        Debug.Log ("Ask for calculation");
        areaText.text = CalculatePlaneArea (arPlane).ToString ();
    }
}