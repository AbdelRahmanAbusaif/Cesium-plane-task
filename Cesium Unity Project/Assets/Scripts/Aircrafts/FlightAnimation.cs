using System;
using UnityEngine;

public class FlightAnimation : MonoBehaviour
{
    [Header("Propeller")]
    [SerializeField] private Transform propeller;
    [SerializeField] private float propellerRotationSpeed = 100f;

    private void Update()
    {
        propeller.Rotate(propellerRotationSpeed * Time.deltaTime * Vector3.forward);    
    }
}
