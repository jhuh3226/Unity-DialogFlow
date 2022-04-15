using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// [RequireComponent (typeof (ARRaycastManager), typeof (AudioSource), typeof (ARPlaneManager))]
[RequireComponent (typeof (ARRaycastManager))]
public class PlaneAreaManager : MonoBehaviour {
    // Start is called before the first frame update

    public float area = 0;
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch (0);
            if (touch.phase == TouchPhase.Ended) {
                if (Input.touchCount == 1) {
                    // Debug.Log ("touch count 1");
                    Ray raycast = Camera.main.ScreenPointToRay (touch.position);
                    if (Physics.Raycast (raycast, out RaycastHit raycastHit)) {
                        // Debug.Log ("raycast hit");
                        var planeAreaBehaviour = raycastHit.collider.gameObject.GetComponent<PlaneAreaBehaviour> ();
                        // Debug.Log (planeAreaBehaviour);
                        if (planeAreaBehaviour != null) {
                            Debug.Log ("communicate with PlaneAreaBehaviour");
                            planeAreaBehaviour.ArPlane_AskCalculation ();
                            area = planeAreaBehaviour.area;
                            Debug.Log (area);
                            planeAreaBehaviour.ToggleAreaView ();
                        }
                    }
                }
            }
        }
    }
}