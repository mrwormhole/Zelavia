using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
 
    bool firstTouch = true;
    public Sprite[] squareSprites;
    public SpriteRenderer squareSpriteRenderer;
    public Sprite[] lineSprites;
    public SpriteRenderer lineSpriteRenderer;
    public Transform lineTransfrom;
    private EdgeCollider2D lineEdgeCollider;

    void Start () {
        lineEdgeCollider = GetComponentInChildren<EdgeCollider2D>();

        //squareSpriteRenderer.sprite = squareSprites[2];
        byte randomNum = (byte)Random.Range(0, 6);
        lineSpriteRenderer.sprite = lineSprites[randomNum];
        tapRandomlyAsInvisible();
    }

    public void tapRandomlyAsInvisible()
    {
        byte randomTap = (byte)Random.Range(1, 3);
        if (randomTap % 2 == 1)
        {
            tap(0);
        }
        else if (randomTap % 2 == 0)
        {
            tap(0);
            tap(0);
        }
    }

    public void tap(byte m)
    {
        //m means mode.Creation mode for 0.Play mode for 1.
        if (firstTouch)
        {
            lineEdgeCollider.enabled = true;
            if(m == 1)
            {
                lineSpriteRenderer.enabled = true;
            }
            lineTransfrom.Rotate(new Vector3(0, 0, 180.0f)); //FOR BUG FIX
            //lineTransfrom.Rotate(new Vector3(0, 0, 90.0f)); //FOR BUG FIX
            //lineTransfrom.Rotate(new Vector3(0, 0, 90.0f)); //FOR BUG FIX
            firstTouch = false;
        }
        else if (!firstTouch)
        {
            lineTransfrom.Rotate(new Vector3(0, 0, 90.0f));
        }
        
    }

    public void clearTile()
    {
        lineSpriteRenderer.enabled = false;
        lineEdgeCollider.enabled = false;
        lineTransfrom.transform.rotation = Quaternion.Euler(0, 0, 0);
        firstTouch = true;
    }

}
