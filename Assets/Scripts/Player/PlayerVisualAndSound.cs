using System;
using UnityEngine;

public class PlayerVisualAndSound : MonoBehaviour
{
    public PlayerChargeAttack chargeAttack;

    public Animator animator;

    private void Update()
    {
        DashAnimation();
    }

    void DashAnimation()
    {
        animator.SetBool("IsDashing", chargeAttack.IsDashing());
    }
}
