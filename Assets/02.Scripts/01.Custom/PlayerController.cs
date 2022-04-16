using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController controller;

    [SerializeField] private Rigidbody rigidbody;   // joystick
    [SerializeField] private FixedJoystick joystick;    // joystick
    [SerializeField] private Animator _animator;    // joystick

    Vector3 direction;
    bool up, down, stay = false;

    [SerializeField] private float tiltAngle = 10;

    public float playerSpeed = 0.5f;

    public float turnSmoothTime = 0.5f; // smoothing turn
    float turnSmoothVelocity;

    private void Start () {
        // controller = GetComponent<CharacterController> ();
    }

    void FixedUpdate () {
        // keyboard control
        // activate character controller to use it
        float horizontal = Input.GetAxisRaw ("Horizontal");
        float vertical = Input.GetAxisRaw ("Vertical");
        Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smoothing number
            transform.rotation = Quaternion.Euler (0f, angle, 0f);
            controller.Move (direction * playerSpeed * Time.deltaTime);
        }

        // joystick control
        // deactivate character controller to use it
        if (Input.GetKeyDown (KeyCode.D)) {
            up = true;
            stay = false;
            down = false;
        } else if (Input.GetKeyDown (KeyCode.S)) {
            up = false;
            stay = true;
            down = false;
            direction = Vector3.zero;
        } else if (Input.GetKeyDown (KeyCode.A)) {
            up = false;
            stay = false;
            down = true;
        }

        if (up == true) {
            if ((joystick.Horizontal * 100 > 0 && joystick.Horizontal * 100 < 100) && (joystick.Vertical * 100 > 0 && joystick.Vertical * 100 < 100)) {
                // Debug.Log ("x+ y+  Vector3.back");
                direction = Vector3.back;
            } else if ((joystick.Horizontal * 100 > 0 && joystick.Horizontal * 100 < 100) && (joystick.Vertical * 100 < 0 && joystick.Vertical * 100 > -100)) {
                // Debug.Log ("x+ y- Vector3.left");
                direction = Vector3.left;
            } else if ((joystick.Horizontal * 100 < 0 && joystick.Horizontal * 100 > -100) && (joystick.Vertical * 100 > 0 && joystick.Vertical * 100 < 100)) {
                // Debug.Log ("x- y+ Vector3.right");
                direction = Vector3.right;
            } else if ((joystick.Horizontal * 100 < 0 && joystick.Horizontal * 100 > -100) && (joystick.Vertical * 100 < 0 && joystick.Vertical * 100 > -100)) {
                // Debug.Log ("x- y- Vector3.forward");
                direction = Vector3.forward;
            }
        } else if (down == true) {
            if ((joystick.Horizontal * 100 > 0 && joystick.Horizontal * 100 < 100) && (joystick.Vertical * 100 > 0 && joystick.Vertical * 100 < 100)) {
                direction = Vector3.forward;
            } else if ((joystick.Horizontal * 100 > 0 && joystick.Horizontal * 100 < 100) && (joystick.Vertical * 100 < 0 && joystick.Vertical * 100 > -100)) {
                direction = Vector3.right;
            } else if ((joystick.Horizontal * 100 < 0 && joystick.Horizontal * 100 > -100) && (joystick.Vertical * 100 > 0 && joystick.Vertical * 100 < 100)) {
                direction = Vector3.left;
            } else if ((joystick.Horizontal * 100 < 0 && joystick.Horizontal * 100 > -100) && (joystick.Vertical * 100 < 0 && joystick.Vertical * 100 > -100)) {
                direction = Vector3.back;
            }
        }

        // joystick
        rigidbody.velocity = new Vector3 (joystick.Horizontal * playerSpeed, rigidbody.velocity.y, joystick.Vertical * playerSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0) {
            // transform.rotation = Quaternion.LookRotation (rigidbody.velocity);
            // transform.rotation = Quaternion.LookRotation (rigidbody.velocity) * Quaternion.Euler (0, 0, 10f);

            // if it is Vector3.right, when gameobject is facing front, it goes down/ when facing back, it goes up
            // if it is Vector3.left, when gameobject is facing front, it goes up/ when facing back, it goes down
            // if it is Vector3.forward, when gameobject is facing right, it goes down/ when facing left, it goes up
            // if it is Vector3.back, when gameobject is facing right, it goes up/ when facing left, it goes down

            transform.rotation = Quaternion.AngleAxis (tiltAngle, direction) * Quaternion.LookRotation (rigidbody.velocity);
        }

        // joystick2
        // Vector2 input = playerInput.actions["Move"].ReadValue<Vector2> ();
        // Vector3 direction = new Vector3 (input.x, 0f, input.y).normalized;
    }
}