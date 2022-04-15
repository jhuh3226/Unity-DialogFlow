using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

// ARTrackableManager is the sealed class, the script is trying to override the class
// show only one raycast
namespace UnityEngine.XR.ARFoundation {

    [RequireComponent (typeof (ARRaycastManager), typeof (ARPlaneManager))]

    public class ShowSingleRaycast : MonoBehaviour {

        private ARRaycastManager arRaycastManager;
        private static List<ARRaycastHit> hits = new List<ARRaycastHit> ();

        bool placementPoseIsValid = false;

        void Start () {
            arRaycastManager = GetComponent<ARRaycastManager> ();
        }

        void Update () {
            SinglePlaneDetection ();
            // UpdatePlacementPose ();
        }

        public void SinglePlaneDetection () {
            // Debug.Log ("hits " + hits);
            // Debug.Log ("pose " + hits.Count);
            // Debug.Log ("pose " + hits[0].pose);

            // var screenCenter = Camera.current.ViewportToScreenPoint (new Vector3 (0.5f, 0.5f));

            // https://forum.unity.com/threads/arfoundation-remove-all-trackables.932769/
            // var hasHit = arRaycastManager.Raycast (screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            // Debug.Log ("has hit: " + hasHit);

            // if (hasHit) {
                // var plane = arRaycastManager.GetPlane (hits.trackableId);
                // var index = hits.FindIndex (_ => arRaycastManager.GetPlane (_.trackableId)?.gameObject.activeSelf ?? false);
                // if (index != -1) {
                //     var hit = hits[index];
                //     // do something with hit
                // }
            // }
        }

        // https://stackoverflow.com/questions/57498983/reset-unity-ar-session-to-find-trackable-planes-on-ios
        private void UpdatePlacementPose () {
            var screenCenter = Camera.current.ViewportToScreenPoint (new Vector3 (0.5f, 0.5f));
            // var camera = GetComponent<ARSessionOrigin> ().camera;

            var hits = new List<ARRaycastHit> ();
            arRaycastManager.Raycast (screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid) {
                var placementPose = hits[0].pose;

                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3 (cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation (cameraBearing);
            }
        }
    }
}