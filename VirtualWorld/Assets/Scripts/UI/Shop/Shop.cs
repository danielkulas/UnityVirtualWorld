/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{
    public GameObject ShopUI;


    public void openShop()
    {
        ShopUI.SetActive(true);
    }

    public void exitShop()
    {
        ShopUI.SetActive(false);
    }
}