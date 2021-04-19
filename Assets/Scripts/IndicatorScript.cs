using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour {

    public Sprite[] animalCircles;
    private SpriteRenderer indicatorSpriteRenderer;
    private TextMesh textMesh; //for text mesh pro to display text
    private MeshRenderer meshRenderer; //for text mesh pro to set sorting layer
    [SerializeField]
    private byte keyValue;
    [SerializeField]
    private byte value = 0;
    [SerializeField]
    private bool unlocked = false;

    [SerializeField]
    public SpriteRenderer greenlightSpriteRender;

    public GameObject smokeFX; //find a sound for this
    public GameObject fireworkFX; //find a sound for this

    void Start () {
        textMesh = GetComponentInChildren<TextMesh>();
        indicatorSpriteRenderer = GetComponent<SpriteRenderer>();

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.sortingLayerName = "Puzzle";
    }

    public void startIndicator()
    {
        keyValue = value;
        textMesh.text = keyValue.ToString();
        value = 0;
        if (keyValue == 0)
        {
            indicatorSpriteRenderer.sprite = animalCircles[0];
            greenlightSpriteRender.color = new Color32(255,255,255,255);
            textMesh.text = null;
            unlocked = true;
        }
        else if (keyValue == 1)
        {
            indicatorSpriteRenderer.sprite = animalCircles[1];
            value++; //bug fix
        }
        else if (keyValue == 2)
        {
            indicatorSpriteRenderer.sprite = animalCircles[2];
            value += 2; //bug fix
        }
        else if (keyValue == 3)
        {
            indicatorSpriteRenderer.sprite = animalCircles[3];
            value += 3; //bug fix
        }
        else if (keyValue == 4)
        {
            indicatorSpriteRenderer.sprite = animalCircles[4];
            value += 4; //bug fix
        }
    }

    public void keyValueCheck()
    {
        if(value == keyValue)
        {
            indicatorSpriteRenderer.color = new Color32(255, 255, 255, 255);
            greenlightSpriteRender.color = new Color32(255, 255, 255, 255);
            textMesh.text = null;
            unlocked = true;
        }
        else
        {
            indicatorSpriteRenderer.color = new Color32(255, 255, 255, 120);
            greenlightSpriteRender.color = new Color32(255, 255, 255, 0);
            textMesh.text = keyValue.ToString();
            unlocked = false;
        }
    }

    public void resetValue()
    {
        value = 0;
        keyValueCheck();
    }

    //utility
    public bool isUnlocked()
    {
        return unlocked;
    }

    //utility
    public byte returnKeyValue()
    {
        return keyValue;
    }

    public void playFX(string FX)
    {
        if(FX == "smoke")
        {
            //find and play sound here
            Instantiate(smokeFX, gameObject.transform.position, Quaternion.identity);
        }
        else if(FX == "firework")
        {
            //find and play sound here
            Instantiate(fireworkFX, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("FX not found");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Line")
        {
            value += 1;
            //Debug.Log("hello there");
            keyValueCheck();
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Line")
        {
            value -= 1;
            //Debug.Log("bye bye");
            keyValueCheck();
        }
    }
}
