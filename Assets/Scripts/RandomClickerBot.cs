using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClickerBot : MonoBehaviour {

    private float delay = 0.1f;
    private bool startTesting = false;
    GameObject[] allTiles;
    public LevelGenerator levelGeneratorScript;
    public UIManager uiManagerScript;
    private int totalTaps = 0;

	void Start () {
        if (startTesting)
        {
            allTiles = new GameObject[72];
            StartCoroutine(getAllTiles());
            InvokeRepeating("startClicking", 0.6f, delay);
        }
	}
	
    IEnumerator getAllTiles()
    {
        yield return new WaitForSeconds(delay + 0.5f);
        allTiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    void startClicking()
    {
        int randomIndex = (int)(Random.Range(0, 72));
        if (UIManager.isTapEnabled() && allTiles[randomIndex] != null)
        {
            allTiles[randomIndex].GetComponent<TileScript>().tap(1);
            levelGeneratorScript.getThenCheckIndicatorsUnlockValues();
            totalTaps += 1;
        }
    }
}
