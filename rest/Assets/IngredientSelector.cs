using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSelector : MonoBehaviour
{
    public static void IngredientSelected(Vector2 touchPosition)
    {
        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(touchPosition));

        if (ray)
        {
            GameObject possibleIngredient = ray.collider.gameObject;
            if (possibleIngredient.CompareTag("Ingredient"))
            {
                possibleIngredient.GetComponent<IngredientController>().SelectItem();
            }
        }
    }
}
