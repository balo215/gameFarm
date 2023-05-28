using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviorManager : MonoBehaviour
{
    public GameObject chestPrefab;

// Start is called before the first frame update
    public static void Use(ItemData item){
    // Implement the behavior of the item when used
        if(item != null){
        	Debug.Log("Item used from im: " + item.itemType);
        	if(item.itemType == ItemData.ItemType.chest){
                Debug.Log(item.itemName);
                Debug.Log(item.itemPrefab);

                Vector2 spawnPosition = new Vector2(-1f, 0f); 
                GameObject chest = Instantiate(item.itemPrefab, spawnPosition, Quaternion.identity);
                chest.GetComponent<item>().SetItemData(item);

            }
        }
    }


}
