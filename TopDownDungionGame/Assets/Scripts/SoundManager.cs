using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    public AudioSource goblinDeath;
    [SerializeField]
    public AudioSource bossGoblinDeath;
    [SerializeField]
    public AudioSource playerDeath;

    private void Awake()
    {
        if(SoundManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance=this;
        }
    }
}
