using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HumanManager _HumanManager;
    [SerializeField] private VariableJoystick _Joystick;
    [SerializeField] private Catapult _PlayerCatapult;
    [SerializeField] private GameObject _Indicator;
    [SerializeField] private PowerUpManager _PlayerPowerUpManager;
    [Space] 
    [SerializeField] private Human _FatHuman;
    [SerializeField] private Human _ThinHuman;
    
    

    private void Awake()
    {
        _Joystick.DragDidStart += () => { _Indicator.SetActive(true); };
        _Joystick.DidDrag += OnDrag;
        _Joystick.DragDidEnd += direction => { _PlayerCatapult.ThrowHumans(-direction);  _Indicator.SetActive(false); };

        _PlayerCatapult.HumanDidComeToCatapult += OnHumanArriveCatapult;
        _PlayerCatapult.DidThrewHumans += () =>
        {
            _PlayerPowerUpManager.HumanGroupList.Add(_PlayerCatapult.HumansOnCatapult);
            _PlayerCatapult.HumansOnCatapult.Clear();
        };

        _PlayerPowerUpManager.PowerUpDidInstantiate += InstantiateHumansForPowerUp;
    }

    private void OnHumanArriveCatapult(Human human)
    {
        if (human == null) return;

        human.IsOnCatapult = true;

        _PlayerCatapult.HumansOnCatapult.Add(human);

        _HumanManager.HumansOnRandomMove.Remove(human);
        _HumanManager.MoveHumansToCatapult();
    }

    private void OnDrag(Vector2 direction)
    { 
        var finishPos = _PlayerCatapult.FindTrajectoryFinishPosition(-direction);
        _Indicator.transform.position = new Vector3(finishPos.x, _Indicator.transform.position.y, finishPos.z);
    }

    private void InstantiateHumansForPowerUp(HumanType type, int humanNumberToInstantiate)
    {
        switch (type)
        {
            case HumanType.Fat:
                for (var i = 0; i < humanNumberToInstantiate; i++)
                {
                    Instantiate(_FatHuman);
                }
                break;
            case HumanType.Thin:
                for (var i = 0; i < humanNumberToInstantiate; i++)
                {
                    Instantiate(_ThinHuman);
                }
                break;
            default:
                return;
        }
    }
}
