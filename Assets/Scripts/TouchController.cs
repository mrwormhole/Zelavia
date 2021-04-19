using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    /*public delegate void LevelChecker ();
    public static event LevelChecker levelChecker; //getting subscribed by LevelGenerator.Perfectly fine*/ //BUG FIX

    LevelGenerator levelGeneratorScript; //BUG FIX
    UIManager uiManagerScript;

    bool mouseAllowedTesting = true; //MUST BE false in production 

    void Awake()
    {
        levelGeneratorScript = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        uiManagerScript = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    //FixedUpdate can also work fine btw.For performance you might consider it
    void Update () {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray touchRay = generateSafeTouchRay(touch);
                    RaycastHit2D hit = Physics2D.Raycast(touchRay.origin, touchRay.direction * 10);
                    
                    if (hit.collider != null && hit.collider.tag == "Tile" && UIManager.isTapEnabled())
                    {
                        hit.transform.gameObject.GetComponent<TileScript>().tap(1);
                        levelGeneratorScript.getThenCheckIndicatorsUnlockValues();
                        UIManager.decrementTapCount();
                    }

                    if (UIManager.checkTapCountAndAskForAds() && !UIManager.isShowingTutorial)
                    {
                        uiManagerScript.showAdsPanel();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && mouseAllowedTesting)
        {
            Ray mouseRay = generateSafeMouseRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction * 10);

            if(hit.collider != null && hit.collider.tag == "Tile" && UIManager.isTapEnabled())
            {
                hit.transform.gameObject.GetComponent<TileScript>().tap(1);
                levelGeneratorScript.getThenCheckIndicatorsUnlockValues();
                UIManager.decrementTapCount();
            }
        }
	}

    Ray generateSafeTouchRay(Touch t)
    {
        Vector3 touchPosFar = new Vector3(t.position.x, t.position.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(t.position.x, t.position.y, Camera.main.nearClipPlane);
        Vector3 touchPosFarInWorldUnits = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosNearInWorldUnits = Camera.main.ScreenToWorldPoint(touchPosNear);
        Ray ray = new Ray(touchPosNearInWorldUnits, touchPosFarInWorldUnits - touchPosNearInWorldUnits);
        return ray;
    }

    Ray generateSafeMouseRay(Vector3 v)
    {
        Vector3 mousePosFar = new Vector3(v.x, v.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(v.x, v.y, Camera.main.nearClipPlane);
        Vector3 mousePosFarInWorldUnits = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosNearInWorldUnits = Camera.main.ScreenToWorldPoint(mousePosNear);
        Ray ray = new Ray(mousePosNearInWorldUnits, mousePosFarInWorldUnits - mousePosNearInWorldUnits);
        return ray;
    }
    
}
