using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;
    private float colorFade = 1f;
    public bool isAlive = true;
    [SerializeField]
    private AudioSource playerWalkAudio;
    [SerializeField]
    private AudioSource playerDamageAudio;
    public string playerDirection = "left/right";

    [SerializeField]
    private Sprite lookingRightSprite;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite backSprite;
    [SerializeField]
    private Weapon playerWeapon;
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

        if(!playerDamageAudio.isPlaying) playerDamageAudio.Play();

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    protected override void Death()
    {
        isAlive = false;
        SoundManager.instance.playerDeath.Play();
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(isAlive)
        {
            switch(y)
            {
                case -1:
                    spriteRenderer.sprite=frontSprite;
                    //playerWeapon.transform.position = new Vector3(-0.05f,0.053f,0f);
                    playerWeapon.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
                    playerWeapon.anim.SetTrigger("FacingDown");
                    playerDirection = "down";
                    break;
                case 1:
                    spriteRenderer.sprite=backSprite;
                    //playerWeapon.transform.position = new Vector3(-0.05f,0.053f,0f);
                    playerWeapon.GetComponent<SpriteRenderer>().sortingLayerName = "Actor";
                    playerWeapon.anim.SetTrigger("FacingUp");
                    playerDirection = "up";
                    break;
                case 0:
                    if(Mathf.Abs(x)>0)
                    {
                        spriteRenderer.sprite=lookingRightSprite;
                        //playerWeapon.transform.position = new Vector3(-0.025f,0.053f,0f);
                        playerWeapon.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
                        playerWeapon.anim.SetTrigger("FacingLeftRight");
                        playerDirection = "left/right";
                    }
                    break;
                default:
                    break;
            }
            
            UpdateMoter(new Vector3(x,y,0));

            // Play walking sound
            if(((Mathf.Abs(x)>0)||(Mathf.Abs(y)>0))&&!playerWalkAudio.isPlaying)playerWalkAudio.Play();
        }

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
        SoundManager.instance.playerDeath.Stop();

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
