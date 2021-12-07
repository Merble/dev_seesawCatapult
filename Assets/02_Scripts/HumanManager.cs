using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HumanManager : MonoBehaviour
{
    [SerializeField] private Transform _Catapult;
    [Space]
    [SerializeField] private List<Human> _HumansOnRandomMove;
    [SerializeField] private List<Human> _ThrownHumans;
    [Space]
    [SerializeField] private float _MinHumanSpeed;
    [SerializeField] private float _MaxHumanSpeed;
    [SerializeField] private float _MinX;
    [SerializeField] private float _MaxX;
    [SerializeField] private float _MinZ;
    [SerializeField] private float _MaxZ;
    [Space]
    [SerializeField] private float _HumanToCatapultWaitDuration;
    [SerializeField] private float _HumanRandomMoveWaitDuration;
    

    public List<Human> HumansOnRandomMove => _HumansOnRandomMove;
    public List<Human> ThrownHumans => _ThrownHumans;

    private void Awake()
    {
        MoveHumansRandomly();
        MoveHumansToCatapult();
    }

    private void MoveHumansRandomly()
    {
        foreach (var human in _HumansOnRandomMove.Where(human => !human.IsOnCatapult && !human.IsOnSeesaw))
        {
            human.MaxX = _MaxX;  human.MinX = _MinX;
            human.MaxZ = _MaxZ;  human.MinZ = _MinZ;
            
            human.MaxMoveSpeed = _MaxHumanSpeed;  human.MinMoveSpeed = _MinHumanSpeed;

            human.IsOnRandomMove = true;
            StartCoroutine(human.MoveRandomLocation());
        }
    }
    
    public void MoveHumansToCatapult()
    {
        StartCoroutine(MoveHumansToCatapultRoutine(_HumanToCatapultWaitDuration));
    }

    private IEnumerator MoveHumansToCatapultRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (!_HumansOnRandomMove.Any()) yield break;
        
        var human = _HumansOnRandomMove[0];
        human.IsOnRandomMove = false;
        
        var moveDuration = Vector3.Distance(human.transform.position, _Catapult.position) / _MaxHumanSpeed;
        LeanTween.move(human.gameObject, _Catapult, moveDuration);
    }

    private void MoveHumansToNearestSeesaw()
    {
        
    }
}
