using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item_Use : MonoBehaviour
{
    public static bool Item_UseOpened = false; 


    [SerializeField]
    private GameObject Item_Use_Base;

    [SerializeField]
    private Button Using;
    [SerializeField]
    private Button Throw_away;
    [SerializeField]
    private Button Close;


    public void Show_Item_Use(int _itemType, Vector3 _Pos)
    {
        Item_Use_Base.SetActive(true);
        SetButten(_itemType);

        Item_UseOpened = true;

        _Pos += new Vector3(Item_Use_Base.GetComponent<RectTransform>().rect.width *0.8f, -Item_Use_Base.GetComponent<RectTransform>().rect.height * 0.8f, 0f);
        Item_Use_Base.transform.position = _Pos;
    }

    void SetButten(int _itemType)
    {
        switch (_itemType)
        {
            case (int)Item.ITEMTYPE.Using:
                Using.interactable = true;
                break;
        }

    }

    public void Button_Using()
    {

    }

    public void Button_Throw_away()
    {

    }

    public void Item_Use_Close()
    {
        Item_Use_Base.SetActive(false);

        Item_UseOpened = false;

    }

}
