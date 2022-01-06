using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames;
    public string destinationScene;
    protected override void OnCollide(Collider2D coll)
    {
        if(coll.name != "Player")
            return;

        Vector3 spawn = GameObject.Find("SpawnPoint").transform.position;

        GameManager.instance.portalSound.Play();
        
        if(coll.gameObject.transform.position == spawn)
            return;
            
        //Teleport the player
        GameManager.instance.SaveState();
        UnityEngine.SceneManagement.SceneManager.LoadScene(destinationScene);
    }
}
