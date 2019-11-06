using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ComplexScaling : ICreateRecipe
{
    private System.Random r;
    private List<Recipe> recipes;
    private LinearScaling linearScaling;
    private int percentageComplexItems;
    private List<GameObject> complexIngredients;

    public void ComplexIngredients(LinearScaling linearScaling, int percentageComplexItems, List<GameObject> complexIngredients)
    {
        this.r = new System.Random();
        this.recipes = new List<Recipe>();
        this.complexIngredients = complexIngredients;
        this.percentageComplexItems = percentageComplexItems;
        this.linearScaling = linearScaling;
    }
    public List<Recipe> CreateRecipe(List<GameObject> ingredients)
    {

        this.recipes = this.linearScaling.CreateRecipe(ingredients); // Getting the recipes from linear scaling
        GameObject basicIngredient, complexIngredient;
        int counter = CalculateNumberComplexItems(this.percentageComplexItems); // calculating the number of complex items based on the percentage entered in the constr.

        List<GameObject> alreadyRemoved = new List<GameObject>();
        List<GameObject> alreadyAddedComplex = new List<GameObject>();

        for (int i = 0; i < counter; i++)
        {// we get random recipe, random basic ingredient and random complex ingredient
            Recipe randomRecipe = GetRandomRecipe();
            basicIngredient = GetRandomIngredient(randomRecipe);
            complexIngredient = GetRandomComplexIngredient();
            if (alreadyRemoved.Contains(basicIngredient) && alreadyRemoved.Count != counter) // check if we already replaced the basic ingredient with complex
            {
                while (alreadyRemoved.Contains(basicIngredient))
                {
                    basicIngredient = GetRandomIngredient(randomRecipe);
                }
            }
            if (alreadyAddedComplex.Contains(complexIngredient) && alreadyAddedComplex.Count != complexIngredients.Count)
            {
                while (alreadyAddedComplex.Contains(complexIngredient))
                {
                    complexIngredient = GetRandomComplexIngredient();
                }
            }
            // replacing the random basic ingredient with the randomly chosen complex ingredient
            randomRecipe.GetIngredients()[randomRecipe.GetIngredients().FindIndex(ind => ind.Equals(basicIngredient))] = complexIngredient;
            alreadyRemoved.Add(basicIngredient);
            alreadyAddedComplex.Add(complexIngredient);
        }

        return recipes;
    }

    private Recipe GetRandomRecipe()
    {
        return recipes.ElementAt(r.Next(0, recipes.Count));
    }
    private GameObject GetRandomIngredient(Recipe recipe)
    {
        GameObject ingr;

        ingr = recipe.GetIngredients().ElementAt(r.Next(0, recipe.GetIngredients().Count));

        return ingr;
    }
    private GameObject GetRandomComplexIngredient()
    {
        GameObject ingr;

        ingr = complexIngredients.ElementAt(r.Next(0, complexIngredients.Count));

        return ingr;
    }

    private int CalculateNumberComplexItems(int percentage)
    {
        int count = 0;
        foreach (var item in recipes)
        {
            count += item.GetIngredients().Count;
        }

        if (percentage > 100)
        {
            percentage = 100;
        }
        if (percentage < 0)
        {
            percentage = 1;
        }

        int result = Convert.ToInt32(Math.Round((double)count * percentage / 100));
        if (result < 1)
        {
            return 1;
        }
        else
        {
            return result;
        }
    }

}
