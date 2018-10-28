using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance { get; private set; }

    private static string[] titles = { "Illiterate Iguana","Arrogant Ant","Lazy Lemure","Nimble Nightingale","Buffoonish Badger", //5 //18NN
                                "Fluffy Fennec","Diligent Duck","Jealous Jellyfish","Cool Cat","Welcoming Walrus", //5
                                "Beige Bettle","Funny Fish","Wailing Whale","Sneaky Sneak","Tempting Toucan", //5
                                "Dank Deer","Calm Camel","Pretty Panda","Delusive Dolphin","Allergic Anaconda", //5
                                "Paranoid Penguin","Affectionate Armadillo","Breathtaking Bunny","Ferocious Feline","Killer Kangaroo", //5
                                "Zealous Zebra","Glorious Gazelle","Fantastic Fox","Blissful Bat","Belligerent Barracuda", //5
                                "Oblivious Octopus","Ubiquitous Unicorn","Pink Panther","Raging Racoon","Loyal Lion", //5
                                "Honorable Hippo","Massive Moose","Enormous Eagle","Barbarous Bear","Dangerous Dinosaur" //5
                               };

    public GameObject MenuPanel;
    public GameObject DifficultyPanel;
    public GameObject FadeScreen;

    static float t = 0.0f;
    bool permissionForDifficultyPanelAnim = false;
    static bool permissionForMenuButtonsAnim = false;

    private static byte levelDifficulty; //1 means easy 2 means medium 3 means hard.
    private static int experienceGain;  //10 for Easy, 50 for Medium, 250 for Hard. 

    public static AudioSource audioSource;
    public AudioClip theme_music;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LevelGenerator.fadeAnimator += playFadeAnimation;
            audioSource = this.GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void onClickPlayButton()
    {
        t = 0.0f;
        MenuPanel.SetActive(false);
        permissionForDifficultyPanelAnim = true;
    }

    public void onClickEasyButton()
    {
        levelDifficulty = 1;
        experienceGain = 10;
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickMediumButton()
    {
        levelDifficulty = 2;
        experienceGain = 50;
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickHardButton()
    {
        levelDifficulty = 3;
        experienceGain = 250;
        SceneManager.LoadScene("Gameplay");
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            audioSource.Play(); //44100 means 1 sec
        }
    }

    public void onClickBackButton()
    {
        t = 0.0f;
        DifficultyPanel.SetActive(false);
        permissionForMenuButtonsAnim = true;
    }

    public static byte getLevelDifficulty()
    {
        return levelDifficulty;
    }

    public static int getExperienceGain()
    {
        return experienceGain;
    }

    public static int getLevel()
    {
        return PlayerPrefs.GetInt("totalExperience") / 800 + 1;
    }

    public static string getTitle()
    {
        return titles[getLevel() - 1];
    }

    public static int getLevel(int exp)
    {
        //calculation for non local data
        return exp / 800 + 1;
    }

    public static string getTitle(int level)
    {
        //calculation for non local data
        return titles[level - 1];
    }

    void FixedUpdate () {
        if (permissionForDifficultyPanelAnim && !MenuPanel.activeSelf)
        {
            playAnimationForDifficultyPanel();
        }
        if (permissionForMenuButtonsAnim && !DifficultyPanel.activeSelf)
        {
            playAnimationForMenuButtons();
        }
    }

    void playAnimationForDifficultyPanel()
    {
        t += 2.5f * Time.deltaTime;
        DifficultyPanel.SetActive(true);
        MenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(1000, 0, 0);
        DifficultyPanel.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(1000, 0, t), 0, 0);
        if (t > 1.0f)
        {
            t = 0.0f;
            permissionForDifficultyPanelAnim = false;
        }
    }

    void playAnimationForMenuButtons()
    {
        t += 2.5f * Time.deltaTime;
        MenuPanel.SetActive(true);
        MenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-1000, 0, 0);
        MenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(-1000,0, t), 0, 0);
        
        if (t > 1.0f)
        {
            t = 0.0f;
            permissionForMenuButtonsAnim = false;
        }
    }

    public void playFadeAnimation(string option)
    {
        //FULL = fadeOut + fadeIn, IN = fadeIn only, OUT = fadeOut only 
        FadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        if(FadeScreen != null && gameObject != null)
        {
            StartCoroutine(fadeCoroutine(option));
        }
    }

    IEnumerator fadeCoroutine(string option)
    {
        if(option == "FULL")
        {
            FadeScreen.GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(0.1f);
            FadeScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        }
        else if(option == "OUT")
        {
            FadeScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        }
        else if(option == "IN")
        {
            FadeScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        }
    }

    public void getMainMenuComponents()
    {
        StartCoroutine(getMainMenuComponentsCoroutine());
    }

    IEnumerator getMainMenuComponentsCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        MenuPanel = GameObject.FindGameObjectWithTag("MenuPanel");
        DifficultyPanel = GameObject.FindGameObjectWithTag("DifficultyPanel");
        Button playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        yield return new WaitForSeconds(0.05f);
        Button[] fourDifficultyPanelButtons = DifficultyPanel.GetComponentsInChildren<Button>();
        yield return new WaitForSeconds(0.05f);
        playButton.onClick.AddListener(onClickPlayButton);
        fourDifficultyPanelButtons[0].onClick.AddListener(onClickEasyButton);
        fourDifficultyPanelButtons[1].onClick.AddListener(onClickMediumButton);
        fourDifficultyPanelButtons[2].onClick.AddListener(onClickHardButton);
        fourDifficultyPanelButtons[3].onClick.AddListener(onClickBackButton);
    }
}
