using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsManager : MonoBehaviour {

    public CanvasGroup gameHeader;
    public RectTransform menuPanel;
    public RectTransform settingsPanel;
    public CanvasGroup progressPanel;

    public InputField usernameField; 
    public Toggle tutorialToggle; 
    public Toggle timerToggle; 
    public Toggle soundToggle; 

    private const string appID = "com.goldenhand.zelavia";

    void Awake()
    {
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
        gameHeader.DOFade(0.0f, 0.5f);
        menuPanel.DOAnchorPos(new Vector2(-1080, 0), 1.0f);
        progressPanel.DOFade(0.0f, 0.5f);
        settingsPanel.DOAnchorPos(Vector2.zero, 1.00f);


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
        sell gems, dont let them donate
    }*/

    public void onClickHomeButton()
    {
        gameHeader.DOFade(1.0f, 0.5f);
        menuPanel.DOAnchorPos(Vector2.zero, 1.00f);
        progressPanel.DOFade(1.0f, 0.5f);
        settingsPanel.DOAnchorPos(new Vector2(0, -2000), 1.00f);


        if (usernameField.text != PlayerPrefs.GetString("playerUsername"))
        {
            GameObject.FindGameObjectWithTag("LeaderboardsManager").GetComponent<LeaderboardsManager>().deleteYourselfFromLeaderboard();
            PlayerPrefs.SetString("playerUsername", usernameField.text);
            GameObject.FindGameObjectWithTag("LeaderboardsManager").GetComponent<LeaderboardsManager>().uploadYourselfToLeaderboards();
        }
    }

    public static bool translateStringToBool(string s)
    {
        if(s == "False" || s == "false" || s == "FALSE")
        {
            return false;
        }
        return true;  
    }

}
