using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelGenerator : LevelGenerator
{
    
    // Start is called before the first frame update
    void Start()
    {
        ProgressManager.setCurrentLevelDifficulty(1);
        //createTilesForBoard(9, 8);
        StartCoroutine(goForNextLevel());
    }

  
}
