using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Collider_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject Skill_Q_Collider;

    public void Skill_Q_AttackCollision()
    {
        Skill_Q_Collider.SetActive(true);
    }
}
