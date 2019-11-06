using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCooker : MonoBehaviour
{
    public int xpMultiplier = 2;
    public int xpPerCorrect = 20;
    public XP xpScript;
    private GameManager gm;
    private int xpGain = 0;
    private int timesGuessed = 0;


    private void Start()
    {
        gm = GetComponent<GameManager>();
    }

    public int CompareRecipeUnordered()
    {
        bool valid = false;
        int correctRecipe = 0;
        foreach (Recipe r in gm.recipesClass) //every recipe of the day
        {
            if (GameManager.playerSelectedIngredients.Count == r.GetIngredients().Count) //check the ingredient quantity
            {
                valid = CompareLists(r.GetIngredients(), GameManager.playerSelectedIngredients);
                if (valid)
                {
                    correctRecipe++;
                    removeRecipe();
                    break;
                }
            }
        }
        return correctRecipe;
    }

    private void removeRecipe() //used to reset the recipes for the day so you cant get more points from one recipe
    {
        foreach (Recipe r in gm.recipesClass)
        {
            if (CompareLists(r.GetIngredients(), GameManager.playerSelectedIngredients))
            {
                gm.recipesClass.Remove(r);
                break;
            }
        }
    }

    private bool CompareLists(List<GameObject> list1, List<GameObject> list2)
    {
        int count = 0;
        if (list1.Count != list2.Count)
        {
            return false;
        }
        for (int i = 0; i < list1.Count; i++)
        {
            for (int j = 0; j < list2.Count; j++)
            {
                if (list1[i].tag == list2[j].tag)
                {
                    count++;
                }
            }
        }
        if (count == list1.Count)
        {
            return true;
        }
        return false;
    }


    public void Cook()
    {
        timesGuessed++;
        
        //indicate how many tries the player has left

        int correct = CompareRecipeUnordered();
        if (correct == 1)
        {
            AudioManager.instance.PlaySound("RightRecipes");
        }
        else
        {
            AudioManager.instance.PlaySound("WrongRecipes");
        }
        xpGain += (correct * xpMultiplier * xpPerCorrect);
        GameManager.playerSelectedIngredients = new List<GameObject>();
        ResetBorderSlots();

        if (timesGuessed >= gm.recipesClass.Count)
        {
            

            if (xpGain > 0)
            {
                //update the xp bar
                this.xpScript.UpdateXp(xpGain);
                AudioManager.instance.PlaySound("GainXp");
                AudioManager.instance.PlaySound("NewDayGood");
            }
            else
            {
                AudioManager.instance.PlaySound("NewDayBad");
            }

            gm.extraIngridients.Clear();
            gm.extraSpawned.Clear();
            gm.StartGame();
            timesGuessed = 0;
            xpGain = 0;


        }
    }

    private void ResetBorderSlots()
    {
        foreach (GameObject page in gm.ingridientsPlaceholders)
        {
            for (int i = 0; i < 12; i++)
            {
                page.GetComponentsInChildren<IngredientController>()[i].ResetBorder();
            }
        }
    }
}
