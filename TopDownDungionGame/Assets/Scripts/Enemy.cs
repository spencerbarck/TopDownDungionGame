using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Expirence
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private bool killedPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    // Audio timer
    private float idleSoundTimer = 4f;
    private float idleSoundTimerLength = 4f;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected void FixedUpdate()
    {
        //Play idle sound on loop
        if(idleSoundTimer<idleSoundTimerLength)
        {
            idleSoundTimer+= Time.deltaTime;
        }else if(chasing){
            idleSoundTimer=0f;
            GetComponent<AudioSource>().Play();
        }

        //If the player is dead return to guard spot
        if(killedPlayer)
        {
            UpdateMoter(startingPosition - transform.position);
            return;
        }

        //Check if enemy is too far from guard position (has chased player too far away) ---
        if(Vector3.Distance(playerTransform.position,startingPosition) < chaseLength)
        {
            
            //Check if player is close enough to provoke an attack/chase
            if(Vector3.Distance(playerTransform.position, startingPosition)< triggerLength)
            {
                chasing=true;
            }

            if (chasing)
            {
                //If not colliding attack player
                if(!collidingWithPlayer)
                {
                    Vector3 attackMoveVector = (playerTransform.position - transform.position);
                    if((Mathf.Abs(attackMoveVector.x)<0.1f)&&(Mathf.Abs(attackMoveVector.y)<0.1f))
                    {
                        Debug.Log("hit");
                        UpdateMoter(playerTransform.position - transform.position);
                    }
                    else
                        UpdateMoter((playerTransform.position - transform.position).normalized);
                }
                else
                {
                    UpdateMoter(startingPosition - transform.position);
                }
            }
            else
            {
                UpdateMoter(startingPosition - transform.position);
            }
        }
        //If chased player to far away return to guard position ---
        else
        {
            UpdateMoter(startingPosition-transform.position);
            chasing = false;
        }

        collidingWithPlayer = false;

        // check player collision
        // collision work
        boxCollider.OverlapCollider(filter,hits);
        for(int i=0;i<hits.Length;i++)
        {
            if(hits[i]==null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
                if(hits[i].gameObject.GetComponent<Player>().isAlive == false) killedPlayer=true;

            }

            //clean array
            hits[i]=null;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue+ " xp", 30, Color.magenta, transform.position, Vector3.up * 40,1.0f);
    }
}
