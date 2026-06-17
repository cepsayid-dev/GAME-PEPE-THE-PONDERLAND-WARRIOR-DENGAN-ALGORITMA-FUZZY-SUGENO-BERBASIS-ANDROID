using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip hurtSfx;

    public UnityEvent<int, int> healthChanged;
    Animator animator;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Behaviour[] components;
    public int MaxHealth 
    {
        get 
        {
            return _maxHealth;
        }
        set
        { 
            _maxHealth = value;
        }
    }

    [SerializeField] private int _health = 100;
    public int Health 
    {
        get 
        {
            return _health;
        }
        set 
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            //if no health is deadge
            if(_health <= 0) 
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        IsAlive = false;
        animator.SetBool("isAlive", false);
        //deactivate all comp
        foreach (Behaviour componment in components) componment.enabled = false;

        // Trigger respawn
        PRespawn respawnComponent = GetComponent<PRespawn>();
        if (respawnComponent != null)
        {
            respawnComponent.Respawn();
        }
    }


    [SerializeField] private bool _isAlive = true;

    [SerializeField] private bool isInvincible = false;
    private float timeSinceHit = 0;
    public float invincibilityTimer = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive",value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible) 
        {
            if(timeSinceHit > invincibilityTimer) 
            {
                //Remove invins
                isInvincible=false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
        
        
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible) 
        {
            Health -= damage;
           
            isInvincible = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            if (SoundManager.instance != null) SoundManager.instance.PlaySound(hurtSfx);
            isInvincible = true;
        }
    }



}
