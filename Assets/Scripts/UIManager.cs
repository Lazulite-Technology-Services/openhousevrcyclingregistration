using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using Unity.VisualScripting;
using System;
using System.Net;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private Button startButton, singlePlayerButton, multiplayerButton, homeButton, skipButton, submitButton, saveButton;

    [SerializeField]
    private bool isSingle = false;

    [SerializeField]
    private TMP_InputField playerName, ipAddress;

    [SerializeField]
    private VideoPlayer bgVidePlayer;

    [SerializeField]
    private GameObject StartPage, RegistrationPage, player_1_Title, player_2_Title;
    public GameObject commandpanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        instance = this;

        startButton.onClick.AddListener(() => EnableNextScreen(1));

        singlePlayerButton.onClick.AddListener(() => isSingle = true);
        singlePlayerButton.onClick.AddListener(() => EnableNextScreen(2));

        multiplayerButton.onClick.AddListener(() => isSingle = false);
        multiplayerButton.onClick.AddListener(() => EnableNextScreen(2));

        submitButton.onClick.AddListener(() => SubmitRegistration(false));
        skipButton.onClick.AddListener(() => SubmitRegistration(true));
        homeButton.onClick.AddListener(Home);

        saveButton.onClick.AddListener(SendIP);

        if (!bgVidePlayer.isPlaying)
        {
            bgVidePlayer.Play();
        }

        playerName.text = string.Empty;
        StartPage.SetActive(true);
    }

    private void EnableNextScreen(int index)
    {
        switch(index)
        {
            case 0: //HOME

                startButton.gameObject.SetActive(true);
                singlePlayerButton.gameObject.SetActive(false);
                multiplayerButton.gameObject.SetActive(false);

                break;
            case 1: //Mode Selection - Single or Multiplayer

                startButton.gameObject.SetActive(false);
                singlePlayerButton.gameObject.SetActive(true);
                multiplayerButton.gameObject.SetActive(true);

                break;
            case 2: //Registration

                startButton.gameObject.SetActive(true);
                StartPage.SetActive(false);
                player_1_Title.SetActive(true);               

                RegistrationPage.SetActive(true);

                break;
            default:
                break;
        }
    }

    private void Home()
    {
        Reset();
    }

    private void SubmitRegistration(bool isskip)
    {      
        //if(!isskip)
        ////Save the user details
        //GameManager.instance.SaveUserNameToCSV(playerName.text);
        //else
        //    GameManager.instance.SaveUserNameToCSV("na");

        GameManager.instance.PlayerCount++;

        if (playerName.text != string.Empty)
        {
            GameManager.instance.PlayerDetailsList.Add($"{playerName.text}");
        }
        else
        {
            GameManager.instance.PlayerDetailsList.Add($"p{GameManager.instance.PlayerCount}skip");
        }
        playerName.text = string.Empty;

        Debug.Log(GameManager.instance.PlayerDetailsList[GameManager.instance.PlayerCount - 1]);

        if (isSingle)
        {           

            GameManager.instance.PlayerDetails = GameManager.instance.PlayerDetailsList[0];

            //Send game start command
            SocketClient.Instance.SendSocketMessage(GameManager.instance.PlayerDetails);

            Invoke("Reset", 5f);

            submitButton.interactable = false;
            skipButton.interactable = false;
            homeButton.interactable = false;

            //Debug.Log($"Game Start SinglePlayer : {isSingle} : player details : {GameManager.instance.PlayerDetails}");
            return;
        }
        else
        {
            player_1_Title.SetActive(false);
            player_2_Title.SetActive(true);

            if (GameManager.instance.PlayerCount == 2)
            {
                GameManager.instance.PlayerDetails = $"{GameManager.instance.PlayerDetailsList[0]}-{GameManager.instance.PlayerDetailsList[1]}";

                submitButton.interactable = false;
                skipButton.interactable = false;
                homeButton.interactable = false;

                Invoke("Reset", 5f);

                //Send game start command
                SocketClient.Instance.SendSocketMessage(GameManager.instance.PlayerDetails);

                Debug.Log($"Game Start is SinglePlayer : {isSingle}");
                
            }
        }

        //Debug.Log($"Game Start SinglePlayer : {isSingle} : player details : {GameManager.instance.PlayerDetails}");
    }

    private void SendIP()
    {
        if(ipAddress.text != string.Empty)
            GameManager.instance.SaveIP(ipAddress.text);
        else
        {
            Debug.Log("Ip address Input field is empty");
        }
    }

    public void LoadIpAddress()
    {
        ipAddress.text = string.Empty;
        ipAddress.text = PlayerPrefs.GetString("Socket_Host");
    }

    private void Reset()
    {
        isSingle = true;
        GameManager.instance.PlayerCount = 0;
        startButton.gameObject.SetActive(true);
        StartPage.SetActive(true);
        singlePlayerButton.gameObject.SetActive(false);
        multiplayerButton.gameObject.SetActive(false);

        player_1_Title.SetActive(false);
        player_2_Title.SetActive(false);

        RegistrationPage.SetActive(false);

        submitButton.interactable = true;
        skipButton.interactable = true;
        homeButton.interactable = true;

        GameManager.instance.PlayerDetailsList.Clear();
    }
}
