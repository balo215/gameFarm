using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class sInventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public item Item;
    public ItemData currentItem;
    public Button dropButton;
    public Button useButton;
    public Text quantityText;

	public Character character;
   
    private void Start(){
    	dropButton.onClick.AddListener(dropButtonClick);
    	useButton.onClick.AddListener(useButtonClick);
    	itemIcon.sprite = null;
    	itemIcon.enabled = false;
    	dropButton.gameObject.SetActive(false);
    	quantityText.text = "";

    }


    public void ClearSlot(){
        //Item = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        currentItem = null;
        dropButton.gameObject.SetActive(false);
        Item = null;
    	quantityText.text = "";
    }

    public void SetItem(ItemData itemToAdd, item fullItem){
    	//Debug.Log(newItem.itemData);
    	if (itemToAdd == null){
        	itemIcon.sprite = null;
        	itemIcon.enabled = false;
        	currentItem = null;
			dropButton.gameObject.SetActive(false);
        	return;
    	}
    	itemIcon.sprite = itemToAdd.icon;
    	itemIcon.enabled = true;
    	currentItem = itemToAdd;
    	Item = fullItem;
    	dropButton.gameObject.SetActive(true);
    	quantityText.text = fullItem.quantity.ToString();
	}

	public void stackItem(item fullItem){
		Item.quantity = fullItem.quantity+Item.quantity;
		quantityText.text = Item.quantity.ToString();
	}

	public bool IsEmpty(){
		return currentItem == null;
	}
	public bool isStackable(ItemData itemToAdd){
		if(currentItem.stackable && itemToAdd.itemName == currentItem.itemName){ 
			return true;
		}
		return false;
	}
	private void dropButtonClick(){
		int randomValue1 = Random.Range(-3, 4);
		int randomValue2 = Random.Range(-3, 4);
        Vector2 spawnPosition = character.transform.position + new Vector3(randomValue1, randomValue2); // Add a 5 unit offset to the character's position
        GameObject newItemObject = Instantiate(currentItem.itemPrefab, spawnPosition, Quaternion.identity);
        item newItem = newItemObject.GetComponent<item>();
        newItem.quantity = Item.quantity;
        newItem.SetItemData(currentItem);
		ClearSlot();
		quantityText.text = "";
	}
	private void useButtonClick(){
		if(currentItem != null){
			Item.Use();
			if(currentItem.DOU){
				if(Item.quantity == 1){
					ClearSlot();
				}else{
					Item.quantity--;
					quantityText.text = Item.quantity.ToString();
				}
			}
		}
	}

}
