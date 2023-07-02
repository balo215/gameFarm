using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
	public List<sInventorySlot> slots;
    public List<InventorySlot> iSlots;
    public GameObject inventoryPanel;
    public bool isInventoryOpen = false;
    public bool isChestOpen = false;
    public chestManager ChestManager;
// https://chat.openai.com/share/a14ba7f2-c3be-4e8c-ab86-bd0e39e7b10e

    void Start()
    {
    	inventoryPanel.SetActive(false);
    }

    void Update(){

        if(isChestOpen == true){
            foreach (sInventorySlot slot in slots){
                slot.showChestBtn();
            }
            foreach (InventorySlot slot in iSlots){
                slot.showChestBtn();
            }
        }else{
            foreach (InventorySlot slot in iSlots){
                slot.hiddeChestBtn();
            }
            foreach (sInventorySlot slot in slots){
                slot.hiddeChestBtn();
            }
        }
        if(isInventoryOpen == true){
            foreach(sInventorySlot slot in slots){
                slot.showDropBtn();
            }
        }
        if(isInventoryOpen == false){
            foreach(sInventorySlot slot in slots){
                slot.hideDropBtn();
            }
        }
    }

    // Start is called before the first frame update
    /*
    public bool AddItem(ItemData newItem, item fullItem){
        sInventorySlot stackSlot = GetStackSlot(newItem);
        if(stackSlot == null){
            sInventorySlot slot = GetEmptySlot();
            if (slot != null){
                slot.SetItem(newItem, fullItem);
                return true;
            }else{
                Debug.Log("full inventory");
                //this script could work as an Inventory Manager
                InventorySlot stackInvSlot = GetInventoryStackSlot(newItem);
                if(stackInvSlot == null){
                	InventorySlot Slot = GetInventorySlot(newItem);
	                if(Slot != null){
	                    Slot.SetItem(newItem, fullItem);
	                    return true;
	                }
                	return false;
                }else{
                	stackInvSlot.stackItem(fullItem);
                	return true;
                }
            }
        }else{
            stackSlot.stackItem(fullItem);
            return true;
        }
    }
*/
    public bool AddItemToInv(ItemData newItem, item fullItem){
    	sInventorySlot stackSmallSlot = GetStackSlot(newItem);
    	InventorySlot stackSlot = GetInventoryStackSlot(newItem);
    	if(stackSmallSlot == null && stackSlot == null){
    		//add in an empty slot
    		sInventorySlot slot = GetEmptySlot();
    		if(slot != null){
    			slot.SetItem(newItem, fullItem);
    			return true;
    		}else{
    			InventorySlot invSlot = GetInventorySlot(newItem);
    			if(invSlot != null){
    				invSlot.SetItem(newItem, fullItem);
    				return true;
    			}else{
    				Debug.Log("full inventory");
    				return false;
    			}
    		}
    	}else if(stackSmallSlot != null){
    		stackSmallSlot.stackItem(fullItem);
    		return true;
    	}else if(stackSlot != null){
    		stackSlot.stackItem(fullItem);
    		return true;
    	}else{
    		return false;
    	}
    }

    private sInventorySlot GetEmptySlot(){
        foreach (sInventorySlot slot in slots){
            if (slot.IsEmpty()){
                return slot;
            }
        }
        return null;
    }

    private sInventorySlot GetStackSlot(ItemData newItem){
        foreach (sInventorySlot slot in slots){
            if(!slot.IsEmpty() && slot.isStackable(newItem)){
                return slot;
            }
        }
        return null;
    }

    private InventorySlot GetInventoryStackSlot(ItemData newItem){
    	foreach(InventorySlot slot in iSlots){
    		if(!slot.IsEmpty() && slot.isStackable(newItem)){
    			return slot;
    		}
    	}
    	return null;
    }

    public InventorySlot GetInventorySlot(ItemData newItem){
        foreach(InventorySlot slot in iSlots){
    		Debug.Log(slot);
            if(slot.IsEmpty()){
                return slot;
            }
        }
        return null;
    }

   public void sendItem(ItemData itemData, int quantity){
        ChestManager.sendItem(itemData, quantity);
    }

    public void setChestManager(chestManager CM){
        ChestManager = CM;
    }


}
