using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Effect : MonoBehaviour
{
    public TrailRenderer trailEffect;

   

    public void Start_Attack()
    {
        trailEffect.enabled = true;
    }
    public void End_Attack()
    {
        trailEffect.enabled = false;
    }
}
