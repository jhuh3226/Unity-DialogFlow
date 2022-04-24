using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDolphinLocation : MonoBehaviour {
    private CharacterController controller;

    public GameObject arCamera;
    public float moveSpeed = 3f;
    bool moveTowardCamera = false;

    Vector3 targetPoint;

    AudioSource audioSource;

    void Start () {
        // targetPoint = new Vector3 (0, 0, 0.8f);
        controller = GetComponent<CharacterController> ();
        // targetPoint = new Vector3 (arCamera.transform.position.x, arCamera.transform.position.y, arCamera.transform.position.z + 0.8f);
        targetPoint = new Vector3 (0, 0, 0.8f);
        audioSource = GetComponent<AudioSource> ();
    }

    void Update () {
        //Given some means of determining a target point.
        // var targetPoint = FindTargetPoint ();
        // targetPoint = new Vector3 (arCamera.transform.position.x, arCamera.transform.position.y, arCamera.transform.position.z + 0.8f); // camera position always changes
        if (moveTowardCamera) MoveTowardsTarget ();
    }

    public void MoveTowardsTarget () {
        var offset = targetPoint - transform.position;
        var offsetRotation = 180 - transform.localRotation.eulerAngles.y;
        // Get the difference.
        // Debug.Log (offset.magnitude); 
        // when step offset of 0.3, there was a problem but, after changing this to 0.1 it solved the problem (but not an ideal solution)
        // https://answers.unity.com/questions/1135167/step-offset-issue.html

        if (offset.magnitude >.1f) {
            controller.enabled = true;
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * moveSpeed;
            //normalize it and account for movement speed.
            controller.Move (offset * Time.deltaTime);
            // transform.position = new Vector3 (transform.position.x, 0, transform.position.z);   // set this 0 as y positiog keeps becoming 0.035f
            transform.Rotate (0.0f, offsetRotation * Time.deltaTime * (1 / moveSpeed), 0.0f);
            //actually move the character.
        } else {
            controller.enabled = false;
            moveTowardCamera = false;
        }
    }

    public void EnableMoveTowardsTarget () {
        moveTowardCamera = true;
    }

    // public void PlayDolphinAudio () {
    //     audioSource.Play ();
    // }
}