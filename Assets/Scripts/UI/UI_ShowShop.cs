using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowShop : MonoBehaviour
{
    [SerializeField]
    private GameObject myShopUI;
    [SerializeField]
    private GameObject otherShopUI;


    public bool showUI = false;

    void Start()
    {
        myShopUI.SetActive(showUI);
    }

    public void OnClicked()
    {
        showUI = myShopUI.activeSelf;
        showUI = !showUI;
        myShopUI.SetActive(showUI);

        if (otherShopUI.activeSelf)
            otherShopUI.SetActive(false);
    }
}

