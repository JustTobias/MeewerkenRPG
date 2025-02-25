using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item selectedItem;
    public Text buyItemName, buyItemDesc, buyItemValue;
    public Text sellItemName, sellItemDesc, sellItemValue;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop() {
        shopMenu.SetActive(true);
        OpenBuyMenu();
        
        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + " G";
    }

    public void CloseShop() {
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu() {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);
        buyItemButtons[0].Press();

        for (int i = 0; i < buyItemButtons.Length; i++) {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "") {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            } else {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu() {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        sellItemButtons[0].Press();

        ShowSellItems();        
    }

    private void ShowSellItems() {
        GameManager.instance.SortItems();

        for (int i = 0; i < sellItemButtons.Length; i++) {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "") {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            } else {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBuyItem(Item buyItem) {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDesc.text = selectedItem.itemDesc;
        buyItemValue.text = "Value: " + selectedItem.value + " G";
    }

    public void SelectSellItem(Item sellItem) {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDesc.text = selectedItem.itemDesc;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + " G";
    }

    public void BuyItem() {
        if (selectedItem != null) {
            if (GameManager.instance.currentGold >= selectedItem.value) {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";
    }

    public void SellItem() {
        if (selectedItem != null) {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);
            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + " G";

        ShowSellItems();
    }

}
