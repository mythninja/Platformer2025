﻿using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    public static event Action OnPlayedDied;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
    }

    private void OnDestroy()
    {
        
        GameController.OnReset -= ResetHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.damage);
        }

        Trap trap = collision.GetComponent<Trap>();
        if (trap != null && trap.damage > 0)
        {
            TakeDamage(trap.damage);
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        if (healthUI != null)
            healthUI.SetMaxHearts(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthUI != null)
            healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            OnPlayedDied?.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.blue;
        }
    }
}
