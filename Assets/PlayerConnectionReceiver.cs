using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class PlayerConnectionReceiver : MonoBehaviour
{
    private PlayerConnection _playerConnection;
    public bool isConnected;
    public TextMeshProUGUI statusTextField;
    public TextMeshProUGUI outputTextField;

    private void Start()
    {
        _playerConnection = PlayerConnection.instance;
        _playerConnection.Register(MessageTypes.MyCustomMessage, OnMessageReceived);
        outputTextField.text = "Player connection start\n";
        statusTextField.text = "Disconnected";
    }

    private void Update()
    {
        isConnected = _playerConnection.isConnected;
        statusTextField.text = isConnected ? "Connected" : "Disconnected";
    }

    private void OnDestroy()
    {
        _playerConnection.Unregister(MessageTypes.MyCustomMessage, OnMessageReceived);
    }

    private void OnMessageReceived(MessageEventArgs messageArgs)
    {
        var receivedMessage = System.Text.Encoding.ASCII.GetString(messageArgs.data);
        outputTextField.text +=  $"Received message: {receivedMessage}\n";
    }
}

public abstract class MessageTypes
{
    public static readonly Guid MyCustomMessage = new Guid("123e4567-e89b-12d3-a456-426655440000");
}