using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public item Item;
    public ItemData currentItem;
    public Text quantityText;


    // Start is called before the first frame update
    void Start()
    {
        itemIcon.sprite = null;
    	itemIcon.enabled = false;
    	quantityText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(ItemData itemToAdd, item fullItem){
    	//Debug.Log(newItem.itemData);
    	if (itemToAdd == null){
        	itemIcon.sprite = null;
        	itemIcon.enabled = false;
        	currentItem = null;
			//dropButton.gameObject.SetActive(false);
        	return;
    	}
    	itemIcon.sprite = itemToAdd.icon;
    	itemIcon.enabled = true;
    	currentItem = itemToAdd;
    	Item = fullItem;
    	Debug.Log(fullItem.quantity);
    	quantityText.text = fullItem.quantity.ToString();
	}
	public bool IsEmpty(){
		return currentItem == null;
	}
}
