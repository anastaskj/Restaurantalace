using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe 
{
    private List<GameObject> ingredients;

    public Recipe()
    {
        this.ingredients = new List<GameObject>();
    }

    public List<GameObject> GetIngredients()
    {
        return this.ingredients;
    }

    public void AddIngredient(GameObject i)
    {
        ingredients.Add(i);
    }
}
