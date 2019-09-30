using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class MenuManager : MonoBehaviour {

    private static string[] titles = { "Illiterate Iguana","Arrogant Ant","Lazy Lemure","Nimble Nightingale","Buffoonish Badger", //5 //18NN
                                "Fluffy Fennec","Diligent Duck","Jealous Jellyfish","Cool Cat","Welcoming Walrus", //5
                                "Beige Bettle","Funny Fish","Wailing Whale","Sneaky Sneak","Tempting Toucan", //5
                                "Dank Deer","Calm Camel","Pretty Panda","Delusive Dolphin","Allergic Anaconda", //5
                                "Paranoid Penguin","Affectionate Armadillo","Breathtaking Bunny","Ferocious Feline","Killer Kangaroo", //5
                                "Zealous Zebra","Glorious Gazelle","Fantastic Fox","Blissful Bat","Belligerent Barracuda", //5
                                "Oblivious Octopus","Ubiquitous Unicorn","Pink Panther","Raging Racoon","Loyal Lion", //5
                                "Honorable Hippo","Massive Moose","Enormous Eagle","Barbarous Bear","Dangerous Dinosaur" //5
                               };

    public CanvasGroup gameHeader;
    public RectTransform menuPanel; // -1080
    public RectTransform difficultyPanel; // 1080
    public CanvasGroup progressPanel;

    public Text progressPanelLevel;
    public Text progressPanelTitle;
    public Text progressPanelExperience;

    //public static AudioSource audioSource;
    //public AudioClip theme_music;

    void Awake()
    {
        visualizeProgressPanel();

        menuPanel.DOAnchorPos(Vector2.zero, 1.25f);
        gameHeader.DOFade(1.0f, 1.0f);
        progressPanel.DOFade(1.0f, 1.0f);
    }

    /*private void Start()
    {
        //if someone changes it, CryptographicException is thrown
        //Catch it and restart everything from start
        //string s = SecurityManager.Encrypt(SecurityManager.Serialize<string>("666"));
        //Debug.Log(s);
        //s = "1257";
        //Debug.Log(SecurityManager.Deserialize<string>(SecurityManager.Decrypt(s)));
    }*/

    public void showMenuPanel()
    {
        menuPanel.DOAnchorPos(Vector2.zero, 1.00f);
    }

    public void hideMenuPanel()
    {
        menuPanel.DOAnchorPos(new Vector2(-1080, 0), 1.00f);
    }

    public void showDifficultyPanel()
    {
        difficultyPanel.DOAnchorPos(Vector2.zero, 1.00f);
    }

    public void hideDifficultyPanel()
    {
        difficultyPanel.DOAnchorPos(new Vector2(1080, 0), 1.00f);
    }

    public void visualizeProgressPanel()
    {
        int totalExperience = ProgressManager.getTotalExperience();
        progressPanelLevel.text = "LEVEL\n" + ProgressManager.getLevel(totalExperience).ToString();
        progressPanelTitle.text = ProgressManager.getTitle(ProgressManager.getLevel(totalExperience));
        progressPanelExperience.text = "EXPERIENCE\n" + totalExperience.ToString();
    }

    public void onClickPlayButton()
    {
        hideMenuPanel();
        showDifficultyPanel();
    }

    public void onClickEasyButton()
    {
        ProgressManager.setCurrentLevelDifficulty(1);
        ProgressManager.setCurrentExperienceGain(10);
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            //audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickMediumButton()
    {
        ProgressManager.setCurrentLevelDifficulty(2);
        ProgressManager.setCurrentExperienceGain(50);
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            //audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickHardButton()
    {
        ProgressManager.setCurrentLevelDifficulty(3);
        ProgressManager.setCurrentExperienceGain(250);
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            //audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickBackButton()
    {
        hideDifficultyPanel();
        showMenuPanel();
    }

    // these methods on bottom are not valid to use now
    public static byte getLevelDifficulty()
    {
        return 0;
        //return levelDifficulty;
    }

    public static int getExperienceGain()
    {
        return 0;
        //return experienceGain;
    }

    public static int getLevel()
    {
        return 69;
        return PlayerPrefs.GetInt("totalExperience") / 800 + 1;
    }

    public static string getTitle()
    {
        return "retarded";
        return titles[getLevel() - 1];
    }

    public static int getLevel(int exp)
    {
        return 0;
        //return exp / 800 + 1;
    }

    public static string getTitle(int level)
    {
        return "blob blob";
        //return titles[level - 1];
    }
    // these methods on top are not valid to use now



}
