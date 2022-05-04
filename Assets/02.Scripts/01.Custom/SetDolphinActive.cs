using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDolphinActive : MonoBehaviour {
    public GameObject dolphin, btnPortal, arCamera;
    // Start is called before the first frame update
    void Start () {
        dolphin.SetActive (false);
    }

    // Update is called once per frame
    void Update () {

    }

    // capture camera location and rotation
    public void ActivateDolphin () {
        dolphin.SetActive (true);
        btnPortal.SetActive (false);
        dolphin.transform.position = new Vector3 ((dolphin.transform.position.x + arCamera.transform.position.x), (dolphin.transform.position.y + arCamera.transform.position.y), dolphin.transform.position.z);
        dolphin.transform.Rotate (0.0f, arCamera.transform.localEulerAngles.y, 0.0f, Space.World);
        Debug.Log ("ar camera location" + arCamera.transform.position);
        // Debug.Log ("ar camera rotation" + arCamera.eulerAngles.x + arCamera.eulerAngles.y + arCamera.eulerAngles.z);
        // localEulerAngles
        Debug.Log ($"ar camera rotation: {arCamera.transform.localEulerAngles.x}, {arCamera.transform.localEulerAngles.y}, {arCamera.transform.localEulerAngles.z}");
        Debug.Log ("dolphin location" + dolphin.transform.position);
    }
}