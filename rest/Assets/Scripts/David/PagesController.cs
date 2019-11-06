using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesController : MonoBehaviour
{
    public List<GameObject> pages;

    private Vector3 _basePagePosition;
    private GameObject _activePage;
    private GameObject _previousPage;
    private GameObject _nextPage;

    void Start()
    {
        this.pages = new List<GameObject>();

        PlayerInput.OnSwipe += Swipe;
        PlayerInput.OnMove += Move;
    }
    
    public void AddPage(GameObject page)
    {
        this.pages.Add(page);
    }

    public void ResetPages()
    {
        this.pages = new List<GameObject>();
        this._activePage = null;
        this._nextPage = null;
        this._previousPage = null;
    }

    public void ActivatePages()
    {
        if (this.pages.Count >= 2)
        {
            GameObject firstPage = this.pages[0];
            this._basePagePosition = firstPage.transform.localPosition;
            this.SetActivePage(firstPage);
        }
    }

    private void SetActivePage(GameObject page)
    {
        this.ResetActivePages();

        this._activePage = page;
        page.SetActive(true);
        //set previous page active
        this._previousPage = this.GetPreviousPage(page);

        if (this._previousPage != null)
        {
            this._previousPage.transform.localPosition = new Vector3(-10f, this._previousPage.transform.localPosition.y,
                this._previousPage.transform.localPosition.z);
            this._previousPage.SetActive(true);
        }

        //set next page active
        this._nextPage = this.GetNextPage(page);

        if (this._nextPage != null)
        {
            this._nextPage.transform.localPosition = new Vector3(11f, this._nextPage.transform.localPosition.y,
                this._nextPage.transform.localPosition.z);
            this._nextPage.SetActive(true);
        }
    }

    private void Move(float distance)
    {
        if (distance > 0 && this._nextPage == null || distance < 0 && this._previousPage == null)
            return;

        this.MovePage(this._activePage, distance);

        if (this._previousPage != null && this._previousPage.transform.localPosition.x < 0)
        {
            this.MovePage(this._previousPage, distance);
        }

        if (this._nextPage != null && this._nextPage.transform.localPosition.x > 1)
        {
            this.MovePage(this._nextPage, distance);
        }
    }

    private void MovePage(GameObject page, float distance)
    {
        page.transform.position = new Vector3(page.transform.position.x - distance,
            page.transform.position.y, page.transform.position.z);
        AudioManager.instance.PlaySound("Swipe");
    }

    private void Swipe(float distance)
    {
        if (this._activePage == null)
            return;

        if (distance == 1 && this._nextPage != null)
        {
            this.SetActivePage(this._nextPage);
        }
        else if (distance == -1 && this._previousPage != null)
        {
            this.SetActivePage(this._previousPage);
        }
    }

    private void ResetActivePages()
    {
        ResetPage(this._activePage);
        ResetPage(this._nextPage);
        ResetPage(this._previousPage);
    }

    private void ResetPage(GameObject page)
    {
        if (page != null)
        {
            page.transform.localPosition = this._basePagePosition;
            page.SetActive(false);
            page = null;
        }
    }

    private GameObject GetPreviousPage(GameObject page)
    {
        int index = pages.IndexOf(page);

        if (index - 1 > -1)
            return pages[index - 1];

        return null;
    }

    private GameObject GetNextPage(GameObject page)
    {
        int index = pages.IndexOf(page);

        if (index + 1 < pages.Count)
        {
            return pages[index + 1];
        }

        return null;
    }
}
