using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class Achievement
{
    public Sprite image;
    public string header;
    public string description;
}

public class AchievementManager : MonoBehaviour {

    private short levelReached; // 0000 0000 0000 0000 -> level 1,2,3,4,5,8,10,15,20,25,30,35,40,45,50,55
    private byte energyUsed; // 0000 0000  -> energy 25,100,500,1000,2500,5000,10000,25000
    private byte specialMissionsCompleted; // 0000 0000 -> 8 special missions
    private byte puzzlesCompleted; // 0000 0000 0000 0000 -> puzzles 1,5,10,15,20,30,40,50
    private bool tutorialCompleted;

    public RectTransform achievementPanel;
    public Image achievementPanelImage;
    public Text achievementPanelHeader;
    public Text achievementPanelDescription;

    public Achievement[] achievements;
    private Queue<Achievement> achievementsQueue = new Queue<Achievement>();
    private static bool achievementAnimationPlaying = false;

    void Start () {
        
    }

    public bool isLevelReached(int index)
    {
        return (levelReached & (1 << index)) != 0;
    }
    
    public bool isEnergyUsed(int index)
    {
        return (energyUsed & (1 << index)) != 0;
    }

    public bool isSpecialMissionsCompleted(int index)
    {
        return (specialMissionsCompleted & (1 << index)) != 0;
    }

    public bool isPuzzlesCompleted(int index)
    {
        return (puzzlesCompleted & (1 << index)) != 0;
    }

    public void setLevelReached(int index)
    {
        levelReached |= (short)(1 << index);
    } 

    public void setEnergyUsed(int index)
    {
        energyUsed |= (byte)(1 << index);
    }

    public void setSpecialMissionsCompleted(int index)
    {
        specialMissionsCompleted |= (byte)(1 << index);
    }

    public void setPuzzlesCompleted(int index)
    {
        puzzlesCompleted |= (byte)(1 << index);
    }

    public bool isTutorialCompleted()
    {
        return tutorialCompleted;
    }

    public void setTutorialCompleted(bool state)
    {
        tutorialCompleted = state;
    }

    private void showAchievementPanel(Achievement a)
    {
        achievementAnimationPlaying = true;
        fillAchievementPanel(a);
        achievementPanel.DOAnchorPos(new Vector2(-1.5f, -700), 0.75f, true).OnComplete(() =>
        {
            StartCoroutine(hideAchievementPanel());
        });
    }

    private IEnumerator hideAchievementPanel()
    {
        yield return new WaitForSeconds(1.0f);
        achievementPanel.DOAnchorPos(new Vector2(1.5f, -1200), 0.75f, true).OnComplete(() =>
        {
            cleanAchievementPanel();
            achievementAnimationPlaying = false;
        });
    }

    private void fillAchievementPanel(Achievement a)
    {
        achievementPanelImage.sprite = a.image;
        achievementPanelHeader.text = a.header;
        achievementPanelDescription.text = a.description;
    }

    private void cleanAchievementPanel()
    {
        achievementPanelImage.sprite = null;
        achievementPanelHeader.text = null;
        achievementPanelDescription.text = null;
    }

    public void addThem_test()
    {
        //straight add test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            achievementsQueue.Enqueue(achievements[0]);
            achievementsQueue.Enqueue(achievements[1]);
        }
        //reverse add test
        if (Input.GetKeyDown(KeyCode.E))
        {
            achievementsQueue.Enqueue(achievements[1]);
            achievementsQueue.Enqueue(achievements[0]);
        }

        //tutorial add test
        if (Input.GetKeyDown(KeyCode.T))
        {
            achievementsQueue.Enqueue(achievements[0]);
        }
    }

    public void checkForAchievements()
    {
        if(achievementsQueue.Count > 0)
        {
            if(achievementsQueue.Peek() != null && !achievementAnimationPlaying)
            {
                showAchievementPanel(achievementsQueue.Peek());
                achievementsQueue.Dequeue();
            }
        }
    }

	
}
