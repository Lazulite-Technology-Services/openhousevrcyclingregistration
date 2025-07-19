using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    public static SocketClient Instance;

    public string key = "lazuGame";

    public SocketIO client;

    private string msg;

    private string SocketHost = "";

    private void Awake() 
    {
          Instance = this;  
    }

    void Start()
    {   
        ConnectHost();
    }

    public void StartConnection()
    {
        ConnectHost();
    }

    async void ConnectHost()
    {
        if(PlayerPrefs.HasKey("Socket_Host"))
        {
            UIManager.instance.LoadIpAddress();
            SocketHost = PlayerPrefs.GetString("Socket_Host");

            client = new SocketIO(SocketHost);
            client.On(this.key, OnLazuGame);
            await client.ConnectAsync();
        }
        else
        {
            UIManager.instance.commandpanel.SetActive(true);
            //SocketHost = "http://127.0.0.1:3000/";
        }

        //client = new SocketIO(SocketHost);
        //client.On(this.key, OnLazuGame);
        //await client.ConnectAsync();
    }

    async void OnDestroy()
    {
        if(client != null)
            await client.DisconnectAsync();
    }

    private void Update()
    {
    }

    private void OnLazuGame(SocketIOResponse response)
    {
        msg = response.GetValue<string>();
        Debug.Log(msg);

        UnityMainThreadDispatcher.Instance().Enqueue(()=>{
            //GameController.Instance.OnSocketMessage(msg);
        });
        
    }

    public void EnterChat()
    {
        // client.EmitAsync("chat message", input.text);
        // input.text = string.Empty;
    }

    public void SendSocketMessage(string message)
    {
        client.EmitAsync(this.key, message);
    }

    public void Clear()
    {
        // text.text = string.Empty;
    }

    public async void SaveSocketHost(string hostAddress)
    {
        PlayerPrefs.SetString("Socket_Host",hostAddress);

        if(client != null && client.Connected)
        {
            await client.DisconnectAsync();
        }

        ConnectHost();

    }
}
