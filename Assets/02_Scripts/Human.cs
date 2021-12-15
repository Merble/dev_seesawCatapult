using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum HumanType
{
    Fat,
    Thin
}

public class Human : MonoBehaviour
{
    public enum HumanState
    {
        Idle,
        RandomMove,
        IsMovingToCatapult,
        OnCatapult,
        IsFlying,
        OnOtherSide,
        IsMovingToSeesaw,
        OnSeesaw
    }
    
    [SerializeField] private CapsuleCollider _CapsuleCollider;
    [SerializeField] private Rigidbody _Rigidbody;
    [SerializeField] private HumanType _Type;
    [SerializeField] private Human _Prefab;
    [Space]
    [SerializeField] private float _Mass;
    [SerializeField] private float _ColliderRadiusMin;
    [SerializeField] private float _ColliderRadiusMax;
    [Space]
    [ShowInInspector, ReadOnly] private HumanState _state = HumanState.Idle;

    private int? _randomMoveTweenId;
    private Vector3 _moveSpot;

    private float _minMoveSpeed;
    private float _maxMoveSpeed;
    private float _maxX;
    private float _minX;
    private float _maxZ;
    private float _minZ;

    public Human Prefab => _Prefab;
    public HumanType Type => _Type;
    public Rigidbody Rigidbody => _Rigidbody;
    public float Mass => _Mass;

    public void MoveToCatapult(Catapult catapult)
    {
        _state = HumanState.IsMovingToCatapult;
        
        if(_randomMoveTweenId != null) LeanTween.cancel(_randomMoveTweenId.Value);
        
        var pos = catapult.transform.position;
        var moveDuration = Vector3.Distance(transform.position, pos) / _maxMoveSpeed;

        LeanTween.move(gameObject, pos, moveDuration).setOnComplete(() =>
        {
            catapult.DidHumanCome(this);
        });
    }
    
    public void MoveToSeesaw(SeesawPad seesaw)
    {
        _state = HumanState.IsMovingToSeesaw;
        
        if(_randomMoveTweenId != null) LeanTween.cancel(_randomMoveTweenId.Value);

        var pos = transform.position;
        var newPos = seesaw.transform.position;
        var moveDuration = Vector3.Distance(pos, newPos) / _maxMoveSpeed;
        
        LeanTween.move(gameObject, newPos, moveDuration).setOnComplete(() =>
        {
            _state = HumanState.OnSeesaw;
            seesaw.AddHuman(this);
        });
    }
    
    public IEnumerator MoveRandomLocation()
    {
        if (_state != HumanState.RandomMove) yield break;
        
        var position = transform.position;
        var posX = Random.Range(_minX, _maxX);
        var posZ = Random.Range(_minZ, _maxZ);
        _moveSpot = new Vector3(posX, position.y, posZ);
            
        var moveSpeed = Random.Range(_minMoveSpeed, _maxMoveSpeed);
        var moveDuration = Vector3.Distance(position, _moveSpot) / moveSpeed;

        _randomMoveTweenId = LeanTween.move(gameObject, _moveSpot, moveDuration).id;

        yield return new WaitForSeconds(moveDuration);
        StartCoroutine(MoveRandomLocation());
    }
    
    public void MakeColliderSmaller()
    {
        _CapsuleCollider.radius = _ColliderRadiusMin;
        //_CapsuleCollider.enabled = false;
    }
    public void MakeColliderBigger()
    {
        //_CapsuleCollider.enabled = true;
        _CapsuleCollider.radius = _ColliderRadiusMax;
    }

    public void SetMinAndMaxValues(float minX, float maxX, float minZ, float maxZ, float minSpeed, float maxSpeed)
    {
        _minX = minX;
        _maxX = maxX;
        
        _minZ = minZ;
        _maxZ = maxZ;
        
        _minMoveSpeed = minSpeed;
        _maxMoveSpeed = maxSpeed;
    }
    
    public HumanState GetState()
    {
        return _state;
    }
    public void SetState(HumanState newState)
    {
        _state = newState;
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckIfGrounded(other.gameObject);
    }

    private void CheckIfGrounded(GameObject other)
    {
        if (!other.GetComponent<Board>()) return;
        
        if (_state != HumanState.IsFlying) return;
        
        _state = HumanState.OnOtherSide;
        MakeColliderBigger();
    }

}
