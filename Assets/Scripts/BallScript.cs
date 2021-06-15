using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float ballSpeed;
    [SerializeField] LayerMask ballLayer;
    float ballRadius = 0.2f;
    public Vector2 moveDirection;
    Transform lastHit;
    Queue<Vector2> hitPositions = new Queue<Vector2>();
    private void Start()
    {
        ballRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
    }
    public void LaunchBall(Vector2 launchDirection)
    {
        StopAllCoroutines();
        moveDirection = launchDirection.normalized;
        StartCoroutine(CollisionCalculation(transform.position, moveDirection));
    } 
    IEnumerator CollisionCalculation(Vector2 origin, Vector2 direction)
    {
        if (!Gamemanager.instance.CanIMove())
            yield break;

        RaycastHit2D hit;
        if(hit = Physics2D.CircleCast(origin, ballRadius, direction, direction.sqrMagnitude * ballSpeed * Time.fixedDeltaTime))
        {
            float distanceToHit = Vector2.Distance(hit.centroid, origin);
            float distanceTotal = direction.sqrMagnitude * ballSpeed * Time.fixedDeltaTime;
            float distanceToAdd = distanceTotal - distanceToHit;

            if (hit.transform.CompareTag("Portal"))
            {
                transform.position = (Vector2)transform.position + moveDirection * ballSpeed * Time.fixedDeltaTime;
                hit.transform.GetComponentInParent<Portal>().ActivatedPortal(transform, hit.transform);
                yield break;
            }

            Vector2 outVector = Vector2.Reflect(direction, hit.normal).normalized;
            transform.position = hit.centroid + outVector * distanceToAdd;
            moveDirection = outVector;
            if(hit.transform != lastHit && hit.transform.CompareTag("Brick"))
            {
                hit.transform.GetComponent<Brick>().TakeDamage(1);
            }
            if (hit.transform.CompareTag("Player"))
                hit.transform.GetComponent<PlayerController>().PaddleCollision(this);

            lastHit = hit.transform;
        }
        else
            transform.position = (Vector2)transform.position + moveDirection * ballSpeed * Time.fixedDeltaTime;

        yield return new WaitForFixedUpdate();
        StartCoroutine(CollisionCalculation(transform.position, moveDirection));
    }
    void BallPredicitionLine(Vector2 origin, Vector2 direction, int depth)
    {
        if (depth > 30)
            return;

        RaycastHit2D hit;
        if (hit = Physics2D.CircleCast(origin, ballRadius, direction, 50))
        {
            Vector2 outVector = Vector2.Reflect(direction, hit.normal.normalized).normalized;
            hitPositions.Enqueue(hit.centroid);
            Debug.DrawRay(hit.point, hit.normal.normalized, Color.blue, 50);

            depth++;
            Debug.DrawLine(origin, hit.centroid, Color.red, 50);
            BallPredicitionLine(hit.centroid, outVector, depth);
        }
    }
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
    public void SetMoveDirection(Vector2 newMoveDirection)
    {
        moveDirection = newMoveDirection;
    }
}
