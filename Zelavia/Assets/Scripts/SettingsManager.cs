using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    public GameObject GameHeader;
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    public GameObject ProgressHUD;

    public InputField usernameField; 
    public Toggle tutorialToggle; 
    public Toggle timerToggle; 
    public Toggle soundToggle; 

    private const string appID = "com.goldenhand.zelavia";

    void Awake()
    {
        Text[] textsOfProgressHUD = ProgressHUD.GetComponentsInChildren<Text>();
        textsOfProgressHUD[0].text = "LEVEL\n" + MenuManager.getLevel().ToString();
        textsOfProgressHUD[1].text = MenuManager.getTitle();
        textsOfProgressHUD[2].text = "EXPERIENCE\n" + PlayerPrefs.GetInt("totalExperience").ToString();

        tutorialToggle.isOn = translateStringToBool(PlayerPrefs.GetString("showTutorial"));
        timerToggle.isOn = translateStringToBool(PlayerPrefs.GetString("showTimer"));
        soundToggle.isOn = translateStringToBool(PlayerPrefs.GetString("enableSound"));
        if (tutorialToggle.isOn)
        {
            timerToggle.isOn = false;
        }
    }

    public void onClickSettingsButton()
    {
        GameHeader.SetActive(false);
        MenuPanel.SetActive(false);
        ProgressHUD.SetActive(false);
        SettingsPanel.SetActive(true);

        if(PlayerPrefs.GetString("playerUsername") != "")
        {
            usernameField.text = PlayerPrefs.GetString("playerUsername");
        }
    }

    public void onClickResetButton()
    {
        PlayerPrefs.DeleteKey("totalExperience");
    }

    public void tutorialToggleOnChange(bool value)
    {
        PlayerPrefs.SetString("showTutorial", value.ToString());
    }
    
    public void timerToggleOnChange(bool value)
    {
        PlayerPrefs.SetString("showTimer", value.ToString());
    }

    public void soundToggleOnChange(bool value)
    {
        Debug.Log(value.ToString());
        PlayerPrefs.SetString("enableSound", value.ToString());
    }

    public void onClickRateButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appID);
    }

    /*public void onClickDonateButton()
    {
        //NOT YET. WAIT FOR ADS IMPLEMENTATION
    }*/

    public void onClickHomeButton()
    {
        GameHeader.SetActive(true);
        MenuPanel.SetActive(true);
        ProgressHUD.SetActive(true);
        SettingsPanel.SetActive(false);

        if(usernameField.text != PlayerPrefs.GetString("playerUsername"))
        {
            GameObject.FindGameObjectWithTag("LeaderboardsManager").GetComponent<LeaderboardsManager>().deleteYourselfFromLeaderboard();
            PlayerPrefs.SetString("playerUsername", usernameField.text);
            GameObject.FindGameObjectWithTag("LeaderboardsManager").GetComponent<LeaderboardsManager>().uploadYourselfToLeaderboards();
        }
    }

    public static bool translateStringToBool(string s)
    {
        if(s == "False" || s == "false")
        {
            return false;
        }
        return true;  
    }

}
