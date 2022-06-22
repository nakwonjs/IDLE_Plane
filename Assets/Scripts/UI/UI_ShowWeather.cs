using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShowWeather : MonoBehaviour
{
    [SerializeField]
    private GameObject myHomeUI;
    [SerializeField]
    private GameObject myShopUI;
    public bool showUI = true;
    public bool ShopShowUI = false;

    void Start()
    {
        myHomeUI.SetActive(showUI);
    }

    public void OnClickedHome()
    {
        if (ShopShowUI)
            OnClickedUpgrade();
        //myHomeUI.SetActive(showUI);
    }
    public void OnClickedUpgrade()
    {
        ShopShowUI = !ShopShowUI;
        myHomeUI.SetActive(ShopShowUI);
    }
}
