using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public static GameMenu instance;

    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerStats;

    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

    public GameObject[] statusButtons;

    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImage;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDesc, useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public Text goldText;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2")) {
            if (theMenu.activeInHierarchy) {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;
                CloseMenu();
            } else {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }
    }

    public void UpdateMainStats() {
        playerStats = GameManager.instance.playerStats;

        for(int i = 0; i < playerStats.Length; i++) {
            if (playerStats[i].gameObject.activeInHierarchy) {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentExp + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentExp;
                charImage[i].sprite = playerStats[i].charImage;

            } else {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";
    }

    public void ToggleWindow(int windowNumber) {
        UpdateMainStats();
        for (int i = 0; i < windows.Length; i++) {
            if (i == windowNumber) {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            } else {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu() {
        for(int i = 0; i < windows.Length; i++) {
            windows[i].SetActive(false);
        }
         theMenu.SetActive(false);

         GameManager.instance.gameMenuOpen = false;

         itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus() {
        UpdateMainStats();

        StatusChar(0);

        for (int i = 0; i < statusButtons.Length; i++) {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);

            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void StatusChar(int selected) {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if (playerStats[selected].equippedWpn != "") {
            statusWpnEqpd.text = playerStats[selected].equippedWpn;
        }
        
        statusWpnPwr.text = playerStats[selected].wpnPwr.ToString();
        
        if (playerStats[selected].equippedArmr != "") {
            statusArmrEqpd.text = playerStats[selected].equippedArmr;
        }
        
        statusArmrPwr.text = playerStats[selected].armrPwr.ToString();
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentExp).ToString();
        statusImage.sprite = playerStats[selected].charImage; 
    }

    public void ShowItems() {

        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++) {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "") {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            } else {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem) {
        activeItem = newItem;

        if (activeItem.isItem) {
            useButtonText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmour) {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDesc.text = activeItem.itemDesc;
    }

    public void DiscardItem() {
        if (activeItem != null) {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice() {
        itemCharChoiceMenu.SetActive(true);
        for (int i = 0; i < itemCharChoiceNames.Length; i++) {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    
    }

    public void CloseItemCharChoice() {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar) {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

public void PlayButtonSound(int soundToPlay) {
    AudioManager.instance.PlaySFX(soundToPlay);
}


}
