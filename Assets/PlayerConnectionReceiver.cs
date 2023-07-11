using System;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class PlayerConnectionReceiver : MonoBehaviour
{
    private PlayerConnection _playerConnection;
    public bool isConnected;
    void Start()
    {
        _playerConnection = PlayerConnection.instance;
        _playerConnection.Register(MessageTypes.MyCustomMessage, OnMessageReceived);
    }

    private void Update()
    {
        isConnected = _playerConnection.isConnected;
    }

    void OnDestroy()
    {
        _playerConnection.Unregister(MessageTypes.MyCustomMessage, OnMessageReceived);
    }

    void OnMessageReceived(MessageEventArgs messageArgs)
    {
        string receivedMessage = System.Text.Encoding.ASCII.GetString(messageArgs.data);
        Debug.Log("Received message: " + receivedMessage);
    }
}

public abstract class MessageTypes
{
    public static readonly Guid MyCustomMessage = new Guid("123e4567-e89b-12d3-a456-426655440000");
}