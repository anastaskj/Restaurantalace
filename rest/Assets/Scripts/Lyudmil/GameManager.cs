using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject GameMenu;
    public GameObject RecipesMenu;
    public GameObject Restaurant;
    public GameObject IngredientsMenu;
    public GameObject XpBar;
    public Button ready;
    public List<GameObject> ingredients;
    public List<GameObject> chosenIngredients;
    public List<GameObject> extraIngridients;
    public List<GameObject> recipes;
    public List<GameObject> ingridientsPlaceholders;
    public List<GameObject> chosenAndExtra;
    public ShaderController shaderScript;
    public List<Recipe> recipesClass;
    public Button ingridientsReadyBtn;
    public Button hintsBtn;
    public Button recipiesBtn;
    int nrOfRecipies;
    private LinearScaling ls;
    private PagesController pg;
    public List<GameObject> allToChooseFrom;
    public List<GameObject> extraSpawned;
    public GameObject extraSpawning;
    public Text dayCounter;
    public Button tutorial;

    
    public static List<GameObject> playerSelectedIngredients;


    private int days = 0;
    void Start()
    {
        this.pg = this.GetComponent<PagesController>();
    }
    
    private void CheckForIngredient()
    {
        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));

        if (ray)
        {
            GameObject possibleIngredient = ray.collider.gameObject;
            if (possibleIngredient.CompareTag("Ingredient"))
            {
                possibleIngredient.GetComponent<IngredientController>().SelectItem();
            }
        }
    }

    public void StartGame()
    {
        ls = new LinearScaling(5);
        this.pg.ResetPages();
        GameMenu.SetActive(false);
        RecipesMenu.SetActive(true);
        Restaurant.SetActive(true);
        XpBar.SetActive(true);
        IngredientsMenu.SetActive(false);
        hintsBtn.gameObject.SetActive(false);
        ingridientsReadyBtn.gameObject.SetActive(false);
        recipiesBtn.gameObject.SetActive(true);
        allToChooseFrom = new List<GameObject>();
        chosenIngredients = new List<GameObject>();
        ClearRecipeSlots();
        ClearIngredientSlots();
        recipesClass = new List<Recipe>();
        tutorial.gameObject.SetActive(true);
        days++;
        dayCounter.text = days.ToString();
        ShuffleList(ingredients);
        AudioManager.instance.PlaySound("DisplayMenu");

        recipesClass = ls.CreateRecipe(ingredients);


        for (int i = 0; i < recipesClass.Count; i++)
        {
            for (int j = 0; j < recipesClass.ElementAt(i).GetIngredients().Count; j++)
            {
                GameObject newIngridient = Instantiate(recipesClass.ElementAt(i).GetIngredients()[j], recipes[i].transform.GetChild(j));
                newIngridient.GetComponent<Transform>().localPosition = new Vector3(-4f, 0, -1);
                newIngridient.GetComponent<Transform>().localScale = new Vector3(5, 5, 1);
                chosenIngredients.Add(newIngridient);
            }
        }

        foreach (GameObject recipe in recipes)
        {
            this.pg.AddPage(recipe);
        }

        this.pg.ActivatePages();
    }

    public void takeHint()
    {
        shaderScript.itemsToDestroy = 5;
        shaderScript.destroyItem(extraSpawned);
        AudioManager.instance.PlaySound("Hint");
    }
   
    public void Ready()
    {
        PlayerInput.OnTouch += IngredientSelector.IngredientSelected;
        this.pg.ResetPages();
        RecipesMenu.SetActive(false);
        IngredientsMenu.SetActive(true);
        ResetRecipePosition();
        makeExtraIngridients(chosenIngredients);
        playerSelectedIngredients = new List<GameObject>();
        int counter = 0;
        recipiesBtn.gameObject.SetActive(false);
        ingridientsReadyBtn.gameObject.SetActive(true);
        hintsBtn.gameObject.SetActive(true);
        AudioManager.instance.PlaySound("Cook");
        for (int k = 0; k < 5; k++) 
        {
            this.pg.AddPage(ingridientsPlaceholders[k]);
            for (int j = 0; j < 12; j++)
            {
            GameObject newIngridient = Instantiate(chosenAndExtra[counter], ingridientsPlaceholders[k].transform.GetChild(j));

                newIngridient.GetComponent<Transform>().localPosition = new Vector3(0, 0, -1);
                newIngridient.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                allToChooseFrom.Add(newIngridient);
                counter++;
                if (counter >= chosenAndExtra.Count)
                {
                    foreach (GameObject item in extraIngridients)
                    {
                        foreach (GameObject chosen in allToChooseFrom)
                        {
                            if (item.name + "(Clone)(Clone)" == chosen.name)
                            {
                                extraSpawned.Add(chosen);
                            }
                        }
                    }
                    
                    this.pg.ActivatePages();
                    return;
                }
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void ClearRecipeSlots()
    {
        foreach (GameObject recipe in recipes)
        {
            for (int i = 0; i < 6; i++)
            {
                recipe.GetComponentsInChildren<IngredientController>()[i].ClearItem();
            }
        }
    }

    private void ClearIngredientSlots()
    {
        foreach (GameObject page in ingridientsPlaceholders)
        {
            for (int i = 0; i < 12; i++)
            {
                page.GetComponentsInChildren<IngredientController>()[i].ClearItem();
                page.GetComponentsInChildren<IngredientController>()[i].ResetBorder();
            }
        }
    }

    private void ResetRecipePosition()
    {
        recipes[0].SetActive(true);

        for (int i = 1; i < recipes.Count; i++)
        {
            recipes[i].SetActive(false);
        }
    }

    private void makeExtraIngridients(List<GameObject> chosenIngridients)
    {
        chosenAndExtra = new List<GameObject>();
        foreach (GameObject item in ingredients)
        {
            extraIngridients.Add(item);
        }

        foreach (GameObject item in chosenIngredients)
        {
            foreach (GameObject chosen in ingredients)
            {
                if (item.name == chosen.name + "(Clone)")
                {
                    extraIngridients.Remove(chosen);
                }
            }
        }

        int counter = 0;

        foreach (GameObject item in extraIngridients)
        {
            GameObject newExtraItem = Instantiate(item, extraSpawning.transform);
            chosenAndExtra.Add(newExtraItem);
            if (counter == chosenIngredients.Count / 2)
            {
                break;
            }
            counter++;
        }
        extraSpawning.SetActive(false);
        foreach (GameObject item in chosenIngredients)
        {
            chosenAndExtra.Add(item);
        }

        ShuffleList(chosenAndExtra);
    }

    public static void ShuffleList(List<GameObject> list)
    {
        System.Random r = new System.Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
