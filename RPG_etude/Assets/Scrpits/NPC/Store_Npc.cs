using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Npc : MonoBehaviour
{
    [SerializeField] private GameObject StoreBase;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && StoreBase.activeSelf)
            Close_Store();
    }

    public void Show_Store()
    {
        StoreBase.SetActive(true);
    }

    public void Close_Store()
    {
        StoreBase.SetActive(false);

    }
}
