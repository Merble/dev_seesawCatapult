using System;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public event Action<Human> HumanDidComeToCatapult;
    public event Action<Human[]> DidThrewHumans;
    
    [SerializeField] private float _ThrowForce;
    [SerializeField] private float _DirectionValueY;
    
    
    private readonly float _gravity = Math.Abs(Physics.gravity.y);

    private List<Human> HumansOnCatapult { get; } = new List<Human>();

    private void OnTriggerEnter(Collider other)
    {
        HumanDidComeToCatapult?.Invoke(other.GetComponent<Human>());
    }

    public void ThrowHumans(Vector2 direction)
    {
        foreach (var human in HumansOnCatapult)
        {
            var humanRb = human.GetComponent<Rigidbody>();
            humanRb.AddForce(new Vector3(direction.x, _DirectionValueY, direction.y) * _ThrowForce, ForceMode.VelocityChange);
            human.IsOnCatapult = false;
        }
        DidThrewHumans?.Invoke(HumansOnCatapult.ToArray());
        HumansOnCatapult.Clear();
    }

    public Vector3 FindTrajectoryFinishPosition(Vector2 direction)
    {
        var forceY = _DirectionValueY * _ThrowForce;
        var time = (forceY * 2) /_gravity;
        var distance = time * direction.magnitude * _ThrowForce;
        
        var groundDirection = new Vector3(direction.x, 0, direction.y);
        var finishPos = transform.position + (distance * groundDirection);
        
        return finishPos;
    }

    public void AddHuman(Human human)
    {
        HumansOnCatapult.Add(human);
    }
}
