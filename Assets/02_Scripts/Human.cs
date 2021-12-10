using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum HumanType
{
    Fat,
    Thin
}
public class Human : MonoBehaviour
{
    [SerializeField] private float _Mass;
    [SerializeField] private HumanType _Type;
    [SerializeField] private Human _Prefab;

    private int? _randomMoveTweenId;

    public HumanType Type => _Type;
    public Vector3 MoveSpot { get; set; }

    public bool IsOnRandomMove { get; set; }
    public bool IsOnCatapult { get; set; }
    public bool IsOnSeesaw { get; set; }
    
    public float MinMoveSpeed { get; set; }
    public float MaxMoveSpeed { get; set; }
    
    public float MaxX { get; set; }
    public float MinX { get; set; }

    public float MaxZ { get; set; }
    public float MinZ { get; set; }

    public Human Prefab => _Prefab;

    public void MoveTo(Vector3 catapultPos)
    {
        if(_randomMoveTweenId != null) LeanTween.cancel(_randomMoveTweenId.Value);
        IsOnRandomMove = false;

        var moveDuration = Vector3.Distance(transform.position, catapultPos) / MaxMoveSpeed;
        LeanTween.move(gameObject, catapultPos, moveDuration);
    }
    
    public IEnumerator MoveRandomLocation()
    {
        if (!IsOnRandomMove) yield break;
        
        var position = transform.position;
        var posX = Random.Range(MinX, MaxZ);
        var posZ = Random.Range(MinZ, MaxZ);
        MoveSpot = new Vector3(posX, position.y, posZ);
            
        var moveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
        var moveDuration = Vector3.Distance(position, MoveSpot) / moveSpeed;

        _randomMoveTweenId = LeanTween.move(gameObject, MoveSpot, moveDuration).id;

        yield return new WaitForSeconds(moveDuration);
        StartCoroutine(MoveRandomLocation());
    }
}
