using System;
using UnityEngine;

public enum PowerUpEnum
{
    Multiplication,
    Addition
}

public class PowerUp : MonoBehaviour
{
    public event Action<Human, PowerUpEnum, Vector3, int> DidUsePowerUp;
    
    [SerializeField] private PowerUpEnum _PowerUp;
    [SerializeField] private int _PowerUpEffectNumber;


    private void OnTriggerEnter(Collider other)
    {
        var human = other.GetComponentInParent<Human>();
        
        if (!human) return;
        
        DidUsePowerUp?.Invoke(human, _PowerUp, transform.position,_PowerUpEffectNumber);
        gameObject.SetActive(false);
    }
}