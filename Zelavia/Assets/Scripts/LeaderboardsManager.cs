using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsManager : MonoBehaviour {

    public GameObject GameHeader;
    public GameObject MenuPanel;
    public GameObject LeaderboardsPanel;
    public GameObject ProgressHUD;
    public GameObject Warning;

    private const string publicDomain = "5bce257d613a89132c0ae241";
    private const string privateDomain = "Y_X9HKdpcE2Vz1lFZAggOgpU4eYWDYE0GvddCTtVD0sQ";
    private const string webURL = "http://dreamlo.com/lb/";

    Player[] allTopPlayers;
    public Text[] allTopPlayersText;
    public Text[] allTopPlayersDetailsText; //includes level and title

    void Awake()
    {
        uploadYourselfToLeaderboards();
        getAllPlayersData();
    }

    public void onClickLeaderboardsButton()
    {
        uploadYourselfToLeaderboards();
        getAllPlayersData();
        GameHeader.SetActive(false);
        MenuPanel.SetActive(false);
        ProgressHUD.SetActive(false);
        LeaderboardsPanel.SetActive(true);
    }

    public void onClickHomeButton()
    {
        GameHeader.SetActive(true);
        MenuPanel.SetActive(true);
        ProgressHUD.SetActive(true); 
        LeaderboardsPanel.SetActive(false);
    }

    private void uploadNewPlayerData(string username,int experience)
    {
        StartCoroutine(uploadNewPlayerDataCoroutine(username, experience));
    }

    IEnumerator uploadNewPlayerDataCoroutine(string username, int experience)
    {
        WWW www = new WWW(webURL + privateDomain + "/add/" + WWW.EscapeURL(username) + "/" + experience);
        yield return www;

        if (string.IsNullOrEmpty(www.error) && PlayerPrefs.GetString("playerUsername") != "")
        {
            Debug.Log("Upload successful");
            Warning.SetActive(false);
        }
        else if(PlayerPrefs.GetString("playerUsername") == "")
        {
            Debug.Log("Empy name");
            Warning.SetActive(true);
        }
        else
        {
            Debug.Log("Error while uploading: " + www.error); 
            yield return new WaitForSeconds(5);
            uploadNewPlayerData(username, experience);
        }
    }

    private void deleteOldPlayerData(string username)
    {
        StartCoroutine(deleteOldPlayerDataCoroutine(username));
    }

    IEnumerator deleteOldPlayerDataCoroutine(string username)
    {
        WWW www = new WWW(webURL + privateDomain + "/delete/" + WWW.EscapeURL(username));
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Deletion successful");
        }
        else
        {
            Debug.Log("Error while deleting: " + www.error);
            yield return new WaitForSeconds(5);
            deleteOldPlayerData(username);
        }
    }

    ///USER METHOD
    public void deleteYourselfFromLeaderboard()
    {
        deleteOldPlayerData(PlayerPrefs.GetString("playerUsername"));
    }

    ///USER METHOD
    public void uploadYourselfToLeaderboards()
    {
        uploadNewPlayerData(PlayerPrefs.GetString("playerUsername"), PlayerPrefs.GetInt("totalExperience"));
    }

    public void getAllPlayersData()
    {
        StartCoroutine(getAllPlayersDataCoroutine());
    }

    IEnumerator getAllPlayersDataCoroutine()
    {
        //currently we support only top 10. Remember maximum we get is top 1000
        //currently we support only all of the times. Think about All of the times - Monthly - Weekly - Daily
        WWW www = new WWW(webURL + publicDomain + "/pipe/0/10");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Download successful"); 
            formatPipeDataForHighScores(www.text);
        }
        else
        {
            Debug.Log("Error while downloading: " + www.error);
            yield return new WaitForSeconds(5);
            getAllPlayersData();
        }
    }

    void formatPipeDataForHighScores(string data)
    {
        string[] entries = data.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        allTopPlayers = new Player[10];

        for(int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int experience = int.Parse(entryInfo[1]);
            allTopPlayers[i] = new Player(username, experience);
            displayDatasOnTexts();
        }
    }

    void displayDatasOnTexts()
    {
        for(int i = 0;i < 10; i++)
        {
            if (allTopPlayers[i].username != null) 
            {
                allTopPlayersText[i].text = i + 1 + ". " + allTopPlayers[i].username;
                allTopPlayersDetailsText[i].text = allTopPlayers[i].level + "          " + allTopPlayers[i].title;
            }
            else
            {
                allTopPlayersText[i].text = "";
                allTopPlayersDetailsText[i].text = "";
            }
        }
    }
}

struct Player
{
    public string username;
    public int experience;
    public int level;
    public string title;

    public Player(string username,int experience)
    {
        this.username = username;
        this.experience = experience;
        level = MenuManager.getLevel(experience);
        title = MenuManager.getTitle(level);
    } 
}
