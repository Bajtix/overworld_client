using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    public MeshRenderer model;

    public Vector3 destPos;
    public float speed = 0;

    public Animator playerAnimator;

    public ItemStack[] stacks;


    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        stacks = new ItemStack[40];
        
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * 20);
        if(playerAnimator!=null)
        playerAnimator.SetFloat("Speed", speed);
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
