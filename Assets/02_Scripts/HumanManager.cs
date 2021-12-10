using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanManager : MonoBehaviour
{
    [SerializeField] private Transform _Catapult;
    [Space]
    [SerializeField] private List<Human> _Humans;
    [Space]
    [SerializeField] private float _MinHumanSpeed;
    [SerializeField] private float _MaxHumanSpeed;
    [SerializeField] private float _MinX;
    [SerializeField] private float _MaxX;
    [SerializeField] private float _MinZ;
    [SerializeField] private float _MaxZ;
    [Space]
    [SerializeField] private float _HumanToCatapultWaitDuration;

    public List<Human> HumansOnRandomMove { get; } = new List<Human>();

    public List<Human> ThrownHumans { get; } = new List<Human>();

    private void Awake()
    {
        foreach (var human in _Humans)
        {
            HumansOnRandomMove.Add(human);
        }
        MoveHumansRandomly();
        MoveHumansToCatapult();
    }

    private void MoveHumansRandomly()
    {
        foreach (var human in HumansOnRandomMove.Where(human => !human.IsOnCatapult && !human.IsOnSeesaw))
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

        if (!HumansOnRandomMove.Any()) yield break;
        
        var human = HumansOnRandomMove[Random.Range(0, HumansOnRandomMove.Count)];
        human.MoveTo(_Catapult.position);
    }

    private void MoveHumansToNearestSeesaw()
    {
        
    }
}
