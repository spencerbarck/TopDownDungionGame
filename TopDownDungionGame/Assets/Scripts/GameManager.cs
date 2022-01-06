 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        SpawnPlayer();
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud.gameObject);
            Destroy(menu.gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();

        instance=this;
        SceneManager.sceneLoaded += LoadState;
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;
    public Animator deathMenuAnimator;

    public AudioSource levelUpSound;
    public AudioSource portalSound;

    //Logic
    public int gold;
    public int experience;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg,fontSize,color,position,motion,duration);
    }

    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if(weaponPrices.Count <= weapon.weaponLevel)
            return false;
        
        if(gold >= weaponPrices[weapon.weaponLevel])
        {
            gold -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitPoints / (float)player.maxHitPoints;

        hitpointBar.localScale = new Vector3(1,ratio,1);
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;
            if(r == xpTable.Count) return r;// Max Level
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r<level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        levelUpSound.Play();
        ShowText("Level Up", 50, Color.magenta, player.transform.position, Vector3.up * 100,1.0f);
        player.OnLevelUp();
    }

    //Save State
    /*
     * INT perferedSkin
     * INT gold
     * INT experience
     * INT weaponLevel
     */
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += gold.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode load)
    {
        SpawnPlayer();

        //if(GetCurrentLevel() != 1)
        //{
        //    player.SetLevel(GetCurrentLevel());
        //}

        //Loading from save

        if(!PlayerPrefs.HasKey("SaveState")) 
        {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change Logic
        gold = int.Parse(data[1]);

        // Expirience
        experience = int.Parse(data[2]);

        // Change weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }

    public void SpawnPlayer()
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death Menu Respawn
    public void Respawn()
    {
        deathMenuAnimator.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        deathMenuAnimator.speed=1f;
        player.Respawn();
    }

    public void ShowRespawnButton()
    {
        deathMenuAnimator.speed=1000f;
    }

}
