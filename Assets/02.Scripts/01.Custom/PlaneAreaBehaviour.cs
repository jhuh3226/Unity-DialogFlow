using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent (typeof (ARRaycastManager), typeof (AudioSource), typeof (ARPlaneManager))]
public class PlaneAreaBehaviour : MonoBehaviour {
    public TextMeshPro areaText;
    public ARPlane arPlane;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        areaText.transform.rotation =
            Quaternion.LookRotation (areaText.transform.position -
                Camera.main.transform.position);
    }

    private void ArPlane_BoundaryChanged (ARPlaneBoundaryChangedEventArgs obj) {
        areaText.text = CalculatePlaneArea (arPlane).ToString ();
    }
    private float CalculatePlaneArea (ARPlane plane) {
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView () {
        if (areaText.enabled)
            areaText.enabled = false;
        else
            areaText.enabled = true;
    }
}