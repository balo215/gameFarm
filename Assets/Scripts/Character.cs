using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float moveSpeed = 5f;
    public GameObject itemPrefab;
    public GameObject inventoryPanel;
    public SmallInventory smallInventory;
    public Transform characterTransform;

    private bool isInventoryOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnItem();
        characterTransform = transform;
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
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
    	if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")){
        	Debug.Log("collision");
    	}
        item item = collision.gameObject.GetComponent<item>();
        if (item != null){
            bool added = smallInventory.AddItem(item.GetItemData(), item);
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

