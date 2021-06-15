using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xInput;
    [SerializeField] float movementSpeed;
    [SerializeField] BallScript ball;
    [SerializeField] Transform rightStop, leftStop;
    Camera mainCamera;
    float startXValue;
    bool canFireBall = true;
    bool pressed;
    void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
        startXValue = transform.position.x;
    }
    
    void Update()
    {
        //Interprets the input from the A,D and arrow keys for movement.
        if(!pressed)
            xInput = Input.GetAxisRaw("Horizontal");

        //Shoots the ball when the user moves or presses spacebar.
        if (xInput != 0 && canFireBall && Gamemanager.instance.CanIMove())
        {
            canFireBall = false;
            ball.LaunchBall(new Vector2(xInput,1));
        }
    }

    void FixedUpdate()
    {
        //Makes sure none of the code below is being run if the player cant move.
        if (!Gamemanager.instance.CanIMove())
            return;

        //Handles the player movement and moves the player untill thay hit the edge of the stop objects in the scene.
        if (xInput > 0 && (transform.position.x + (transform.localScale.x / 2)) + xInput * movementSpeed * Time.fixedDeltaTime < rightStop.position.x)
            transform.position += new Vector3(xInput * movementSpeed * Time.fixedDeltaTime, 0,0);
        if (xInput < 0 && (transform.position.x - (transform.localScale.x / 2)) + xInput * movementSpeed * Time.fixedDeltaTime > leftStop.position.x)
            transform.position += new Vector3(xInput * movementSpeed * Time.fixedDeltaTime, 0, 0);
    }
    public void PaddleCollision(BallScript ballHitting)
    {
        ballHitting.SetMoveDirection(new Vector2(ballHitting.transform.position.x - transform.position.x, 1).normalized);
    }
    public void ButtonPressed(int newDirection)
    {
        pressed = true;
        xInput = newDirection;
    }
    public void ButtonReleased()
    {
        pressed = false;
    }

    public void ResetPlayer()
    {
        //Resets the player and the ball, called from the gamemanager.
        transform.position = new Vector2(startXValue, transform.position.y);
        //ball.SetMoveDirection(Vector2.zero);
        ball.transform.position = new Vector2(startXValue, transform.position.y +0.5f);
        canFireBall = true;
    }
}
