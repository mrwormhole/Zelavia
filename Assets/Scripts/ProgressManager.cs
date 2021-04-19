using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour { 

    public static ProgressManager instance { get; private set; }
    private static byte currentLevelDifficulty; //1 means easy 2 means medium 3 means hard.
    private static byte currentExperienceGain; //10 for Easy, 50 for Medium, 250 for Hard.

    private static string[] titles = { "Illiterate Iguana","Arrogant Ant","Lazy Lemure","Nimble Nightingale","Buffoonish Badger", //5 //18NN
                                "Fluffy Fennec","Diligent Duck","Jealous Jellyfish","Cool Cat","Welcoming Walrus", //5
                                "Beige Bettle","Funny Fish","Wailing Whale","Sneaky Sneak","Tempting Toucan", //5
                                "Dank Deer","Calm Camel","Pretty Panda","Delusive Dolphin","Allergic Anaconda", //5
                                "Paranoid Penguin","Affectionate Armadillo","Breathtaking Bunny","Ferocious Feline","Killer Kangaroo", //5
                                "Zealous Zebra","Glorious Gazelle","Fantastic Fox","Blissful Bat","Belligerent Barracuda", //5
                                "Oblivious Octopus","Ubiquitous Unicorn","Pink Panther","Raging Racoon","Loyal Lion", //5
                                "Honorable Hippo","Massive Moose","Enormous Eagle","Barbarous Bear","Dangerous Dinosaur" //5
                               };


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //audioSource = this.GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static byte getCurrentLevelDifficulty()
    {
        //what will be the level difficulty in selected game
        return currentLevelDifficulty;
    }

    public static byte getCurrentExperienceGain()
    {
        //what will be the experience gain in selected game
        return currentExperienceGain;
    }

    public static void setCurrentLevelDifficulty(byte levelDiff)
    {
        currentLevelDifficulty = levelDiff;
    }

    public static void setCurrentExperienceGain(byte expGain)
    {
        currentExperienceGain = expGain;
    }

    public static int getTotalExperience()
    {
        return PlayerPrefs.GetInt("totalExperience");
    }

    public static void setTotalExperience(int value)
    {
        PlayerPrefs.SetInt("totalExperience", value);
    }

    public static int getLevel()
    {
        //loads from memory
        return getTotalExperience() / 800 + 1;
    }

    public static int getLevel(int exp)
    {
        //calculation performs without memory load overhead
        return exp / 800 + 1;
    }

    public static string getTitle()
    {
        //loads from memory
        int level = getLevel();
        if(level > titles.Length)
        {
            return titles[titles.Length - 1];
        }
        return titles[level - 1];
    }

    public static string getTitle(int level)
    {
        //calculation performs without memory load overhead
        if (level > titles.Length)
        {
            return titles[titles.Length -1 ];
        }
        return titles[level - 1];
    }

  
}
