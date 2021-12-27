using System;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    #region Public Fields

    public float health = 10;
    #endregion

    private float _maxHealth;
    
    #region Unity Methods

    private void Awake()
    {
        _maxHealth = health;
    }
    #endregion

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
    
    #region Private Methods
    #endregion
}
