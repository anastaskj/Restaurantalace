using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public List<Renderer> disolveRenderer;
    float reducer = 0.1f;
    bool reducing = false;
    int index;
    public int itemsToDestroy=3;
    int counter = 1;
    private void Update()
    {
        if (counter > itemsToDestroy)
        {
            reducing = false;
        }
        if (reducing)
        {
            DisolveItem(index);
        } 
    }
    public void destroyItem(List<GameObject> extraIngridients)
    {
        disolveRenderer.Clear();
        foreach (GameObject item in extraIngridients)
        {

            disolveRenderer.Add(item.GetComponent<SpriteRenderer>());
        }

        counter = 1;
        index = Random.Range(0, disolveRenderer.Count-1);
        reducing = true;
    }
    public void DisolveItem(int itemIndex)
    {
        reducer += Time.deltaTime;
        disolveRenderer[itemIndex].material.SetFloat("Vector1_8FC47B1C", reducer);
        if (disolveRenderer[itemIndex].material.GetFloat("Vector1_8FC47B1C") >= 1)
        {

            reducer = 0.01f;
            disolveRenderer.Remove(disolveRenderer[itemIndex]);
            AudioManager.instance.PlaySound("BurnHint");
            nextItem();
        }
        
    }
    public void nextItem()
    {
        counter++;
        index = Random.Range(0, disolveRenderer.Count-1);      
    }
}
