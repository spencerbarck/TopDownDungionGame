using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //Damage 
    public int damage = 1;
    public int pushForce = 5;

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter" && coll.name == "Player")
        {
            // Create damage before sedning it to player    

            Damage dmg = new Damage();
                
            dmg.damageAmount = damage;
            dmg.origin = transform.position;
            dmg.pushForce=pushForce;

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
