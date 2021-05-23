using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class End : UnityEvent<string>
{

}

public class Quest_Slot : MonoBehaviour
{
    public End EndCheck;

    [SerializeField] private Text Quest_Name;
    [SerializeField] private Text Quest_info;
    [SerializeField] private Text Button_Text;
    [SerializeField] private Button button;

    public void Set_Quest_Slot(string _name, string _info)
    {
        Quest_Name.text = _name;
        Quest_info.text = _info;
    }
    
    //퀘스트 수락하기
    public void Acceptance_Quest()
    {
        button.interactable = false;
        Button_Text.text = "미션중";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Clear_Quest_Button);
    }

    //퀘스트 클리어
    public void Clear_Quest_Button()
    {
        button.interactable = false;
        Button_Text.text = "퀘스트 완료";
        EndCheck.Invoke(Quest_Name.ToString());

    }

    public void Clear_Quest()
    {
        
        button.interactable = true;
        Button_Text.text = "완료";

    }

    public Button Return_Button()
    {
        return button;
    }
}
