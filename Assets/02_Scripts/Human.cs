using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Human : MonoBehaviour
{
    [SerializeField] private float _Mass;
    
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
    
    private void Awake()
    {
        //GetComponent<Rigidbody>().mass = _Mass;
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

        LeanTween.move(gameObject, MoveSpot, moveDuration);

        yield return new WaitForSeconds(moveDuration);
        StartCoroutine(MoveRandomLocation());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Board" && !IsOnRandomMove)
        {
            GetComponent<Rigidbody>().mass = _Mass;
        }
    }
}
