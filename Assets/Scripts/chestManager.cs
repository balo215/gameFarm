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


    // Start is called before the first frame update
    void Start()
    {
        int chestLayer = LayerMask.NameToLayer("Chest"); // Replace "Character" with the name of your character's layer
        layerMask = ~(1 << chestLayer);
        inventoryManager.isChestOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, layerMask);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider);
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("opening chest after collision");
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
}
