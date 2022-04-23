using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeizerCurve : MonoBehaviour {
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector3 dolphinPos;

    public float speed, rotationSpeed, delayTime, moveSpeed;
    public bool beizerCurveReset, beizerCurveOn, resetOn;
    Vector3 targetPoint;
    Vector3 beizerPosition;

    private bool coroutineAllowed;

    private void Start () {
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
        beizerCurveReset = false;
        beizerCurveOn = false;
        resetOn = false; // need this seperate bool to refrain from Reset() running multiple times
        targetPoint = new Vector3 (0, -90.0f, 0); // camera position always changes
        beizerPosition = new Vector3 (-1.4f, 0.0f, 2.0f);
    }

    private void FixedUpdate () {
        if (coroutineAllowed) {
            StartCoroutine (GoByTheRoute (routeToGo));
        }

        // when beizer curve reset, 1) dolphin comes back to original position, 2) rotate 90 degree, and rotate following beizer curve
        if (beizerCurveReset) {
            MoveTowardsTarget (); // calls function from control dolphin location script
        }
    }

    private IEnumerator GoByTheRoute (int routeNumber) {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNumber].GetChild (0).localPosition;
        Vector3 p1 = routes[routeNumber].GetChild (1).localPosition;
        Vector3 p2 = routes[routeNumber].GetChild (2).localPosition;
        Vector3 p3 = routes[routeNumber].GetChild (3).localPosition;

        while (tParam < 1 && beizerCurveOn) {
            //move the car only before reaching point3 and stop the car when it reaches point3
            // if (vehiclePassedPoint3 == false) {
            tParam += Time.deltaTime * speed;

            dolphinPos = Mathf.Pow (1 - tParam, 3) * p0 + 3 * Mathf.Pow (1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow (tParam, 2) * p2 + Mathf.Pow (tParam, 3) * p3;
            transform.LookAt (dolphinPos); // make dolphin look at correct position;
            transform.localPosition = dolphinPos;
            // if (tParam > 1) tParam = 0;
            yield return new WaitForEndOfFrame ();
        }
        tParam = 0f;
        routeToGo += 1;
        if (routeToGo > routes.Length - 1) routeToGo = 0;
        coroutineAllowed = true;
    }

    public void MoveTowardsTarget () {
        GetComponent<ControlDolphinLocation> ().MoveTowardsTarget (); // move dolphin's position to initial starting point
        // Debug.Log (transform.position.z);
        if (transform.position.z < 0.9f) {
            RotateDesiredAngle (); // when dolphin comesback, start rotating to the desired angle
        }
        // resetOn = true;
    }

    public void RotateDesiredAngle () {
        Debug.Log ("Rotate to desired angle");
        resetOn = true;
        // Debug.Log (transform.rotation.eulerAngles.y);
        if (transform.rotation.eulerAngles.y < 270) {
            transform.Rotate (0.0f, Time.deltaTime * moveSpeed, 0.0f);
        } else {
            Debug.Log ("Call reset function");
            Reset (); // if the angle reach to desired angle, Reset() to start beizer curve
            // Debug.Log ("tParam: " + tParam);
            // Debug.Log ("routeToGo: " + routeToGo);
        }
    }

    public void Reset () {
        if (resetOn) {
            Debug.Log ("Reset dolphin beizer variables");
            routeToGo = 0;
            tParam = 0f;
            speed = 0.15f;
            transform.eulerAngles = new Vector3 (0f, -90.0f, 0.0f);
            coroutineAllowed = true;
            beizerCurveOn = true;
            beizerCurveReset = false;
            resetOn = false; // need this seperate bool to refrain from Reset() running multiple times
        }
    }

    void RotateDolphin () {
        // transform.Rotate (Vector3.up * (rotationSpeed * Time.deltaTime));

        // transform.rotation = Quaternion.AngleAxis (tiltAngle, direction) * Quaternion.LookRotation (rigidbody.velocity);

        Vector3 dir = beizerPosition - transform.position;
        float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler (0, angle, 0);
    }
}