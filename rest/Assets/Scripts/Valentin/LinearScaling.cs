using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


public class LinearScaling :  ICreateRecipe
{
    private System.Random r;
    private List<Recipe> recipes;
    private List<Recipe> finalRecipes;
    private List<GameObject> ingredientsUsed;
    private int numberRecipes;
    
    public LinearScaling(int numberRecipes)
    {
        this.r = new System.Random();
        this.recipes = new List<Recipe>();
        this.finalRecipes = new List<Recipe>();
        this.ingredientsUsed = new List<GameObject>();
        this.numberRecipes = numberRecipes;
        this.createRecipes(numberRecipes);
    }

    public List<Recipe> CreateRecipe(List<GameObject> ingredients)
    {
        assignIngredientsToBeUsed(ingredients, 6, numberRecipes); // from the provided list we chose a maximum of 6 x 5 ingredients
        int randomRecipe = 0;

        for (int i = 0; i < this.ingredientsUsed.Count; i++) // for each ingredient in our list we chose to which recipe it will be passed to.
        {
            randomRecipe = r.Next(0, this.numberRecipes);
            double average = (GetRecipeWithMaxIngredients(recipes) + GetRecipeWithMinIngredients(recipes)) / 2; // get the current average ingredients in each recipe

            if (recipes.ElementAt(randomRecipe).GetIngredients().Count < average) // if the number of ingredients of the other recipes is too low we try to fill up the others first. Important: that is not always the case - which is good :)
            {
                while (recipes.ElementAt(randomRecipe).GetIngredients().Count < average)
                {
                    randomRecipe = r.Next(0, this.numberRecipes);
                }
            }

            if (recipes.ElementAt(randomRecipe).GetIngredients().Count >= 6) // if we reached a maximum number of ingredients in a recipe we move to the next
            {
                while (recipes.ElementAt(randomRecipe).GetIngredients().Count >= 6)
                {
                    randomRecipe = r.Next(0, this.numberRecipes);
                }
            }
            recipes.ElementAt(randomRecipe).AddIngredient(this.ingredientsUsed.ElementAt(i));     // adding the ingredient to the semi-randomly chosen recipe
        }

        foreach (var recipe in recipes) // we need to have ONLY recipes with ingredients so the below code is making sure that a recipe without ingredients will never be returned.
        {
            if (recipe.GetIngredients().Count > 0)
            {
                finalRecipes.Add(recipe);
            }
        }
        return finalRecipes;
    }


    private int GetRecipeWithMaxIngredients(List<Recipe> rec)
    {
        int maximumItemsInRecipe = 0;
        foreach (var recipe in rec)
        {
            if (recipe.GetIngredients().Count > maximumItemsInRecipe)
            {
                maximumItemsInRecipe = recipe.GetIngredients().Count;
            }
        }
        return maximumItemsInRecipe;
    }
    private int GetRecipeWithMinIngredients(List<Recipe> rec)
    {
        int minimumItemsInRecipe = int.MaxValue;
        foreach (var recipe in rec)
        {
            if (recipe.GetIngredients().Count < minimumItemsInRecipe)
            {
                minimumItemsInRecipe = recipe.GetIngredients().Count;
            }
        }
        return minimumItemsInRecipe;
    }

    private void assignIngredientsToBeUsed(List<GameObject> ingredients, int maxIngredientsPerRecipe, int maxRecipes)
    {
        if (ingredients.Count > maxIngredientsPerRecipe * maxRecipes)
        {
            for (int i = 0; i < maxIngredientsPerRecipe * maxRecipes; i++)
            {
                this.ingredientsUsed.Add(ingredients.ElementAt(i)); 
            }
        }
        else
        {
            this.ingredientsUsed = ingredients;
        }
    }

    private void createRecipes(int number)
    {
        for (int i = 0; i < number; i++)
        {
            recipes.Add(new Recipe());
        }
    }
}
