using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviorManager : MonoBehaviour
{
    public GameObject chestPrefab;
    private static ItemBehaviorManager instance;
    public GameObject inventoryPanel;
    public GameObject backgroundInventory;

    private void Awake()
    {
        instance = this;
    }
// Start is called before the first frame update
    public static void Use(ItemData item){
    // Implement the behavior of the item when used
        if(item != null){
        	Debug.Log("Item used from im: " + item.itemType);
        	if(item.itemType == ItemData.ItemType.chest){
                Debug.Log(item.itemName);
                Debug.Log(item.itemPrefab);
                if (instance != null){
            // Access the non-static members and functions of ItemBehaviorManager
                    instance.spawnItem();
                }
                //GameObject chest = Instantiate(item.itemPrefab, spawnPosition, Quaternion.identity);
                //chest.GetComponent<item>().SetItemData(item);

            }
        }
    }
    public void spawnItem(){
        Vector2 spawnPosition = new Vector2(-1f, 0f); 
        Debug.Log(chestPrefab);
        GameObject chest = Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        chest.transform.SetAsLastSibling();
        chestManager cManager = chest.transform.gameObject.GetComponent<chestManager>();
        cManager.setInventoryPanel(inventoryPanel, backgroundInventory);
        //Debug.Log(chest.transform.GetChild(0).gameObject);

    }


}
