using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public event Action<HumanType, int> PowerUpDidInstantiate;
    
    [SerializeField] private List<PowerUps> _PowerUps = new List<PowerUps>();

    public List<List<Human>> HumanGroupList { get; } = new List<List<Human>>();

    private void Awake()
    {
        foreach (var powerUp in _PowerUps)
        {
            powerUp.DidUsePowerUp += OnPowerUpUse;
        }
    }

    private void OnPowerUpUse(Human human, PowerUpEnum powerUpType, int powerUpEffectNumber)
    {
        foreach (var humanGroup in HumanGroupList)
        {
            if (humanGroup.Contains(human))
            {
                Debug.Log("We're here!");
                switch (powerUpType)
                {
                    case PowerUpEnum.Addition:
                        AddHumans(humanGroup, powerUpEffectNumber);
                        break;
                    case PowerUpEnum.Multiplication:
                        MultiplyHumans(humanGroup, powerUpEffectNumber);
                        break;
                    default:
                        return;
                }
            }
        }
    }
    
    private void MultiplyHumans(List<Human> humanGroup, int powerUpEffectNumber)
    {
        var thinHumanCounter = humanGroup.Count(human => human.Type == HumanType.Thin);
        var fatHumanCounter = humanGroup.Count(human => human.Type == HumanType.Fat);

        // foreach (var human in humanGroup)
        // {
        //     if (human.Type == HumanType.Fat)
        //     {
        //         fatHumanCounter++;
        //     }
        //     else
        //     {
        //         thinHumanCounter++;
        //     }
        // }
        
        if (fatHumanCounter > 0)
        {
            var number = fatHumanCounter * (powerUpEffectNumber - 1);
            PowerUpDidInstantiate?.Invoke(HumanType.Fat, number);
        }
        
        if (thinHumanCounter > 0)
        {
            var number = thinHumanCounter * (powerUpEffectNumber - 1);
            PowerUpDidInstantiate?.Invoke(HumanType.Thin, number);
        }
    }

    private void AddHumans(List<Human> humanGroup, int powerUpEffectNumber)
    {
        var fatHumanCounter = humanGroup.Count(human => human.Type == HumanType.Fat);

        PowerUpDidInstantiate?.Invoke(fatHumanCounter > 0 ? HumanType.Fat : HumanType.Thin, powerUpEffectNumber);
    }
}
