using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New item/item")]
public class Item : ScriptableObject
{
    public string itemName;     //아이템 이름
    public ITEMTYPE itemtype;   //아이템 종류
    public Sprite itemSprite;       //아이템 스프라이트
    public enum ITEMTYPE
    {
        Using,
    }
}
