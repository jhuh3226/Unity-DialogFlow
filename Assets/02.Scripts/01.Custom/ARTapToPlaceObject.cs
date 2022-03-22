using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent (typeof (ARRaycastManager), typeof (AudioSource), typeof (ARPlaneManager))]
public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject bottleNose;
    // [SerializeField]
    private GameObject spawnedObject; // reference of the created object

    /* Raycast control */
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition; // position of touch

    static List<ARRaycastHit> hits = new List<ARRaycastHit> (); // reference of raycast

    private void Awake () {
        _arRaycastManager = GetComponent<ARRaycastManager> ();
        m_ARPlaneManager = GetComponent<ARPlaneManager> (); // toggle plane visibility
    }

    bool TryGetTouchPosition (out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch (0).position;

            // Block UI
            bool isOverUI = touchPosition.IsPointOverUIObject ();

            if (isOverUI) {
                Debug.Log ("touch over UI");
            }
            return true;
        }

        touchPosition = default;
        return false;
    }
    void Start () {

    }

    void Update () {

        if (!TryGetTouchPosition (out Vector2 touchPosition)) return;
        if (_arRaycastManager.Raycast (touchPosition, hits, TrackableType.PlaneWithinPolygon)) {

            var hitPose = hits[0].pose; // get hitpoint
            TogglePlaneDetection ();

            // spawn object ready or not?
            if (spawnedObject == null) {
                spawnedObject = Instantiate (bottleNose, hitPose.position, hitPose.rotation);

            } else {
                // Debug.Log ("moving the position");
                spawnedObject.transform.position = hitPose.position;
                // spawnedObject.transform.rotation = hitPose.rotation;

            }
        }
    }

    /* Toggles plane detection and the visualization of the planes. */
    void TogglePlaneDetection () {
        // enable the following code if you want to raycast guide to be gone, but then you may not be available to interact with the instantiated model
        // m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;        

        if (m_ARPlaneManager.enabled) {
            SetAllPlanesActive (true);
        } else {
            SetAllPlanesActive (false);
        }
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    void SetAllPlanesActive (bool value) {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive (value);
    }

    ARPlaneManager m_ARPlaneManager;
}