using System.Collections;
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

    private bool isInventoryOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnItem();
        characterTransform = transform;
        int characterLayer = LayerMask.NameToLayer("character"); // Replace "Character" with the name of your character's layer
        layerMask = ~(1 << characterLayer); 
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput =  Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;
        if(Input.GetKeyDown("i")){
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            Time.timeScale = isInventoryOpen ? 0f : 1f;
        }
        if(Input.GetKeyDown("e")){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 2f, layerMask);
            if (hit.collider != null){

                Debug.Log(hit.collider);
                // Check if the raycast hit the chest object
                if (hit.collider.CompareTag("Chest"))
                {
                    // Trigger "open chest" function
                    Debug.Log("opening chest");
                }
            }
        }
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
    	if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
        	Debug.Log("collision");
    	}
        if(collision.gameObject.layer == LayerMask.NameToLayer("Chest")){
            Debug.Log("collision with a Chest");
        }
        item item = collision.gameObject.GetComponent<item>();
        if (item != null){
            bool added = inventoryManager.AddItemToInv(item.GetItemData(), item);
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

