using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;

public class UDPListner : MonoBehaviour
{
    public Action<string> onMessageReceived;

    [SerializeField] private int port = 12345;
    [SerializeField] private string ipAddress = "127.0.0.1:";

    private UdpClient udpClient;
    private bool listening = false;

    private void Start()
    {
        udpClient = new UdpClient(port);
        listening = true;
        StartCoroutine(ListenCoroutine());
        Debug.Log($"Listening for UDP messages on {ipAddress}{port}");
    }

    private IEnumerator ListenCoroutine()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);

        while (listening)
        {
            if (udpClient.Available > 0)
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                Debug.Log($"Received message: {message} from {remoteEP.Address}:{remoteEP.Port}");
                onMessageReceived?.Invoke(message);
            }
            yield return null; // Wait for next frame
        }
    }

    private void OnDestroy()
    {
        listening = false;
        udpClient?.Close();
    }
}
