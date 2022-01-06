using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    private float[] minionsSpeed = {3f, -3f};
    [SerializeField]
    private float minionDistance = 0.25f;
    [SerializeField]
    private Transform[] minions;
    private void Update()
    {
        for (int i = 0; i < minions.Length; i++)
        {
            // Minions rotate around boss
            minions[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * minionsSpeed[i]) * minionDistance, Mathf.Sin(Time.time * minionsSpeed[i])* minionDistance, 0);
        }
    }
    protected override void Death()
    {
        Destroy(gameObject);
        SoundManager.instance.bossGoblinDeath.Play();
        GameManager.instance.GrantXp(5);
        GameManager.instance.ShowText("+" + xpValue+ " xp", 30, Color.magenta, transform.position, Vector3.up * 40,1.0f);
    }
}
