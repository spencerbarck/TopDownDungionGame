using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] minionsSpeed = {3f, -3f};
    public float minionDistance = 0.25f;
    public Transform[] minions;

    private void Update()
    {
        for (int i = 0; i < minions.Length; i++)
        {
            minions[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * minionsSpeed[i]) * minionDistance, Mathf.Sin(Time.time * minionsSpeed[i])* minionDistance, 0);
        }
        

    }


}
