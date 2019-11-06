using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEvent : MonoBehaviour
{
    public GameObject menuText;
    public Animator itemsFadeIn;
    public void FadeInMenu()
    {
        menuText.SetActive(true);
        itemsFadeIn.Play(0);
    }
}
