using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;
    private float colorFade = 1f;
    public bool isAlive = true;
    protected override void Start()
    {
        base.Start();
        GameManager.instance.OnHitpointChange();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
    }
    protected override void ReceiveDamage(Damage dmg)
    {
        if(!isAlive) return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(isAlive)UpdateMoter(new Vector3(x,y,0));

        if(!isAlive)
        {
            colorFade *= 0.9f;

            //fade player
            spriteRenderer.color = new Color(spriteColor.r,spriteColor.g,spriteColor.b,colorFade);

            if(FindObjectOfType<CameraMoter>().boundX > 0f)FindObjectOfType<CameraMoter>().boundX *= 0.99f;
            if(FindObjectOfType<CameraMoter>().boundY > 0f)FindObjectOfType<CameraMoter>().boundY *= 0.99f;

            if(FindObjectOfType<Camera>().orthographicSize>0.5f)FindObjectOfType<Camera>().orthographicSize *= 0.99f;
        }
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitPoints++;
        hitPoints = maxHitPoints;
        GameManager.instance.OnHitpointChange();
    }

    public void SetLevel(int level)
    {
        for(int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        hitPoints += healingAmount;
        if(hitPoints > maxHitPoints)
        {
            int overHealAmount = hitPoints-maxHitPoints;
            int healingAmountNoExcess = healingAmount-overHealAmount;
            GameManager.instance.ShowText(healingAmountNoExcess.ToString(), 15, Color.green, GameManager.instance.player.transform.position, Vector3.up * 40,1.0f);
            hitPoints=maxHitPoints;
        }
        else 
        {
            GameManager.instance.ShowText(healingAmount.ToString(), 30, Color.green, GameManager.instance.player.transform.position, Vector3.up * 40,1.0f);
        }
        GameManager.instance.OnHitpointChange();
    }

    public void Respawn()
    {
        hitPoints=maxHitPoints;
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;

        GameManager.instance.OnHitpointChange();

        //unfade player
        colorFade = 1;
        spriteRenderer.color = new Color(spriteColor.r,spriteColor.g,spriteColor.b,colorFade);
    }
}
