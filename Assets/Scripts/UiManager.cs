using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour 
{
    public GameObject _GoldBar;
    public GameObject _Shop;
    private void Start()
    {
        GameManager._Instance._UiManager = this;
    }

    //OpenShop-----------------
    public void OpenShop()
    {
        _Shop.SetActive(true);
    }

    //ClossShop----------------
    public void ClossShop()
    {
        _Shop.SetActive(false);
    }
}
