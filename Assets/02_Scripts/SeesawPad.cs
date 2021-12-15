using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SeesawPad : MonoBehaviour
{
    public Action<float, bool> DidMassChange;

    [SerializeField, ReadOnly] private List<Human> _Humans = new List<Human>();
    [SerializeField] private bool _IsPlayer;
    

    public List<Human> Humans => _Humans;
    public float TotalMass { get; private set; }

    public void AddHuman(Human human)
    {
        TotalMass += human.Mass;
        _Humans.Add(human);
        
        DidMassChange?.Invoke(human.Mass, _IsPlayer);
    }

    [Button]
    private void ShowTotalMass()
    {
        Debug.Log(TotalMass);
    }
}
