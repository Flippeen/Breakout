using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform[] portals;

    Transform exitPortal, entryPortal;
    public void ActivatedPortal(Transform ballTransform, Transform newEntryPortal)
    {
        entryPortal = newEntryPortal;
        foreach (var portal in portals)
            if (portal != entryPortal)
                exitPortal = portal;

        Teleport(ballTransform);
    }
    void Teleport(Transform ballTransform)
    {
        ballTransform.position = exitPortal.position;

        BallScript ballScript = ballTransform.GetComponent<BallScript>();
        Vector2 outVector = Vector2.Reflect(ballScript.GetMoveDirection(), entryPortal.right);
        float angleFromRight = Vector2.Angle(outVector, entryPortal.right);
        Vector2 exitVector = Quaternion.Euler(0, 0, -angleFromRight) * exitPortal.right;
        ballScript.LaunchBall(exitVector);
    }
}
