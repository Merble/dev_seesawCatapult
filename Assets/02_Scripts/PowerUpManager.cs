using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private List<PowerUps> _PowerUps = new List<PowerUps>();
    [SerializeField] private float _InstantiateCircleRadius;

    public List<Human[]> HumanGroupList { get; } = new List<Human[]>();

    private void Awake()
    {
        foreach (var powerUp in _PowerUps)
        {
            powerUp.DidUsePowerUp += OnPowerUpUse;
        }
    }

    private void OnPowerUpUse(Human human, PowerUpEnum powerUpType, Vector3 powerUpPos,int powerUpEffectNumber)
    {
        foreach (var humanGroup in HumanGroupList.Where(humanGroup => humanGroup.Contains(human)))
        {
            switch (powerUpType)
            {
                case PowerUpEnum.Addition:
                    AddHumans(humanGroup, powerUpPos,powerUpEffectNumber);
                    break;
                case PowerUpEnum.Multiplication:
                    MultiplyHumans(humanGroup, powerUpPos,powerUpEffectNumber);
                    break;
                default:
                    return;
            }
        }
    }
    
    private void MultiplyHumans(Human[] humanGroup, Vector3 powerUpPos,int powerUpEffectNumber)
    {
        var thinHumanCounter = 0;
        Human thinHumanPrefab = null;
        foreach (var human in humanGroup)
        {
            if (human.Type != HumanType.Thin) continue;
            
            thinHumanPrefab = human.Prefab;
            thinHumanCounter++;
        }

        var fatHumanCounter = 0;
        Human fatHumanPrefab = null;
        foreach (var human in humanGroup)
        {
            if (human.Type != HumanType.Fat) continue;
            
            fatHumanPrefab = human.Prefab;
            fatHumanCounter++;
        }
        
        var thinHumanNumber = thinHumanCounter * (powerUpEffectNumber - 1);
        for (var i = 0; i < thinHumanNumber; i++)
        {
            var pos = Random.insideUnitCircle * _InstantiateCircleRadius;
            
            if (thinHumanPrefab is null) continue;
            
            var newPos = new Vector3(powerUpPos.x + pos.x, thinHumanPrefab.transform.position.y, powerUpPos.z + pos.y);
            Instantiate(thinHumanPrefab, newPos, Quaternion.identity);
        }
        
        var fatHumanNumber = fatHumanCounter * (powerUpEffectNumber - 1);
        for (var i = 0; i < fatHumanNumber; i++)
        {
            var pos = Random.insideUnitCircle * _InstantiateCircleRadius;
            
            if (fatHumanPrefab is null) continue;
            
            var newPos = new Vector3(powerUpPos.x + pos.x, fatHumanPrefab.transform.position.y, powerUpPos.z + pos.y);
            Instantiate(fatHumanPrefab, newPos, Quaternion.identity);
        }
    }

    private void AddHumans(Human[] humanGroup, Vector3 powerUpPos, int powerUpEffectNumber)
    {
        Human humanPrefab = null;
        foreach (var human in humanGroup)
        {
            if (human.Type == HumanType.Fat)
            {
                humanPrefab = human.Prefab;
                break;
            }
            
            humanPrefab = human.Prefab;
        }
        
        for (var i = 0; i < powerUpEffectNumber; i++)
        {
            var pos = Random.insideUnitCircle * _InstantiateCircleRadius;
            
            if (humanPrefab is null) continue;
            
            var newPos = new Vector3(powerUpPos.x + pos.x, humanPrefab.transform.position.y, powerUpPos.z + pos.y);
            Instantiate(humanPrefab, newPos, Quaternion.identity);
        }
    }
}
