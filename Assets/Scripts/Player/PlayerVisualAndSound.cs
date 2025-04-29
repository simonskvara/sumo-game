using System;
using UnityEngine;

public class PlayerVisualAndSound : MonoBehaviour
{
    public PlayerChargeAttack chargeAttack;

    public Animator animator;

    [Header("Sound Effect")]
    public AudioSource audioSource;
    public AudioClip dashSfx;

    private void OnEnable()
    {
        chargeAttack.OnDashStart += DashSoundEffect;
    }

    private void OnDisable()
    {
        chargeAttack.OnDashStart -= DashSoundEffect;
    }

    private void Update()
    {
        DashAnimation();
    }

    void DashAnimation()
    {
        animator.SetBool("IsDashing", chargeAttack.IsDashing());
    }

    void DashSoundEffect()
    {
        audioSource.clip = dashSfx;
        audioSource.Play();
    }
}
