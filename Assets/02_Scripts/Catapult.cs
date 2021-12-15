using System;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public event Action<Human> HumanDidComeToCatapult;
    public event Action<Human[]> DidThrowHumans;
    
    [SerializeField] private float _ThrowForce;
    [SerializeField] private float _DirectionValueY;
    
    
    private readonly float _gravity = Math.Abs(Physics.gravity.y);

    private List<Human> HumansOnCatapult { get; } = new List<Human>();

    public void DidHumanCome(Human human)
    {
        HumanDidComeToCatapult?.Invoke(human);
    }

    public void ThrowHumans(Vector2 direction)
    {
        foreach (var human in HumansOnCatapult)
        {
            human.Rigidbody.AddForce(new Vector3(direction.x, _DirectionValueY, direction.y) * _ThrowForce, ForceMode.VelocityChange);
            human.SetState(Human.HumanState.IsFlying);
            //human.MakeColliderSmaller();
        }
        DidThrowHumans?.Invoke(HumansOnCatapult.ToArray());
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
