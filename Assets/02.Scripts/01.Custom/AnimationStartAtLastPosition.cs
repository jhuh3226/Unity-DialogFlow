using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartAtLastPosition : MonoBehaviour {
    Animator anim;
    public GameObject dolphinChild;
    public bool isTalk = false;
    bool newPosition = false;

    // Start is called before the first frame update
    void Start () {
        anim = GetComponent<Animator> ();
    }

    void Update () {
        // Debug.Log ("anim position: " + anim.transform.position);
        // Debug.Log ("position: " + dolphinChild.transform.position);

        if (isTalk) {
            AnimationFinished ();
        }
    }

    public void AnimationFinished () {
        // anim.Play ("Talk");
        //  animator.transform.parent.position = animator.transform.position;
        if (!newPosition) transform.position = dolphinChild.transform.position;
        newPosition = true;
        // anim.transform.position = anim.transform.position;
        // anim.transform.localPosition = Vector3.zero;
        anim.SetBool ("isTalk", true);
    }
}