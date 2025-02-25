using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive) {
            PlayerController.instance.canMove = false;
        } else {
            PlayerController.instance.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.J)){
            // AddItem("Iron Chestplate");
            // AddItem("Nossink");

            // RemoveItem("HP Potion");
            // RemoveItem("Evreysink");
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            LoadData();
        }
    }

    public Item GetItemDetails(string itemToGrab) {
        for (int i = 0; i < referenceItems.Length; i++) {
            if (referenceItems[i].itemName == itemToGrab) {
                return referenceItems[i];
            }
        }
        
        
        return null;
    }

    public void SortItems() {
        bool itemAfterSpace = true;

        while (itemAfterSpace) {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++) {
                if (itemsHeld[i] == "") {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i+1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "") {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd) {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++) {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd) {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace) {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++) {
                if (referenceItems[i].itemName == itemToAdd) {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }
            if (itemExists) {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            } else {
                Debug.LogError(itemToAdd + " does not exist in this world!!!");
            }
        }
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove) {
        bool foundItem = false;
        int itemPosition = 0;
        for (int i = 0; i < itemsHeld.Length; i++) {
            if (itemsHeld[i] == itemToRemove) {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if (foundItem) {
            numberOfItems[itemPosition]--;
            if (numberOfItems[itemPosition] <= 0) {
                itemsHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowItems();
        } else {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void SaveData() {
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_X", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_Y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_Z", PlayerController.instance.transform.position.z);

        //save character info
        for (int i = 0; i < playerStats.Length; i++) {
            if (playerStats[i].gameObject.activeInHierarchy) {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            } else {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentExp);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
            
        }

        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++) {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    public void LoadData() {
        SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_X"),
                                                                   PlayerPrefs.GetFloat("Player_Position_Y"),
                                                                   PlayerPrefs.GetFloat("Player_Position_Z"));

        for (int i = 0; i < playerStats.Length; i++) {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0) {
                playerStats[i].gameObject.SetActive(false);
            } else {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentExp = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        for (int i = 0; i < itemsHeld.Length; i++) {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }

}
