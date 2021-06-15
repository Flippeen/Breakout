using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    [SerializeField] int lives;
    [SerializeField] List<Image> livesImages = new List<Image>();
    [SerializeField] Canvas gameoverCanvas, victoryCanvas;
    [SerializeField] ParticleSystem starPs;
    [SerializeField] Image starImage;
    List<PlayerController> players = new List<PlayerController>();
    List<BallScript> balls = new List<BallScript>();
    List<Brick> bricks = new List<Brick>();
    int maxLives, bricksLeft;
    bool canFireBall = true;
    bool canLoseLife = true;
    bool playerCanMove;

    float deltaTime;
    [SerializeField] TextMeshProUGUI fpsText;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        players.AddRange(FindObjectsOfType<PlayerController>());
        balls.AddRange(FindObjectsOfType<BallScript>());
        maxLives = lives;
        gameoverCanvas.enabled = false;
        victoryCanvas.enabled = false;
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1 / deltaTime;
        fpsText.text = Mathf.Round(fps).ToString();

        //When the player starts the game this makes the needed changes in the gamemanager to make the game run.
        if (Input.GetKeyDown(KeyCode.Space) && canFireBall || Input.GetAxisRaw("Horizontal") != 0 && canFireBall || Input.GetMouseButtonDown(0) && canFireBall)
        {
            canFireBall = false;
            playerCanMove = true;
        }
    }

    void LateUpdate()
    {
        //Checks if any of the balls is below the paddles y-value. This makes it possible to have many ball as long as the paddles are all on the same level.
        for (int i = 0; i < players.Count; i++)
        {
            if (balls[i].transform.position.y < players[i].transform.position.y - 1 && canLoseLife || balls[i].transform.position.y > players[i].transform.position.y + 10 && canLoseLife)
                LostLife();
        }
    }
    //Takes a life away from the player and disables one of the life images, also resets the ball.
    public void LostLife()
    {
        foreach (var ball in balls)
        {
            ball.SetMoveDirection(Vector2.zero);
        }
        playerCanMove = false;
        canLoseLife = false;
        lives--;
        livesImages[lives].enabled = false;
        if (lives <= 0)
        {
            foreach (var ball in balls)
            {
                ball.transform.position = new Vector2(-100, 0);
            }
            float currentAmount = (1 - (float)bricksLeft / (float)bricks.Count)*100;
            if(currentAmount >= 60)
                StartCoroutine(GameWon());
            else
                gameoverCanvas.enabled = true;
        }
        else
            StartCoroutine(ResetBall());
    }
    //Used for buttons to use methods since it could not run a IEnumerator.
    public void ResetLevel()
    {
        StopCoroutine("ResetGame");
        StartCoroutine("ResetGame");
    }
    //Resets score,lives and bricks with a short and simple animation.
    IEnumerator ResetGame()
    {
        foreach (var ball in balls)
        {
            ball.transform.position = new Vector2(-400, 0);
            ball.SetMoveDirection(Vector2.zero);
        }
        gameoverCanvas.enabled = false;
        victoryCanvas.enabled = false;
        yield return new WaitForSeconds(0.5f);
        foreach (var brick in bricks)
        {
            yield return new WaitForSeconds(0.005f);
            brick.Respawn();
        }
        lives = maxLives;
        bricksLeft = bricks.Count;
        TextMeshProUGUI amountText = starImage.GetComponentInChildren<TextMeshProUGUI>();
        starImage.fillAmount = 0;
        amountText.text = "";

        foreach (var image in livesImages)
        {
            yield return new WaitForSeconds(0.1f);
            image.enabled = true;
        }
        StartCoroutine(ResetBall());
    }
    //Calls the method in the player to center up with the ball in their starting position.
    IEnumerator ResetBall()
    {
        yield return new WaitForSeconds(0.5f);
        canFireBall = true;
        playerCanMove = false;
        canLoseLife = true;
        foreach (var player in players)
        {
            player.ResetPlayer();
        }
    }
    //Sends the score to the text UI to update it.
    public void BrickWasDestroyed()
    {
        bricksLeft--;
        if(bricksLeft <= 0)
            StartCoroutine(GameWon());
    }
    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var ball in balls)
        {
            ball.transform.position = new Vector2(-100, 100);
            ball.SetMoveDirection(Vector2.zero);
        }
        playerCanMove = false;
        TextMeshProUGUI amountText = starImage.GetComponentInChildren<TextMeshProUGUI>();
        starImage.fillAmount = 0;
        amountText.text = "";
        victoryCanvas.enabled = true;
        yield return new WaitForSeconds(0.5f);

        float precentLeft = (float)bricksLeft/(float)bricks.Count;
        float elapsed = 0.0f;
        while (starImage.fillAmount < (1 - precentLeft))
        {
            yield return new WaitForFixedUpdate();
            float currentAmount = Mathf.Lerp(0, 1 - precentLeft, elapsed / 3);
            starImage.fillAmount = currentAmount;
            elapsed += Time.deltaTime;
            amountText.text = Mathf.Round(currentAmount * 100).ToString() + "%";
        }
        if (starImage.fillAmount == 1)
            starPs.Play();
    }
    //Used by all the bricks to be added to the list so they can be reset at the end of the game.
    public void AddToListOfBricks(Brick newBrick)
    {
        bricks.Add(newBrick);
        bricksLeft++;
    }
    //Gives the bool-value if the player can move or not.
    public bool CanIMove()
    {
        return playerCanMove;
    }
}
