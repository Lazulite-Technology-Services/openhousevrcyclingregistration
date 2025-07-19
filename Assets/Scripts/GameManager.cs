using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int PlayerCount = 0;

    public string PlayerDetails = string.Empty;

    public List<string> PlayerDetailsList = new List<string>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        instance = this;
    }

    public void SaveIP(string ip)
    {
        PlayerPrefs.SetString("Socket_Host", "http://" + ip + ":3000/");

        //if (SocketClient.Instance.client != null)
        //{
        //    SocketClient.Instance.client.Dispose();
        //    //SocketClient.Instance.client.DisconnectAsync();
        //}

        SocketClient.Instance.StartConnection();

        Invoke("SendTestMsg", 5f);

    }

    private void SendTestMsg()
    {
        //SocketClient.Instance.SendSocketMessage("test");
    }

    public void SaveUserNameToCSV(string userName)
    {
        // Ensure the StreamingAssets path exists
        string directoryPath = Application.streamingAssetsPath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Define CSV file path
        string filePath = Path.Combine(directoryPath, "UserData.csv");

        // Write header only if file doesn't exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "UserName\n");
        }

        // Append username
        File.AppendAllText(filePath, userName + "\n");

        Debug.Log("Username saved to CSV: " + filePath);

       // ------------------------------------------------------------
       //Read the csv file and then add to the existing data
    }

    
}
