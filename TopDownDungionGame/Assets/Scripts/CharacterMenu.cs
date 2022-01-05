using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitPointText, goldText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Character Selection
    public void OnArrowClick(bool right)
    {
        if(right)
        {
            currentCharacterSelection++;

            //if we went to far in sprite array
            if(currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;
            
            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            //if we went to far in sprite array
            if(currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            
            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon upgrade
    public void OnUpgradeClick()
    {
        if(GameManager.instance.TryUpgradeWeapon())
            GetComponent<AudioSource>().Play();
            UpdateMenu();
    }

    // Update the character information



    public void UpdateMenu()
    {
        //Weapon
        if(GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
        {
            weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
            upgradeCostText.text = "MAX";
        }
        else
        {
            weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel+1];
            upgradeCostText.text =  GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }

        //Meta
        hitPointText.text = GameManager.instance.player.hitPoints.ToString()+" / "+GameManager.instance.player.maxHitPoints.ToString();
        goldText.text = GameManager.instance.gold.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        // xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();

        if(currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total expirience points";// Display total xp
            xpBar.localScale = new Vector3(1,1,1);
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio,1,1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }

    }
}
