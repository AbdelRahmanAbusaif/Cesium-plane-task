using DG.Tweening;
using UnityEngine;

public class AircraftsAnimations : MonoBehaviour
{
    [Header("Wings")]
    public Transform LeftWingLower;     // Lower flap on left wing
    public Transform LeftWingLeft;      // Inner flap on left wing
    public Transform RightWingLower;    // Lower flap on right wing
    public Transform RightWingRight;    // Inner flap on right wing

    [Header("Rear Control Surfaces")]
    public Transform LowerWingDirectionX; // Rudder (Yaw)
    public Transform LowerWingDirectionY; // Elevator (Pitch)

    [Header("Aircraft Inputs")]
    [Range(0, 1000)] public float airspeed;
    [Range(-45, 45)] public float pitch; // Nose up/down
    [Range(-45, 45)] public float roll;  // Banking left/right
    [Range(-45, 45)] public float yaw;   // Turning left/right

    void Update()
    {
        AnimateFlaps();
        AnimateRearControl();
    }

    void AnimateFlaps()
    {
        // Ailerons controlled by roll — opposite directions on each side
        float aileronAngle = Mathf.Clamp(roll, -30f, 30f);

        LeftWingLower.localRotation = Quaternion.Euler(-aileronAngle, 0f, 0f);
        LeftWingLeft.localRotation = Quaternion.Euler(-aileronAngle * 0.5f, 0f, 0f);

        RightWingLower.localRotation = Quaternion.Euler(aileronAngle, 0f, 0f);
        RightWingRight.localRotation = Quaternion.Euler(aileronAngle * 0.5f, 0f, 0f);
    }

    void AnimateRearControl()
    {
        // Elevator controlled by pitch
        float elevatorAngle = Mathf.Clamp(pitch, -25f, 25f);
        LowerWingDirectionY.localRotation = Quaternion.Euler(-elevatorAngle, 0f, 0f);

        // Rudder controlled by yaw
        float rudderAngle = Mathf.Clamp(yaw, -25f, 25f);
        LowerWingDirectionX.localRotation = Quaternion.Euler(0f, rudderAngle, 0f);
    }
}
