using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] int healthPoints, scoreValue;
    [SerializeField] List<Sprite> breakingStages = new List<Sprite>();
    ParticleSystem particleSystem;
    SpriteRenderer spriteRend;
    Gamemanager gamemanager;
    int maxHealth;
    void Awake()
    {
        particleSystem = transform.GetComponentInParent<ParticleSystem>();
        spriteRend = GetComponent<SpriteRenderer>();
        maxHealth = healthPoints;
        gamemanager = FindObjectOfType<Gamemanager>();
        gamemanager.AddToListOfBricks(this);
    }

    public void TakeDamage(int damageRecived)
    {
        //Takes damage and kills itself if it reaches 0 HP.
        healthPoints -= damageRecived;
        if (healthPoints <= 0)
        {
            Killed();
            return;
        }

        //Checks at what precentage of breaking the blocks are and applies the appropriate sprite.
        int save = (int)(healthPoints * 100.0 / breakingStages.Count);
        switch (save)
        {
            case 75:
                save = 1;
                break;
            case 50:
                save = 2;
                break;
            case 25:
                save = 3;
                break;
            default:
                break;
        }
        spriteRend.sprite = breakingStages[save];
    }

    //Moves the block to the side sa that it wont have to be reloaded when the player restarts the level.
    void Killed()
    {
        particleSystem.Play();
        transform.localPosition = new Vector2(50, 0);
        gamemanager.BrickWasDestroyed();
    }
    //Respawns the block to the position of it's particlesystem parent.
    public void Respawn()
    {
        transform.position = particleSystem.transform.position;
        spriteRend.sprite = breakingStages[0];
        healthPoints = maxHealth;
    }
    //The only thing that can collide with the bricks is the ball but just to be sure I check it before they take damage.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            TakeDamage(1);
        }
    }
}
