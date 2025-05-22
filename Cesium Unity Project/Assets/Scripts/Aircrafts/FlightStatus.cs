[System.Serializable]
public class FlightStatus
{
    public int Mode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public float Altitude { get; set; }
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
    public int VehicleId { get; set; }
    public double HomeLatitude { get; set; }
    public double HomeLongitude { get; set; }
    public float HomeAltitude { get; set; }
    public float Airspeed { get; set; }
}
