using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviorManager : MonoBehaviour
{
    // Start is called before the first frame update
        public static void Use(ItemData item)
    {
        // Implement the behavior of the item when used
        if(item != null){
        	Debug.Log("Item used from im: " + item.itemType);
        }


        // Add any other logic or functionality for item usage
        // such as increasing character's life, interacting with tiles, etc.
    }
}
