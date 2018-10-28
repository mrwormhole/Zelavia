using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class UIManager : MonoBehaviour {

    private LevelGenerator levelGeneratorScript;

    private static int tapCount = 0;
    [SerializeField]
    public static Text tapCountText;

    public GameObject ProgressPanel;
    public Text titleText;
    public Text levelText;
    public Text experienceText;
    public RectTransform progressBar;
    public GameObject BonusExperience;
    public GameObject ContinueButton;

    public GameObject TutorialPanel;
    public GameObject OkButton;

    public GameObject AdsPanel;
    public static bool isShowingTutorial;

    public GameObject timer;
    public Text timerText;
    float myTime;

    public static float oldProgressBarScale; 
    public static int oldTotalExp; 
    static float t = 0.0f;
    bool animationPermissionGivenForOnlyProgressBar = false;
    bool animationPermissionGivenForProgressBarAndLevel= false;
    public static bool tapDisabled = false;

    void Awake() { 
        LevelGenerator.tapCountResetter += resetTapCount;
        LevelGenerator.experienceGiver += addAndSaveExperience;
        myTime = 0.0f;
        initiateTimer();
    }

    void FixedUpdate()
    {
        if(animationPermissionGivenForOnlyProgressBar && ProgressPanel.activeSelf)
        {
            t += 0.5f * Time.deltaTime;
            experienceText.text = "Experience\n" + Mathf.Floor((Mathf.Lerp(oldTotalExp, PlayerPrefs.GetInt("totalExperience"), t))).ToString();
            progressBar.localScale = new Vector3(Mathf.Lerp(oldProgressBarScale, oldProgressBarScale + MenuManager.getExperienceGain(), t), 1, 0);
            if(t > 1.0f)
            {
                t = 0.0f;
                animationPermissionGivenForOnlyProgressBar = false;
            }
        }
        if(animationPermissionGivenForProgressBarAndLevel && ProgressPanel.activeSelf)
        {
            t += 0.5f * Time.deltaTime;
            experienceText.text = "Experience\n" + Mathf.Floor((Mathf.Lerp(oldTotalExp, PlayerPrefs.GetInt("totalExperience"), t))).ToString();
            if (t < 0.5f)
            {
                //50 + 850  = 50 but this should give 100
                progressBar.localScale = new Vector3(Mathf.Lerp(oldProgressBarScale, 800, t * 2), 1, 0);
                if (progressBar.localScale.x >= 800)
                {
                    progressBar.localScale -= new Vector3(800, 0, 0);
                };
            }
            else if(t > 0.5f)
            {
                progressBar.localScale = new Vector3(Mathf.Lerp(0,oldProgressBarScale + MenuManager.getExperienceGain()-800, (t - 0.5f) * 2), 1, 0);
            }
          
            if (t > 1.0f)
            {
                t = 0.0f;
                titleText.text = MenuManager.getTitle();
                levelText.text = "LVL\n" + MenuManager.getLevel().ToString();
                animationPermissionGivenForProgressBarAndLevel = false;
            }
        }  
    }

    void Start () {
        levelGeneratorScript = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        tapCountText = GameObject.FindGameObjectWithTag("EnergyCount").GetComponent<Text>();
    }

    void initiateTimer()
    {
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("showTimer")) && !SettingsManager.translateStringToBool(PlayerPrefs.GetString("showTutorial")))
        {
            CancelInvoke();
            myTime = 0.0f;
            timer.SetActive(true);
            InvokeRepeating("countTimer", 0.0f, 0.1f);
        }
    }

    void countTimer()
    {
        myTime += 0.1f;
        timerText.text = myTime.ToString("0.0");
    }
    

    public void onClickMainMenuButton()
    {
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            MenuManager.audioSource.Stop(); //44100 means 1 sec
        }
        timerText.text = "";
        SceneManager.LoadScene("Menu");
        tapDisabled = false;
        GameObject.Find("MenuManager").GetComponent<MenuManager>().getMainMenuComponents();
    }

    /*//we will use this on ads menu for asking player
    public void onClickRestartButton()
    {
        //playFadeAnimation();
        levelGeneratorScript.restartTheSameLevel();
    }

    //this is for development.It won't be included in the release
    public void onClickPassLevelButton()
    {
        //playFadeAnimation();
        levelGeneratorScript.setLevelCompletedStatus(true);
    }*/

    public void onClickContinueButton()
    {
        initiateTimer();
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            MenuManager.audioSource.Play(); //44100 means 1 sec
        }
        ProgressPanel.SetActive(false);
        tapDisabled = false;
        levelGeneratorScript.setLevelCompletedStatus(true);
        levelGeneratorScript.increaseCurrentDifficulty();
    }

    public void onClickOkButton()
    {
        TutorialPanel.SetActive(false);
        PlayerPrefs.SetString("showTutorial","false");
        isShowingTutorial = false;
        levelGeneratorScript.setLevelCompletedStatus(true);
    }

    public static void decrementTapCount()
    {
        tapCount -= 1;
        tapCountText.text = tapCount.ToString();
    }

    public static bool checkTapCountAndAskForAds()
    {
        if(tapCount == 0)
        {
            return true;
        }
        return false;
    }

    public static bool isTapEnabled()
    {
        if (tapDisabled)
        {
            return false;
        }
        if(tapCount > 0)
        {
            return true;
        }
        return false;
    }

    void resetTapCount()
    {
        switch(MenuManager.getLevelDifficulty())
        {
            case 1: //EASY
                tapCount = 35;
            break;
            case 2: //MEDIUM
                tapCount = 125;
            break;
            case 3: //HARD
                tapCount = 215;
            break;
            default:
                Debug.Log("tapCount couldn't be set");
            break;
        }
        tapCountText.text = tapCount.ToString();
    }

    void addAndSaveExperience()
    {
        PlayerPrefs.SetInt("totalExperience", oldTotalExp + MenuManager.getExperienceGain());
    }

    public void showTutorialPanel()
    {
        isShowingTutorial = true;
        TutorialPanel.SetActive(true);
    }
    
    public void showProgressPanel()
    {
        MenuManager.audioSource.Stop();
        ProgressPanel.SetActive(true);
        titleText.text = MenuManager.getTitle();
        levelText.text = "LVL\n" + MenuManager.getLevel().ToString();
        experienceText.text = "Experience\n" + PlayerPrefs.GetInt("totalExperience").ToString();
        progressBar.localScale = new Vector3(PlayerPrefs.GetInt("totalExperience") % 800,1,0);
        oldProgressBarScale = progressBar.localScale.x;
        oldTotalExp = PlayerPrefs.GetInt("totalExperience");
    }

    public void giveAnimationPermissionForProgressPanel()
    {
        //animate progress bar if it did reach end of it; change title change level then reset it
        if(oldProgressBarScale + MenuManager.getExperienceGain() <= 800)
        {
            animationPermissionGivenForOnlyProgressBar = true;
        }
        else if(oldProgressBarScale + MenuManager.getExperienceGain() > 800)
        {
            //780 exp gets 20 exp and becomes 800
            //780 exp gets 50 exp and becomes 800 + 30(animate from 780 to 800 then animate from 800 to 30
            //in the end change title and level
            animationPermissionGivenForProgressBarAndLevel = true;
        }
    }

    //ADS SECTION STARTS HERE
    public void showAdsPanel()
    {
        AdsPanel.SetActive(true);
    }

    public  void showAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = handleAdsResult});
        }
    }

    private void handleAdsResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                tapCount += 25;
                tapCountText.text = tapCount.ToString();
                timer.SetActive(false);
                AdsPanel.SetActive(false);
                break;
            case ShowResult.Skipped:
                Debug.Log("skipped");
                timer.SetActive(false);
                AdsPanel.SetActive(false);
                break;
            case ShowResult.Failed:
                Debug.Log("failed");
                timer.SetActive(false);
                AdsPanel.SetActive(false);
                break;
        }
    }
    //ADS SECTION ENDS HERE

}
