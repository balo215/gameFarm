using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestManager : MonoBehaviour
{

    private int layerMask;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    public GameObject chestPanel;
    public GameObject backgroundInventory;
    public GameObject backgroundChest;
    public List<chestSlot> slots;
    public Canvas canvas;



    // Start is called before the first frame update
    void Start()
    {
        int chestLayer = LayerMask.NameToLayer("Chest"); // Replace "Character" with the name of your character's layer
        layerMask = ~(1 << chestLayer);
        inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager.isChestOpen = false;
        //canvas.sortingOrder = 0;
    }

    public void setInventoryPanel(GameObject invPanel, GameObject backInv){
        inventoryPanel = invPanel;
        backgroundInventory = backInv;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, layerMask);
            if (hit.collider != null){
                if (hit.collider.CompareTag("Player")){
                    inventoryManager.setChestManager(this);
                    
                    if (inventoryManager.isInventoryOpen == true && inventoryManager.isChestOpen == true)
                    {
                        inventoryManager.isChestOpen = false;
                        Vector2 newPosition = new Vector2(inventoryPanel.transform.position.x + 330f, inventoryPanel.transform.position.y);
                        inventoryPanel.transform.position = newPosition;
                        inventoryManager.isInventoryOpen = false;
                        inventoryPanel.transform.localScale = Vector2.one;

                    }
                    else
                    {
                        inventoryManager.isChestOpen = true;
                        Vector2 newPosition = new Vector2(inventoryPanel.transform.position.x - 330f, inventoryPanel.transform.position.y);
                        inventoryPanel.transform.position = newPosition;
                        inventoryManager.isInventoryOpen = true;
                        inventoryPanel.transform.localScale = new Vector2(.7f, .7f);
                        //canvas.sortingOrder = 1;


                    }
                    //isInventoryOpen = !isInventoryOpen;
                    inventoryPanel.SetActive(inventoryManager.isInventoryOpen);
                    chestPanel.SetActive(inventoryManager.isChestOpen);
                    backgroundInventory.SetActive(!inventoryManager.isChestOpen);
                    backgroundChest.SetActive(inventoryManager.isChestOpen);
                    Time.timeScale = inventoryManager.isInventoryOpen ? 0f : 1f;
                }
            }

            Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);

            
        }
    }

    private chestSlot GetEmptySlot(){
        foreach (chestSlot slot in slots){
            if (slot.IsEmpty()){
                return slot;
            }
        }
        return null;
    }

    public bool AddItem(ItemData newItem, int quantity){
        chestSlot stackSlot = GetStackSlot(newItem);
        if(stackSlot == null){
            chestSlot slot = GetEmptySlot();
            if(slot != null){
                slot.SetItem(newItem, quantity);
                return true;
            }else{
                return false;
            }
        }else{
            stackSlot.stackItem(quantity);
            return true;
        }
        /*sInventorySlot stackSlot = GetStackSlot(newItem);
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
        }*/
    }

    public void changeOrder(int order){
        Debug.Log("changing order");
        canvas.sortingOrder = order;
    }

    public void sendItem(ItemData itemData, int quantity){
        AddItem(itemData, quantity);
    }
 
    private chestSlot GetStackSlot(ItemData newItem){
        foreach (chestSlot slot in slots){
            if(!slot.IsEmpty() && slot.isStackable(newItem)){
                return slot;
            }
        }
        return null;
    }

}
