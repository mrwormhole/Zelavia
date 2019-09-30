using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private GameObject IndicatorPrefab;

    private GameObject[] allTiles;
    private GameObject[] allIndicators;
    private IList<bool> allUnlockValuesList = new List<bool>();
    private float randomScale = 0.0f;

    ////////FOR TEST//////////////////////////
    //int sizeTest = 100; //ending point
    //int indexTest = 0; //starting point
    //public int[] allSumOfKeyValuesList; //each level's total indicator key values total
    ////////FOR TEST//////////////////////////
    
    [SerializeField]
    private int indicatorsCount = 0;
    private bool levelCompleted; //is this obsolute ?????

    public delegate void TapCountResetter();
    public static event TapCountResetter tapCountResetter; //getting subscribed by UIManager.Perfectly fine

    //public delegate void ExperienceGiver();
    //public static event ExperienceGiver experienceGiver; //getting subscribed by UIManager.Perfectly fine

    private UIManager uiManagerScript;
    private AchievementManager achievementManagerScript;

    public AudioClip tada;
    public AudioClip poof;

    void Start () {
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0)));
        //mainTestCall();

        GameObject uiManager = GameObject.FindGameObjectWithTag("UIManager");

        uiManagerScript = uiManager.GetComponent<UIManager>();
        achievementManagerScript = uiManager.GetComponent<AchievementManager>();


        if (!SettingsManager.translateStringToBool(PlayerPrefs.GetString("showTutorial")))
        {
            //**
            //FULL = fadeOut + fadeIn
            // ?? uiManagerScript = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            StartCoroutine(goForNextLevel());
        }
        else
        {
            //tutorial
            Debug.Log("Tutorial on progress");
            //**
            //OUT = fadeOut only 
            // ?? uiManagerScript = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            uiManagerScript.showTutorialPanel();
        }
    }
	
    //WE DONT NEED THIS OR DO WE?? O.o
	void FixedUpdate () {
        //IF LEVEL COMPLETED
        // this might not be necessary already

        //Debug.Log("inheritance makes this call?"); inheritance calls here

        //achievementManagerScript.addThem_test(); // q and e adds
        //achievementManagerScript.checkForAchievements();

        if (levelCompleted)
        {
            levelCompleted = false; //is this obsolute ?????
            /*if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("showTimer")))
            {
                uiManagerScript.InvokeRepeating("countTimer", 0.0f, 0.1f);
                Debug.Log("go home you are drunk");
            }*/
            //Debug.Log(PlayerPrefs.GetInt("totalExperience"));
            StartCoroutine(goForNextLevel());
        }
    }

    void destroyAll()
    {
        if (allIndicators != null && allTiles != null)
        {
            foreach (GameObject g in allIndicators)
            {
                if (g != null)
                {
                    Destroy(g);
                }
            }
            foreach (GameObject g in allTiles)
            {
                if (g != null)
                {
                    Destroy(g);
                }
            }
        }
        indicatorsCount = 0;
    }

    void createTilesForBoard(int row,int column)
    {
        int p = 0;
        allTiles = new GameObject[row * column];
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                GameObject temp = Instantiate(TilePrefab, new Vector3(-2.10f + (j * 0.60f), 2.40f - (i * 0.60f), 0.00f), Quaternion.identity) as GameObject;
                allTiles[p] = temp;
                p++;
            }
        }
    }

    void createIndicatorsForTiles(int i,int j)
    {
        int q = 0;
        allIndicators = new GameObject[(i + 1) * (j + 1)];
        for (int m = 0; m < i+1; m++)
        {
            for(int n=0 ;n < j+1; n++)
            {
                if(generateChanceAccordingToDifficulty())
                {
                    GameObject temp = Instantiate(IndicatorPrefab, new Vector3(-2.40f + (n * 0.60f), 2.70f - (m * 0.60f), 0.00f), Quaternion.identity) as GameObject; //1 size
                    allIndicators[q] = temp;
                    q++;
                }
            }
        }
        indicatorsCount = q;
    }

    void startOnIndicators()
    {
        foreach (GameObject g in allIndicators)
        {
            if (g != null)
            {
                g.GetComponent<IndicatorScript>().startIndicator();
            }
        }
    }

    void clearOnTiles()
    {
        foreach(GameObject g in allTiles)
        {
            if( g != null)
            {
                g.GetComponent<TileScript>().clearTile();
            }
        }
    }

    void resetValuesOnIndicators()
    {
        foreach(GameObject g in allIndicators)
        {
            if(g != null)
            {
                g.GetComponent<IndicatorScript>().resetValue();
            }
        }
    }

    public void restartTheSameLevel()
    {
        StartCoroutine(restartTheSameLevelCoroutine());
    }

    IEnumerator restartTheSameLevelCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        if (tapCountResetter != null)
        {
            tapCountResetter();
        }
        yield return new WaitForSeconds(0.05f);
        clearOnTiles();
        yield return new WaitForSeconds(0.05f);
        resetValuesOnIndicators();
    }

    IEnumerator goForNextLevel()
    {
        //Process 1-destroying old ones 2-resetting tap count 3-creating new ones 4-generating random map
        if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
        {
            //MenuManager.audioSource.PlayOneShot(poof); //44100 means 1 sec
        }
        destroyAll();
        yield return new WaitForSeconds(0.05f); //for bug fix
        if (tapCountResetter != null)
        {
            tapCountResetter();
        }
        yield return new WaitForSeconds(0.05f); //for bug fix
        createTilesForBoard(9, 8); //12 looks dope instead of 9
        createIndicatorsForTiles(9, 8);//12 looks dope instead of 9
        playFXonIndicators("smoke");
        yield return new WaitForSeconds(0.05f); //for bug fix
        startOnIndicators();
        clearOnTiles();
    }

    public void getThenCheckIndicatorsUnlockValues()
    {
        StartCoroutine(getThenCheckIndicatorsUnlockValuesCoroutine());
    }

    IEnumerator getThenCheckIndicatorsUnlockValuesCoroutine()
    {
        yield return new WaitForSeconds(0.1f); //for bug fix
        //clear them
        allUnlockValuesList.Clear();

        //get them
        if (allIndicators != null)
        {
            for (int i = 0; i < 10 * 9; i++)
            {
                if (allIndicators[i] != null)
                {
                    allUnlockValuesList.Add(allIndicators[i].GetComponent<IndicatorScript>().isUnlocked());
                }
            }
        }

        /// CONTROL POINT : : : : : : debugFuncForUnlockValues();

        //check them
        for (int i = 0; i < allUnlockValuesList.Count; i++)
        {
            if (allUnlockValuesList[i])
            {
                if (i == allUnlockValuesList.Count - 1)
                {
                    //Debug.Log("ALL TRUE.YOU ARE GONNA GO FOR THE NEXT LEVEL");
                    if (SettingsManager.translateStringToBool(PlayerPrefs.GetString("enableSound")))
                    {
                        //MenuManager.audioSource.PlayOneShot(tada); //44100 means 1 sec
                    }
                    UIManager.tapDisabled = true;
                    uiManagerScript.CancelInvoke(); // it is a stupid animation thingy related to timer
                    makeAllIndicatorsInvisible();
                    Handheld.Vibrate(); // if vibration is enabled via settings do that not like this
                    playFXonIndicators("firework");
                    yield return new WaitForSeconds(1.5f);
                    uiManagerScript.doFadeSplash(1.0f,1.5f);
                    yield return new WaitForSeconds(0.5f);
                    //GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().showProgressPanel();

                    /*uiManagerScript.showProgressPanel();
                    yield return new WaitForSeconds(0.05f);
                    
                    if(experienceGiver != null)
                    {
                        experienceGiver();
                        //?
                    }

                    yield return new WaitForSeconds(0.05f);
                    //GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().giveAnimationPermissionForProgressBar();
                    uiManagerScript.giveAnimationPermissionForProgressPanel();*/

                    uiManagerScript.showProgressPanelTest();
                    
                }
                continue;
            }
            else if (!allUnlockValuesList[i])
            {
                break;
            }
        }
    }

    //utility
    void debugFuncForUnlockValues()
    {
        foreach (var el in allUnlockValuesList)
        {
            Debug.Log(el);
        }
    }

    void playFXonIndicators(string FX)
    {
        //"firework" and "smoke" available
        if(allIndicators != null)
        {
            for (int i = 0; i < 10 * 9; i++)
            {
                if (allIndicators[i] != null)
                {
                    allIndicators[i].GetComponent<IndicatorScript>().playFX(FX);
                }
            }
        }
    }

    void makeAllIndicatorsInvisible()
    {
        if(allIndicators != null)
        {
            for(int i = 0; i < 10 * 9; i++)
            {
                if(allIndicators[i] != null)
                {
                    allIndicators[i].GetComponent<SpriteRenderer>().sprite = null;
                    allIndicators[i].GetComponent<IndicatorScript>().greenlightSpriteRender.sprite = null;
                }
            }
        }
    }

    public void setLevelCompletedStatus(bool status)
    {
        //StartCoroutine(setLevelCompletedStatusCoroutine(status));
        if (status)
        {
            //increaseLevelDifficultyAccordingToProgress();
            uiManagerScript.doFadeSplash(0.0f, 1.5f);
            StartCoroutine(goForNextLevel());
        }
    }

    IEnumerator setLevelCompletedStatusCoroutine(bool status)
    {
        if (status)
        {
            //increaseLevelDifficultyAccordingToProgress();

            uiManagerScript.doFadeSplash(0.0f, 1.5f);

            yield return new WaitForSeconds(0.001f);

            StartCoroutine(goForNextLevel());
        }
    }

    bool generateChanceAccordingToDifficulty()
    {
        int rngGOD = Random.Range(1, 101);
        switch (ProgressManager.getCurrentLevelDifficulty())
        {
            case 1: //EASY LEVEL 10%
                if (rngGOD < 11 + randomScale) { return true; } //testing 10% chance //(M:15.09,SD:5.24) (M:15.16,SD:5.54) (M:14.96,SD:5.81) for 100 samples = 15 6 = 36 => 35
                break;
            case 2: //MEDIUM LEVEL 40%
                if (rngGOD < 41 + randomScale) { return true; } //testing 40% chance //(M:56.21,SD:8.45) (M:58.04,SD:8.89) (M:57.96,SD:9.36) for 100 samples = 58 9 = 125 => 125
                break;
            case 3: //HARD LEVEL 70%
                if (rngGOD < 71 + randomScale) { return true; } //testing 70% chance //(M:100.53,SD:8.80) (M:100.53,SD:8.71) (M:100.75,SD:7.88) for 100 samples = 101  9 => 215
                break;
            default:
                Debug.Log("rngGOD couldn't be set");
                break;
        }
        return false;
    }

    public void increaseCurrentDifficulty()
    {
        randomScale += 0.8f;
    }
    
    //TESTING TOOLS START HERE

    //testing utility tool
    /*void addSumOfAllKeyValues()
    {
        int sum = 0;
        if( allIndicators != null)
        {
            foreach (GameObject g in allIndicators)
            {
                if (g != null)
                {
                    sum += g.GetComponent<IndicatorScript>().returnKeyValue();
                }
            }
            //Debug.Log("Sum of all key values: " + sum);
            allSumOfKeyValuesList[indexTest] = sum;
            indexTest++;
        }
    }

    //testing utility tool
    float calculateMean(int[] array)
    {
        float sum = 0;
        for(int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum / sizeTest;
    }

    //testing utility tool
    float calculateStandardDeviation(int[] array)
    {
        float variance = 0;
        float mean = calculateMean(array);
        for(int i = 0; i < array.Length; i++)
        {
            variance += Mathf.Pow(array[i] - mean,2);
        }
        return Mathf.Sqrt(variance / sizeTest);
    }

    //testing tool
    void testThisManyTimes()
    {
        if(indexTest != sizeTest)
        {
            //collecting datas
            StartCoroutine(testThisManyTimesCo());
        }
        else
        {
            //evaluating datas
            Debug.Log("Testing cases ended.And this is the mean: " + calculateMean(allSumOfKeyValuesList).ToString());
            Debug.Log("Testing cases ended.And this is the strandart deviation: " + calculateStandardDeviation(allSumOfKeyValuesList).ToString());
            //EditorApplication.Beep();
            //51 seconds runtime are required
            CancelInvoke();
        }

    }

    //testing tool
    IEnumerator testThisManyTimesCo()
    {
        setLevelCompletedStatus(true);
        yield return new WaitForSeconds(0.4f); //necesarry for computer evaluation
        addSumOfAllKeyValues();
    }

    //main test call
    void mainTestCall()
    {
        allSumOfKeyValuesList = new int[sizeTest];
        InvokeRepeating("testThisManyTimes", 1.0f, 0.5f);
    }*/
    //TESTING TOOLS END HERE
}
