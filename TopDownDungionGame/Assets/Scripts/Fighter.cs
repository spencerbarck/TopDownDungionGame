using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitPoints;
    public int maxHitPoints = 10;
    public float pushRecoverySpeed = 1f;

    // Immunity
    protected float immuneTime = 0.4f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    //All fighters can take damage / die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if(Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitPoints -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 30, Color.red, transform.position, Vector3.zero, 0.5f);

            if(hitPoints <= 0)
            {
                hitPoints = 0;
                Death();
            }
        }
    }
    protected virtual void Death()
    {

    }
}
