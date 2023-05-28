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
    //public Button toChest;
    public Text quantityText;
    public bool usable;
    public int quantity;
    public int indexSlot;
    public bool dragged = false;

	public Character character;
	public GameObject pausedBackground;
    public GameObject toChestBtn;


    public static GameObject itemBeingDragged;
    public GameObject duplicateItem;
    private CanvasGroup canvasGroup;


    public Transform parentBeforeDrag;
   
    private void Start(){
    	dropButton.onClick.AddListener(dropButtonClick);
    	toChestBtn.GetComponent<Button>().onClick.AddListener(toChestButtonClick);
    	itemIcon.sprite = null;
    	itemIcon.enabled = false;
    	dropButton.gameObject.SetActive(false);
    	quantityText.text = "";
    	usable = true;
    	canvasGroup = GetComponent<CanvasGroup>();
    	toChestBtn.SetActive(false);

    }
	
    public void OnBeginDrag(PointerEventData eventData){
    	if(currentItem != null){
	    	canvasGroup.blocksRaycasts = false;
	    	canvasGroup.alpha = .5f;
	        
    		dragged = true;
    	
	    	parentBeforeDrag = eventData.pointerDrag.GetComponent<Transform>().parent;
	    	indexSlot = eventData.pointerDrag.GetComponent<Transform>().GetSiblingIndex();
	    	eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<Transform>().parent.parent.parent);
	    	eventData.pointerDrag.GetComponent<Transform>().SetAsLastSibling();
    	}

    }

    public void OnDrag(PointerEventData eventData){
    	if(currentItem != null){
    		itemIcon.transform.position = eventData.position;
    	}
    }

    public void OnEndDrag(PointerEventData eventData){
    	canvasGroup.blocksRaycasts = true;
    	canvasGroup.alpha = 1;


    	if(dragged == true){
    		eventData.pointerDrag.GetComponent<Transform>().SetParent(parentBeforeDrag);
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
        //Debug.Log(eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem);
        //Debug.Log(this.IsEmpty());
        //"this" is the slot getting the item

		//this condition prevents the issue of dragging empty slots, giving "white square items" in the inventory
		//eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem != null
        if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("SmallInventorySlot")){
        	if(eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem != null){
	        	Debug.Log("from smallInvSlot");
	        	Debug.Log(this);
	        	Debug.Log(eventData.pointerDrag);
	        	Debug.Log("---end from smallInvSlot---");
	        	eventData.pointerDrag.GetComponent<sInventorySlot>().dragged = false;
	        	//eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
	        	Sprite itemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon.sprite;
	        	Image newItemImage = eventData.pointerDrag.GetComponent<sInventorySlot>().itemIcon;
	        	newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
	        	this.itemIcon.enabled = true;
	        	this.itemIcon.sprite = itemImage;
	        	//this.dropButton.gameObject.SetActive(true);
	        	this.currentItem = eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem;
	        	this.quantity = eventData.pointerDrag.GetComponent<sInventorySlot>().quantity;
	        	this.quantityText.text = quantity.ToString();

	        	newItemImage.enabled = false;
	        	
	        	eventData.pointerDrag.GetComponent<Transform>().SetParent(this.GetComponent<Transform>().parent);
	        	eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<sInventorySlot>().indexSlot);
	        	ClearSmallInventoryAfterDrag(eventData.pointerDrag);
	        	
        	}
        }else if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("InventorySlot")){
        	if(eventData.pointerDrag.GetComponent<InventorySlot>().currentItem != null){
	        	Debug.Log("from smallInvSlot");
	        	Debug.Log(this);
	        	Debug.Log(eventData.pointerDrag);
	        	Debug.Log("---end from smallInvSlot---");
	        	eventData.pointerDrag.GetComponent<InventorySlot>().dragged = false;
	        	//eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
	        	Sprite itemImage = eventData.pointerDrag.GetComponent<InventorySlot>().itemIcon.sprite;
	        	Image newItemImage = eventData.pointerDrag.GetComponent<InventorySlot>().itemIcon;
	        	newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
	        	this.itemIcon.enabled = true;
	        	this.itemIcon.sprite = itemImage;
	        	//this.dropButton.gameObject.SetActive(true);
	        	this.currentItem = eventData.pointerDrag.GetComponent<InventorySlot>().currentItem;
	        	this.quantity = eventData.pointerDrag.GetComponent<InventorySlot>().quantity;
	        	this.quantityText.text = quantity.ToString();

	        	newItemImage.enabled = false;
	        	
	        	eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<InventorySlot>().parentBeforeDrag);
	        	eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<InventorySlot>().indexSlot);
	        	ClearInventoryAfterDrag(eventData.pointerDrag);
	        
        	}
        }
    }

    private void ClearSmallInventoryAfterDrag(GameObject slot){
    	sInventorySlot slotToEmpty = slot.GetComponent<sInventorySlot>();
    	Transform slotTransform = slot.GetComponent<Transform>();
    	slotToEmpty.itemIcon.sprite = null;
    	//slotToEmpty.dropButton.gameObject.SetActive(false);
    	slotToEmpty.currentItem = null;
    	slotToEmpty.Item = null;
    	slotToEmpty.quantityText.text = "";
    	slotToEmpty.quantity = 0;
    }

    private void ClearInventoryAfterDrag(GameObject slot){
    	InventorySlot slotToEmpty = slot.GetComponent<InventorySlot>();
    	Transform slotTransform = slot.GetComponent<Transform>();
    	slotToEmpty.itemIcon.sprite = null;
    	//slotToEmpty.dropButton.gameObject.SetActive(false);
    	slotToEmpty.currentItem = null;
    	slotToEmpty.Item = null;
    	slotToEmpty.quantityText.text = "";
    	slotToEmpty.quantity = 0;
    }

    public void ClearSlot(){
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        currentItem = null;
        //dropButton.gameObject.SetActive(false);
        Item = null;
    	quantityText.text = "";
    	quantity = 0;
    }

    public void SetItem(ItemData itemToAdd, item fullItem){
    	if (itemToAdd == null){
        	itemIcon.sprite = null;
        	itemIcon.enabled = false;
        	currentItem = null;
			//dropButton.gameObject.SetActive(false);
        	return;
    	}
    	itemIcon.sprite = itemToAdd.icon;
    	itemIcon.enabled = true;
    	currentItem = itemToAdd;
    	//dropButton.gameObject.SetActive(true);
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
        dropButton.gameObject.SetActive(false);    

	}

    /*
	public void useButtonClick(){
        //Debug.Log(pausedBackground.activeSelf);
        if (!pausedBackground.activeSelf ){
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
    */
    public void useButtonKey()
    {
        //Debug.Log(pausedBackground.activeSelf);
        if (!pausedBackground.activeSelf)
        {
            if (currentItem != null)
            {
                //Debug.Log(currentItem);
                ItemBehaviorManager.Use(currentItem);
                if (currentItem.DOU)
                {
                    if (quantity == 1)
                    {
                        ClearSlot();
                    }
                    else
                    {
                        quantity--;
                        quantityText.text = quantity.ToString();
                    }
                }
            }
        }
    }

    public void showChestBtn(){
    	if(currentItem != null){
    		toChestBtn.SetActive(true);
    	}
    }

    public void hiddeChestBtn(){
    	if(currentItem != null){
    		toChestBtn.SetActive(false);
    	}
    }

	private void toChestButtonClick(){
		Debug.Log(currentItem);
	}

    public void showDropBtn(){
        if(currentItem != null){
            dropButton.gameObject.SetActive(true);
        }
    }

    public void hideDropBtn(){
        dropButton.gameObject.SetActive(false);    
    }
    /*
        public void DropIntoSlot(ItemData itemToAdd, int quantityOrigin){
            //Debug.Log(newItem.itemData);
            if (itemToAdd == null){
                itemIcon.sprite = null;
                itemIcon.enabled = false;
                currentItem = null;
                //dropButton.gameObject.SetActive(false);
                return;
            }
            itemIcon.sprite = itemToAdd.icon;
            itemIcon.enabled = true;
            currentItem = itemToAdd;
            //dropButton.gameObject.SetActive(true);
            quantity = quantityOrigin;
            quantityText.text = quantity.ToString();
        }*/

   

    private IEnumerator OnDropCoroutine(){
    	yield return new WaitForSeconds(1f); // Add a delay of 0.5 seconds (adjust as needed)
	}
}
