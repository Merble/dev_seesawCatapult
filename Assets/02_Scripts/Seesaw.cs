using System;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    public event Action PlayerDidWin;
    public event Action EnemyDidWin;

    [SerializeField] private SeesawPad _PlayerSeesawPad;
    [SerializeField] private SeesawPad _EnemySeesawPad;
    [SerializeField] private float _MassEffectOnBalance;
    [SerializeField] private float _MaxRotationAngle;
        
    [SerializeField] private float _BalanceValue;   // Between (0, 1)
        
    [SerializeField] private float _RotationSpeed;

    private void Awake()
    {
        _BalanceValue = .5f;
        _PlayerSeesawPad.DidMassChange += BalanceChange;
        _EnemySeesawPad.DidMassChange += BalanceChange;
    }
    
    private void BalanceChange(float mass, bool isPlayer)
    {
        _BalanceValue += (isPlayer ? 1 : -1) * mass * _MassEffectOnBalance;
        _BalanceValue = Mathf.Clamp(_BalanceValue, 0, 1);
        
        RotateBoardToCurrentBalance();
            
        // if (_BalanceValue >= 1f)
        // {
        //     PlayerDidWin?.Invoke();
        // }
        //     
        // if (_BalanceValue <= 0)
        // {
        //     EnemyDidWin?.Invoke();
        // }
    }

    // public void SetBalance(float val)
    // {
    //     _BalanceValue = val;
    //     RotateBoardToCurrentBalance();
    // }

    private void RotateBoardToCurrentBalance()
    {
        var newAngle = Mathf.Lerp(-_MaxRotationAngle, _MaxRotationAngle, _BalanceValue);
        var angle = transform.rotation.eulerAngles.x;

        if (angle > 180)
        {
            angle -= 360;
        }
            
        var angleDistance = Mathf.Abs(newAngle - angle) / _RotationSpeed;
        var rotationEuler = new Vector3(newAngle, 0, 0);

        foreach (var human in _PlayerSeesawPad.Humans)
        {
            human.transform.SetParent(transform);
        }
        
        LeanTween.cancel(gameObject);
        LeanTween.rotate(gameObject, -rotationEuler, angleDistance);
    }
}
