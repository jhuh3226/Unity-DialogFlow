using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public CharacterController controller;
    // public PlayerController playerInput;

    public FixedJoystick joystick; // joystick
    public float playerSpeed = 0.5f;

    public float turnSmoothTime = 0.5f; // smoothing turn
    float turnSmoothVelocity;

    private void Start () {
        // controller = GetComponent<CharacterController> ();
        // playerInput = GetComponent<PlayerInput> ();
    }

    void Update () {
        // keyboard
        float horizontal = Input.GetAxisRaw ("Horizontal");
        float vertical = Input.GetAxisRaw ("Vertical");
        Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;

        // joystick
        // Vector2 input = playerInput.actions["Move"].ReadValue<Vector2> ();
        // Vector3 direction = new Vector3 (input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smoothing number
            transform.rotation = Quaternion.Euler (0f, angle, 0f);
            controller.Move (direction * playerSpeed * Time.deltaTime);
        }
    }
}