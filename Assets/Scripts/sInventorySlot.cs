using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class sInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image itemIcon;
    public item Item;
    public ItemData currentItem;
    public Button dropButton;
    public Button useButton;
    public Text quantityText;
    public bool usable;
    public int quantity;
    private int indexSlot;
    private bool dragged = false;

	public Character character;
	public GameObject pausedBackground;

	public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;
    public GameObject duplicateItem;
    private CanvasGroup canvasGroup;


    private Transform parentAfterDrag;
   
    private void Start(){
    	dropButton.onClick.AddListener(dropButtonClick);
    	useButton.onClick.AddListener(useButtonClick);
    	itemIcon.sprite = null;
    	itemIcon.enabled = false;
    	dropButton.gameObject.SetActive(false);
    	quantityText.text = "";
    	usable = true;
    	canvasGroup = GetComponent<CanvasGroup>();

    }
	
    public void OnBeginDrag(PointerEventData eventData){
    	if(currentItem != null){
	    	//Debug.Log("---");
	    	//item itemBeingDragged = gameObject.GetComponentInChildren<item>();
	    	canvasGroup.blocksRaycasts = false;
	    	canvasGroup.alpha = .5f;
	        startPosition = itemIcon.transform.position;
	        startParent = transform.parent;
	        dragStartPosition = eventData.position;
    		dragged = true;
    	
	    	parentAfterDrag = eventData.pointerDrag.GetComponent<Transform>().parent;
	    	indexSlot = eventData.pointerDrag.GetComponent<Transform>().GetSiblingIndex();
	    	//Debug.Log(parentAfterDrag);
	    	eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<Transform>().parent.parent.parent);
	    	eventData.pointerDrag.GetComponent<Transform>().SetAsLastSibling();
    	}
	     //   parentAfterDrag = transform.parent;
	       // transform.SetParent(transform.root);
	        //transform.SetAsLastSibling();
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
    	if(currentItem != null){
    		//Debug.Log(currentItem);
    		Debug.Log("draging");
    		itemIcon.transform.position = eventData.position;
    		//Image newItemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon;
    		//newItemImage.enabled = true;
    	}

    	//transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
    	Debug.Log("end dragging");
    	dragEndPosition = eventData.position;
    	canvasGroup.blocksRaycasts = true;
    	canvasGroup.alpha = 1;
    	if(dragged == true){
    		Debug.Log(eventData.pointerDrag);
    		eventData.pointerDrag.GetComponent<Transform>().SetParent(parentAfterDrag);
    		eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(indexSlot);
    		Image newItemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon;
        	newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
    		dragged = false;
    	}
    	
        // Do something when the dragging ends
    }

    public void OnDrop(PointerEventData eventData) {
        // Check if there is no item on this slot
        //eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon.sprite = null;
        //Debug.Log(eventData.pointerDrag.gameObject);
        //Debug.Log(this);
        Debug.Log("OnDrop");
        //Debug.Log(this.IsEmpty());
        //"this" is the slot getting the item
        if(eventData.pointerDrag != null && this.IsEmpty() == true){
        	eventData.pointerDrag.GetComponent<sInventorySlot>().dragged = false;
        	//eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        	Sprite itemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon.sprite;
        	Image newItemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon;
        	newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        	this.itemIcon.enabled = true;
        	this.itemIcon.sprite = itemImage;
        	this.dropButton.gameObject.SetActive(true);
        	this.currentItem = eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem;
        	this.quantity = eventData.pointerDrag.GetComponent<sInventorySlot>().quantity;
        	this.quantityText.text = quantity.ToString();

        	newItemImage.enabled = false;
        	eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon.sprite = null;
        	eventData.pointerDrag.GetComponent<sInventorySlot>().dropButton.gameObject.SetActive(false);
        	//Debug.Log(this.currentItem);
        	//Debug.Log("----");
        	//Debug.Log(this.GetComponent<Transform>().parent);
        	eventData.pointerDrag.GetComponent<Transform>().SetParent(this.GetComponent<Transform>().parent);
        	eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<sInventorySlot>().indexSlot);
        	//Debug.Log(eventData.pointerDrag.GetComponent<sInventorySlot>().indexSlot);
        	eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem = null;
        	eventData.pointerDrag.GetComponent<sInventorySlot>().Item = null;
        	eventData.pointerDrag.GetComponent<sInventorySlot>().quantityText.text = "";
        	eventData.pointerDrag.GetComponent<sInventorySlot>().quantity = 0;
		    StartCoroutine(OnDropCoroutine()); // Start coroutine with delay
        	//Debug.Log(this.GetComponent<RectTransform>().anchoredPosition);
        	//Debug.Log("OnDrop");
        	//Debug.Log(this.currentItem);
        	//Debug.Log("----");
        	//Debug.Log(currentItem);
        	//Debug.Log(eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem);
        	//!!! I need to create an AddFromDrop function
        }else{
        	Debug.Log(eventData.pointerDrag);
        	//eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
        }
        /*if (Item == null) {
            // Get the dragged item from the event data
            Item draggedItem = eventData.pointerDrag.GetComponent<Item>();

            // Check if the dragged item is not null and remove it from its original slot
            if (draggedItem != null) {
                InventorySlot originalSlot = draggedItem.GetComponentInParent<InventorySlot>();
                if (originalSlot != null) {
                    originalSlot.item = null;
                }

                // Set the dragged item to this slot's item and set its parent to this slot
                item = draggedItem;
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
            }
        }*/
    }



    public void ClearSlot(){
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        currentItem = null;
        dropButton.gameObject.SetActive(false);
        Item = null;
    	quantityText.text = "";
    	quantity = 0;
    }

    public void SetItem(ItemData itemToAdd, item fullItem){
    	if (itemToAdd == null){
        	itemIcon.sprite = null;
        	itemIcon.enabled = false;
        	currentItem = null;
			dropButton.gameObject.SetActive(false);
        	return;
    	}
    	itemIcon.sprite = itemToAdd.icon;
    	itemIcon.enabled = true;
    	currentItem = itemToAdd;
    	dropButton.gameObject.SetActive(true);
    	quantity = fullItem.quantity;
    	quantityText.text = quantity.ToString();
	}

	public void stackItem(item fullItem){
		quantity = fullItem.quantity + quantity;
		quantityText.text = quantity.ToString();
	}

	public bool IsEmpty(){
		return currentItem == null;
	}
	public bool isStackable(ItemData itemToAdd){
		if(currentItem.stackable && itemToAdd.itemName == currentItem.itemName){ 
			return true;
		}
		return false;
	}
	private void dropButtonClick(){
		int randomValue1 = Random.Range(-3, 4);
		int randomValue2 = Random.Range(-3, 4);
        Vector2 spawnPosition = character.transform.position + new Vector3(randomValue1, randomValue2); // Add a 5 unit offset to the character's position
        GameObject newItemObject = Instantiate(currentItem.itemPrefab, spawnPosition, Quaternion.identity);
        item newItem = newItemObject.GetComponent<item>();
        newItem.quantity = quantity;
        newItem.SetItemData(currentItem);
		ClearSlot();
		quantityText.text = "";
	}

	public void useButtonClick(){
		Debug.Log(currentItem);
		//float distance = Vector2.Distance(dragStartPosition, dragEndPosition);
		if(!pausedBackground.activeSelf ){
			if(currentItem != null){
				//Debug.Log(currentItem);
				ItemBehaviorManager.Use(currentItem);
				if(currentItem.DOU){
					if(quantity == 1){
						ClearSlot();
					}else{
						quantity--;
						quantityText.text = quantity.ToString();
					}
				}
			}
		}
	}

	private IEnumerator OnDropCoroutine(){
    	yield return new WaitForSeconds(1f); // Add a delay of 0.5 seconds (adjust as needed)
	}
}
