using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePortal : MonoBehaviour {
    public float rotationSpeed;

    void Start () {

    }

    void Update () {

        this.transform.Rotate (Vector3.forward * (rotationSpeed * Time.deltaTime));
    }
}