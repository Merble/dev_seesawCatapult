using System;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public event Action<Human> HumanDidComeToCatapult;
    [SerializeField] private float _ThrowForce;
    [SerializeField] private float _DirectionValueY;
    
    private List<Human> _humansOnCatapult = new List<Human>();

    public List<Human> HumansOnCatapult => _humansOnCatapult;

    private void OnTriggerEnter(Collider other)
    {
        HumanDidComeToCatapult?.Invoke(other.GetComponent<Human>());
        // var human = other.gameObject.GetComponent<Human>();
        //
        // if (human == null) return;
        //
        // human.IsOnCatapult = true;
        // _humansOnCatapult.Add(human);
    }

    public void ThrowHumans(Vector2 direction)
    {
        foreach (var human in _humansOnCatapult)
        {
            var humanRb = human.GetComponent<Rigidbody>();
            humanRb.AddForce(new Vector3(direction.x, _DirectionValueY, direction.y) * _ThrowForce, ForceMode.Impulse);
        }
        _humansOnCatapult.Clear();
    }
}
