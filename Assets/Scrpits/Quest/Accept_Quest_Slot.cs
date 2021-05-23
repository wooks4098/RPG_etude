using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Accept_Quest_Slot : MonoBehaviour
{
    [SerializeField] private Text Quest_Name;
    [SerializeField] private Text Quest_info;

    public Text Return_Name()
    {
        return Quest_Name;
    }

    public Text Return_info()
    {
        return Quest_info;
    }
}
