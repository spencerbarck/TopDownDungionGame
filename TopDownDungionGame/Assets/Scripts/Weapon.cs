using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct

    public int[] damagePoint = {1,2,3,4,5,6,7,8};
    public float[] pushForce = {4f,4.25f,4.5f,4.75f,5f,5.5f,6f,7f};

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private Animator anim;
    private float cooldown = 0.1f;
    private float lastSwing;
    private float colorFade;

    //Audio
    [SerializeField]
    private AudioSource swingSound;
    [SerializeField]
    private AudioSource hitSound;
    private float hitSoundTimer = 0.2f;
    private float hitSoundTimerLength = 0.2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        SetWeaponLevel(weaponLevel);
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if(hitSoundTimer<hitSoundTimerLength)
        {
            hitSoundTimer+= Time.deltaTime;
        }

        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }

        if(GameManager.instance.player.isAlive == false)
        {
            colorFade *= 0.9f;
            Color weaponColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(weaponColor.r,weaponColor.g,weaponColor.b,colorFade);
        }
        else if(colorFade!=1)
        {
            colorFade = 1;
            Color weaponColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(weaponColor.r,weaponColor.g,weaponColor.b,colorFade);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if(coll.name != "Player")
            {
                
                //Play idle sound on loop
                if(hitSoundTimer>=hitSoundTimerLength)
                {
                    hitSoundTimer=0f;
                    hitSound.Play();
                }

                Damage dmg = new Damage();
                
                dmg.damageAmount = damagePoint[weaponLevel];
                dmg.origin = transform.position;
                dmg.pushForce=pushForce[weaponLevel];

                coll.SendMessage("ReceiveDamage", dmg);
            }
        }
    }

    private void Swing()
    {
        swingSound.Play();
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
