using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HumanManager _HumanManager;
    [SerializeField] private VariableJoystick _Joystick;
    [SerializeField] private Catapult _PlayerCatapult;
    
    private void Awake()
    {
        _Joystick.DragDidStart += () => { /*_HumanManager.MoveHumansToCatapult();*/ };
        _Joystick.DragDidEnd += direction => { _PlayerCatapult.ThrowHumans(-direction.normalized); };

        _PlayerCatapult.HumanDidComeToCatapult += human =>
        {
            if (human == null) return;
        
            human.IsOnCatapult = true;
            _PlayerCatapult.HumansOnCatapult.Add(human);
            _HumanManager.HumansOnRandomMove.Remove(human);
            _HumanManager.MoveHumansToCatapult();
        };
    }
}
