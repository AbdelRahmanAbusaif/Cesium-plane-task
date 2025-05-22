using System;
using System.Text;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UDPListner uDPListner;
    [SerializeField] private CesiumGlobeAnchor anchorPoint;

    private FlightStatus flightStatus = new();
    private Vector3 targetPosition;
    private bool isMoving = false;
    private void Awake()
    {
        uDPListner.onMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(string message)
    {
        Debug.Log($"Received message: {message}");

        // Parce the message into a FlightStatus object from (,,)
        // Remove parentheses and whitespace, then split by comma
        string trimmed = message.Trim('(', ')', ' ', '\n', '\r');

        Debug.Log($"Trimmed message: {trimmed}");

        string[] parts = trimmed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        flightStatus = new FlightStatus
        {
            Mode = int.Parse(parts[0]),
            Latitude = double.Parse(parts[1]),
            Longitude = double.Parse(parts[2]),
            Altitude = float.Parse(parts[3]),
            Roll = float.Parse(parts[4]),
            Pitch = float.Parse(parts[5]),
            Yaw = float.Parse(parts[6]),
            VehicleId = int.Parse(parts[7]),
            HomeLatitude = double.Parse(parts[8]),
            HomeLongitude = double.Parse(parts[9]),
            HomeAltitude = float.Parse(parts[10]),
            Airspeed = float.Parse(parts[11])
        };

        targetPosition = new Vector3(
            (float)flightStatus.Longitude,
            (float)flightStatus.Altitude,
            (float)flightStatus.Latitude
        );
        isMoving = true;
    }
    void Update()
    {
        if (!isMoving)
            return;

        // Move the aircraft towards the target position
        Vector3 currentPosition = new Vector3(
            (float)anchorPoint.longitudeLatitudeHeight.x,
            (float)anchorPoint.longitudeLatitudeHeight.z,
            (float)anchorPoint.longitudeLatitudeHeight.y
        );
        Vector3 direction = targetPosition - currentPosition;
        float distance = direction.magnitude;
        if (distance > 0.1f)
        {
            Debug.Log($"Moving towards target: {targetPosition}, Current position: {currentPosition}");
            // Normalize the direction and move towards the target
            direction.Normalize();
            float speed = flightStatus.Airspeed; // Adjust speed as needed
            Vector3 movement = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 0.01f);
            anchorPoint.longitudeLatitudeHeight = new double3(
                movement.x,
                movement.z,
                movement.y
            );

            Quaternion rotation = Quaternion.Euler(
                flightStatus.Pitch,
                flightStatus.Yaw,
                flightStatus.Roll
            );
            anchorPoint.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
        }
        else
        {
            isMoving = false; // Stop moving when close to the target
        }
    }

    void OnDestroy()
    {
        uDPListner.onMessageReceived -= OnMessageReceived;
    }
}


