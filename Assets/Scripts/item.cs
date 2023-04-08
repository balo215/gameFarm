using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private string itemName;
    private string description;
    private int itemValue;

	public ItemData itemData;    
    public Sprite icon;
    public GameObject itemPrefab;
    public int quantity = 1;
    public bool stackable;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = itemData.itemName;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        itemName = itemData.itemName;
        description = itemData.description;
        itemValue = itemData.sellPrice;
        icon = itemData.icon;
        itemPrefab = itemData.itemPrefab;
        stackable = itemData.stackable;
    }

    public item(ItemData itemData){
    	this.itemData = itemData;
    }

    public string GetItemName()
    {
        return itemName;
    }
    
    public string GetItemDescription()
    {
        return description;
    }
    
    public int GetItemValue()
    {
        return itemValue;
    }

    public ItemData GetItemData(){
    	return itemData;
    }

    public void SetItemData(ItemData newItemData){
    	itemData = newItemData;
    	gameObject.name = itemData.itemName;
    	GetComponent<SpriteRenderer>().sprite = itemData.icon;
    	// other initialization code for the item object based on the itemData
	}

	public void Use(){
		switch(itemData.itemType){
			case ItemData.ItemType.hoe:
				Debug.Log("using hoe");
				break;
			case ItemData.ItemType.seed:
				Debug.Log("using seed");
				break;
		}
	}

}