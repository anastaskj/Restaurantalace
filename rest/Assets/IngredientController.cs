using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    public Sprite defaultBorder;
    public Sprite selectedBorder;

    private SpriteRenderer sr;
    public bool selected = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }
        
    public void ClearItem()
    {
        foreach (Transform child in transform)
        {
           Destroy(child.gameObject);
           break;
        }
    }

    public void SelectItem()
    {
        if (!selected)
        {
            sr.sprite = selectedBorder;
            AddItem();
            selected = true;
        }
        else
        {
            sr.sprite = defaultBorder;
            RemoveItem();
            selected = false;
        }

        AudioManager.instance.PlaySound("Select");
    }

    public void ResetBorder()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultBorder;

        selected = false;
    }

    private void AddItem()
    {
        foreach (Transform child in transform)
        {
            GameManager.playerSelectedIngredients.Add(child.gameObject);
            break;
        }
    }

    private void RemoveItem()
    {
        foreach (Transform child in transform)
        {
            foreach (GameObject i in GameManager.playerSelectedIngredients)
            {
                if (i.name == child.name)
                {
                    GameManager.playerSelectedIngredients.Remove(child.gameObject);
                    GameManager.playerSelectedIngredients.RemoveAll(item => item == null);
                    break;
                }
            }
        }
       
    }
}
