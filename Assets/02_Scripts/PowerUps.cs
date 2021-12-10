using System;
using UnityEngine;

public enum PowerUpEnum
{
    Multiplication,
    Addition
}

public class PowerUps : MonoBehaviour
{
    public event Action<Human, PowerUpEnum, int> DidUsePowerUp;
    
    [SerializeField] private PowerUpEnum _PowerUp;
    [SerializeField] private int _PowerUpEffectNumber;

    // public float PowerUpEffectNumber => _PowerUpEffectNumber;
    // public PowerUpEnum PowerUp => _PowerUp;

    private void OnTriggerEnter(Collider other)
    {
        var human = other.GetComponent<Human>();
        
        if (!human) return;
        
        DidUsePowerUp?.Invoke(human, _PowerUp, _PowerUpEffectNumber);
        gameObject.SetActive(false);
    }
}
