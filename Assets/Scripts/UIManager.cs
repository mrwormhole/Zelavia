using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using DG.Tweening;

public class UIManager : MonoBehaviour {

    private LevelGenerator levelGeneratorScript;

    private static int tapCount = 0;
    [SerializeField]
    public static Text tapCountText;

    public GameObject ProgressPanel;
    public Text titleText;
    public Text levelText;
    public Text experienceText;

    public RectTransform progressBar;   /// main UI element

    public GameObject BonusExperience;
    public GameObject ContinueButton;

    public GameObject TutorialPanel;
    public GameObject OkButton;

    public GameObject AdsPanel;
    public static bool isShowingTutorial;

    public GameObject timer;
    public Text timerText;
    float myTime;

    public static bool tapDisabled = false;

    public Image fadeImage;

    void Awake() {
        //fadeImage.DOFade(0.0f, 1.5f);
        doFadeSplash(0.0f,1.0f);
        LevelGenerator.tapCountResetter += resetTapCount;
        myTime = 0.0f;
        initiateTimer();

    }

    void Start()
    {
        levelGeneratorScript = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        tapCountText = GameObject.FindGameObjectWithTag("EnergyCount").GetComponent<Text>();
    }

    // garbage piece of code here. Because we will remove it in 0.3v
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

    // this is just a horrible func of above func. It will be removed in 0.3v
    void countTimer()
    {
        myTime += 0.1f;
        timerText.text = myTime.ToString("0.0");
    }

    public void doFadeSplash(float alpha,float duration)
    {
        fadeImage.DOFade(alpha, duration);
    }
    

    public void onClickMainMenuButton()
    {
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            //MenuManager.audioSource.Stop(); //44100 means 1 sec
        }
        timerText.text = "";
        SceneManager.LoadScene("Menu");
        tapDisabled = false;
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
            //MenuManager.audioSource.Play(); //44100 means 1 sec
            //Note: Camera can play this music?
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
        if (tapDisabled) //this doesn't seem right
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
        switch(ProgressManager.getCurrentLevelDifficulty())
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

    public void showTutorialPanel()
    {
        isShowingTutorial = true;
        TutorialPanel.SetActive(true);
    }
    
    /*public void showProgressPanel()
    {
        //MenuManager.audioSource.Stop();
        ProgressPanel.SetActive(true);
        int totalExperience = ProgressManager.getTotalExperience();
        titleText.text = ProgressManager.getTitle(ProgressManager.getLevel(totalExperience));
        levelText.text = "LVL\n" + ProgressManager.getLevel(totalExperience).ToString();
        experienceText.text = "Experience\n" + totalExperience.ToString();
        progressBar.localScale = new Vector2(totalExperience % 800,1);
        oldProgressBarScale = progressBar.localScale.x; //?
        oldTotalExp = totalExperience; //?
    }*/

    public void showProgressPanelTest()
    {
        ProgressPanel.SetActive(true);
        int totalExperience = ProgressManager.getTotalExperience();
        byte experienceGained = ProgressManager.getCurrentExperienceGain();
        titleText.text = ProgressManager.getTitle(ProgressManager.getLevel(totalExperience));
        levelText.text = "LVL\n" + ProgressManager.getLevel(totalExperience).ToString();
        experienceText.text = "Experience\n" + totalExperience.ToString();
        progressBar.localScale = new Vector2(totalExperience % 800, 1);
       
        animateProgressBar(progressBar.localScale.x,totalExperience,experienceGained);
        animateTotalExperienceText(totalExperience,experienceGained);

        ProgressManager.setTotalExperience(totalExperience + experienceGained);
    }

    /*public void giveAnimationPermissionForProgressPanel()
    {
        //animate progress bar if it did reach end of it; change title change level then reset it
        if(oldProgressBarScale + ProgressManager.getCurrentExperienceGain() <= 800)
        {
            animationPermissionGivenForOnlyProgressBar = true;
        }
        else if(oldProgressBarScale + ProgressManager.getCurrentExperienceGain() > 800)
        {
            //780 exp gets 20 exp and becomes 800
            //780 exp gets 50 exp and becomes 800 + 30(animate from 780 to 800 then animate from 800 to 30
            //in the end change title and level
            animationPermissionGivenForProgressBarAndLevel = true;
        }
    }*/

    private void animateProgressBar(float from, int totalExp, byte expGain)
    {
        if(from + expGain < 800)
        {
            progressBar.DOScaleX(from + expGain, 1.5f);
        }
        else if(from + expGain >= 800)
        {
            float remaining = from + expGain - 800;
            progressBar.DOScaleX(800, 1.5f).OnComplete(() => {
                progressBar.localScale = new Vector2(0, 1);
                progressBar.DOScaleX(remaining, 1.5f);
                titleText.text = ProgressManager.getTitle(ProgressManager.getLevel(totalExp + expGain));
                levelText.text = "LVL\n" + ProgressManager.getLevel(totalExp + expGain).ToString();
            }); 
        }
        else
        {
            Debug.Log(":@ :@ :@ Animation fatal error!!!");
        }
    }

    private void animateTotalExperienceText(int from, byte expGain)
    {
        DOTween.To(x => experienceText.text = "Experience\n" + Mathf.Floor(x).ToString(), from, from + expGain, 1.6f);
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
