using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	public List<sInventorySlot> slots;
    public List<InventorySlot> iSlots;
    // Start is called before the first frame update
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
                InventorySlot Slot = GetInventorySlot(newItem);
                if(Slot != null){
                    Slot.SetItem(newItem, fullItem);
                    return true;
                }
                return false;
            }
        }else{
            stackSlot.stackItem(fullItem);
            return true;
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

    public InventorySlot GetInventorySlot(ItemData newItem){
        foreach(InventorySlot slot in iSlots){
            if(slot.IsEmpty()){
                Debug.Log(slot);
                return slot;
            }
        }
        return null;
    }
}
