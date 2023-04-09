using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallInventory : MonoBehaviour
{
    public List<sInventorySlot> slots;
    public List<ItemData> items = new List<ItemData>();
    
    [SerializeField] private RectTransform shadowRect;
    [SerializeField] private Image shadowImage;
    private sInventorySlot selectedSlot;
    public GridLayoutGroup gridLayoutGroup;
    private int selectedIndex = 0;

    void Start(){
        //In order to make this (from this comment to the next) to work, i had to disable the GridLayoutGroup
        RectTransform firstSlotRect = slots[selectedIndex].GetComponent<RectTransform>();
        shadowRect.anchoredPosition = firstSlotRect.anchoredPosition;
        //remember if more slots needs to be added, 
        //enable GridLayoutGroup so i wont need to manually place the slots
    }

    void Update(){
        if (Input.mouseScrollDelta.y > 0){
            if(selectedIndex < 8){
                selectedIndex++;
                RectTransform firstSlotRect = slots[selectedIndex].GetComponent<RectTransform>();
                shadowRect.anchoredPosition = firstSlotRect.anchoredPosition;
            }
        }
        else if (Input.mouseScrollDelta.y < 0){
            if(selectedIndex > 0){
                selectedIndex--;
                RectTransform firstSlotRect = slots[selectedIndex].GetComponent<RectTransform>();
                shadowRect.anchoredPosition = firstSlotRect.anchoredPosition;
            }
        }
        if(Input.GetKeyDown("space")){
            slots[selectedIndex].useButtonClick();
        }
    }

    public bool AddItem(ItemData newItem, item fullItem){
        sInventorySlot stackSlot = GetStackSlot(newItem);
        if(stackSlot == null){
            sInventorySlot slot = GetEmptySlot();
            if (slot != null){
                slot.SetItem(newItem, fullItem);
                return true;
            }else{
                Debug.Log("full inventory");
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
    
}
