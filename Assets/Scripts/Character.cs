﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float moveSpeed = 5f;
    public GameObject itemPrefab;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    public Transform characterTransform;
    private int layerMask;
    private Vector3 raycastDirection;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnItem();
        characterTransform = transform;
        int characterLayer = LayerMask.NameToLayer("character"); // Replace "Character" with the name of your character's layer
        layerMask = ~(1 << characterLayer);
        inventoryManager.isChestOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput =  Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 lastFacingDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(lastFacingDirection != Vector3.zero){
            raycastDirection = new Vector3(lastFacingDirection.x, lastFacingDirection.y, 0); // Convert last facing direction to a 3D vector
        }
        //Debug.Log(raycastDirection);

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;
        if(Input.GetKeyDown("i") && inventoryManager.isChestOpen == false){
            inventoryManager.isInventoryOpen = !inventoryManager.isInventoryOpen;
            inventoryPanel.SetActive(inventoryManager.isInventoryOpen);
            Time.timeScale = inventoryManager.isInventoryOpen ? 0f : 1f;
        }
        /*if (Input.GetKeyDown("e"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, 2f, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Chest"))
                {
                    
                    if (isInventoryOpen == true && isChestOpen == true)
                    {
                        isChestOpen = false;
                        Vector2 newPosition = new Vector2(inventoryPanel.transform.position.x + 400f, inventoryPanel.transform.position.y);
                        inventoryPanel.transform.position = newPosition;
                        isInventoryOpen = false;

                    }
                    else
                    {
                        isChestOpen = true;
                        Vector2 newPosition = new Vector2(inventoryPanel.transform.position.x - 400f, inventoryPanel.transform.position.y);
                        inventoryPanel.transform.position = newPosition;
                        isInventoryOpen = true;
                    }
                    //isInventoryOpen = !isInventoryOpen;
                    inventoryPanel.SetActive(isInventoryOpen);
                    Time.timeScale = isInventoryOpen ? 0f : 1f;
                }
            }
        }*/
        Debug.DrawRay(transform.position, raycastDirection * 2f, Color.red);



        
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
    	if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
        	Debug.Log("collision");
    	}
        item item = collision.gameObject.GetComponent<item>();
        if (item != null){
            bool added = inventoryManager.AddItemToInv(item.GetItemData(), item.quantity);
            if(added == true){
                Destroy(collision.gameObject);
            }
        }
	}

    void SpawnItem(){
        Vector2 spawnPosition = transform.position + new Vector3(5f, 0f); // Add a 5 unit offset to the character's position
        GameObject itemGO = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        item Item = itemGO.GetComponent<item>();
        //Debug.Log(Item);
        //item.SetItemData(itemData);
    }
    
}

