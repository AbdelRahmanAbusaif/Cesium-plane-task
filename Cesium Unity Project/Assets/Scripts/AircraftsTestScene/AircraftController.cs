using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AircraftController : MonoBehaviour
{
    [Header("Aircraft Stats")]

    [Tooltip("How mush the aircraft can throttle up and down")]
    [SerializeField] private float throttleIncreamant = 0.1f;

    [Tooltip("Maximum speed of the aircraft")]
    [SerializeField] private float maxThrottleSpeed = 100f;

    [Tooltip("How responsive the aircraft is rolling , pitching and yawing")]
    [SerializeField] private float responsiveness = 1f;

    [Tooltip("How fast the aircraft can be lifted up")]
    [SerializeField] private float lift = 100f;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Transform propella;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI throttleText;
    [SerializeField] private TextMeshProUGUI airSpeedText;
    [SerializeField] private TextMeshProUGUI altitudeText;

    private float yaw;
    private float pitch;
    private float roll;
    private float throttle;

    private float ResponsiveModifire => rb.mass / 10f * responsiveness;

    private void Update()
    {
        HandleInput();
        UpdateUI();
        // Rotate the propeller based on the throttle
        if (throttle > 0)
        {
            propella.Rotate(throttle * 10f * Time.deltaTime * Vector3.right);
        }
    }
    private void FixedUpdate()
    {
        // Apply the forces to the aircraft
        rb.AddTorque(new Vector3(pitch, yaw, -roll) * ResponsiveModifire);
        rb.AddForce(Vector3.forward * maxThrottleSpeed * throttle);
        rb.AddForce(lift * rb.linearVelocity.magnitude * Vector3.up); // Lift force based on speed
    }

    private void HandleInput()
    {
        // Use the input actions defined in the Unity Input System asset
        var yawAction = inputActions.actionMaps[0].actions.FirstOrDefault(x => x.name == "Yaw");
        var pitchAction = inputActions.actionMaps[0].actions.FirstOrDefault(x => x.name == "Pitch");
        var rollAction = inputActions.actionMaps[0].actions.FirstOrDefault(x => x.name == "Roll");
        var throttleAction = inputActions.actionMaps[0].actions.FirstOrDefault(x => x.name == "Throttle");

        // If your input actions are Vector2, read their values accordingly
        yaw = yawAction != null ? yawAction.ReadValue<Vector2>().x : 0f;
        pitch = pitchAction != null ? pitchAction.ReadValue<Vector2>().y : 0f;
        roll = rollAction != null ? rollAction.ReadValue<Vector2>().x : 0f;

        float throttleInput = throttleAction != null ? throttleAction.ReadValue<Vector3>().z : 0f;

        if (throttleInput > 0)
            throttle += throttleInput * throttleIncreamant;
        else if (throttleInput <= 0)
            throttle -= throttleIncreamant;

        // Clamp the throttle value to be between 0 and maxThrottleSpeed
        throttle = Mathf.Clamp(throttle, 0f, maxThrottleSpeed);
    }
    private void UpdateUI()
    {
        // Update the UI elements with the current values
        throttleText.text = $"Throttle: {throttle:F0}%";
        airSpeedText.text = $"Air Speed: {rb.linearVelocity.magnitude:F1} m/s";
        altitudeText.text = $"Altitude: {transform.position.y:F1} m";
    }
}
