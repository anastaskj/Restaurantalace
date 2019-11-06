using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICreateRecipe
{
    List<Recipe> CreateRecipe(List<GameObject> ingredients);
}
