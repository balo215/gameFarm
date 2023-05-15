using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallInventory : MonoBehaviour
{
    public List<sInventorySlot> slots;
    public List<InventorySlot> iSlots;
    public GameObject pausedBackground;


    [SerializeField] private RectTransform shadowRect;
    private sInventorySlot selectedSlot;

    public int selectedIndex = 0;
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
        if(Input.GetKeyDown("space") && !pausedBackground.activeSelf)
        {
            slots[selectedIndex].useButtonKey();
        }
        if (Input.GetMouseButtonDown(0))
        {
            slots[selectedIndex].useButtonKey();
        }
    }
}
