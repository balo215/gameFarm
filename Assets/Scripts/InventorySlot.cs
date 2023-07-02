using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool dragged = false;
    public int quantity;
    public int indexSlot;
    public Text quantityText;

    public Image itemIcon;
    public item Item;
    public ItemData currentItem;
    private CanvasGroup canvasGroup;
    public Transform parentBeforeDrag;
    public GameObject toChestBtn;
    public GameObject manyToChestBtn;
    private InventoryManager inventoryManager;



    // Start is called before the first frame update
    void Start()
    {
        itemIcon.sprite = null;
    	itemIcon.enabled = false;
    	quantityText.text = "";
    	canvasGroup = GetComponent<CanvasGroup>();
        toChestBtn.SetActive(false);
        manyToChestBtn.SetActive(false);
        toChestBtn.GetComponent<Button>().onClick.AddListener(toChestButtonClick);
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData){
    	if(currentItem != null){
	    	canvasGroup.blocksRaycasts = false;
	    	canvasGroup.alpha = .5f;
    		dragged = true;
            eventData.pointerDrag.GetComponent<Transform>().parent.parent.parent.GetComponent<Canvas>().sortingOrder = 1;

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
    		Image newItemImage = eventData.pointerDrag.GetComponent<InventorySlot>().itemIcon;
        	newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
    		dragged = false;
    	}
    }

    public void OnDrop(PointerEventData eventData) {
        eventData.pointerDrag.GetComponent<Transform>().parent.GetComponent<Canvas>().sortingOrder = 0;
    	//the next 2 if's are for drop a single item from the small inventory to the inventory and within the inventory, to a stack of stackable items
    	//this is NOT implemented on small inventory (from within and from inventory)
    	//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if(eventData.pointerDrag != null && this.IsEmpty() == false && eventData.pointerDrag.gameObject.CompareTag("InventorySlot") ){
        	InventorySlot invSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        	if(invSlot.currentItem != null && invSlot.currentItem.itemName == this.currentItem.itemName && this.currentItem.stackable == true){
        		//invSlot is the one being dragged
	        	this.quantity = this.quantity + invSlot.quantity;
	        	this.quantityText.text = this.quantity.ToString();
	        	ClearInventoryAfterDrag(eventData.pointerDrag);
	        	invSlot.itemIcon.enabled = false;

        	}
		}
		if(eventData.pointerDrag != null && this.IsEmpty() == false && eventData.pointerDrag.gameObject.CompareTag("SmallInventorySlot") ){
        	sInventorySlot invSlot = eventData.pointerDrag.GetComponent<sInventorySlot>();
        	if(invSlot.currentItem != null && invSlot.currentItem.itemName == this.currentItem.itemName && this.currentItem.stackable == true){        		
	        	this.quantity = this.quantity + invSlot.quantity;
	        	this.quantityText.text = this.quantity.ToString();
	        	ClearSmallInventoryAfterDrag(eventData.pointerDrag);
	        	invSlot.itemIcon.enabled = false;

        	}
		}
        if(eventData.pointerDrag != null && this.IsEmpty() == false && eventData.pointerDrag.gameObject.CompareTag("chestSlot") ){
            chestSlot invSlot = eventData.pointerDrag.GetComponent<chestSlot>();
            if(invSlot.currentItem != null && invSlot.currentItem.itemName == this.currentItem.itemName && this.currentItem.stackable == true){             
                this.quantity = this.quantity + invSlot.quantity;
                this.quantityText.text = this.quantity.ToString();
                ClearChestInventoryAfterDrag(eventData.pointerDrag);
                invSlot.itemIcon.enabled = false;

            }
        }
		//============================================= END OF 2 IFs =================================================== 


        if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("InventorySlot") ){
        	if(eventData.pointerDrag.GetComponent<InventorySlot>().currentItem != null){
	        	Debug.Log("from InvSlot");
	        	Debug.Log(this);
	        	Debug.Log(eventData.pointerDrag);
	        	Debug.Log("---end from InvSlot---");
	        	eventData.pointerDrag.GetComponent<InventorySlot>().dragged = false;
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
	        	
	        	eventData.pointerDrag.GetComponent<Transform>().SetParent(this.GetComponent<Transform>().parent);
	        	eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<InventorySlot>().indexSlot);
	        	ClearInventoryAfterDrag(eventData.pointerDrag);
        	}
        }else if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("SmallInventorySlot")){
        	if(eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem != null){
	        	eventData.pointerDrag.GetComponent<sInventorySlot>().dragged = false;
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
	        	
	        	eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<sInventorySlot>().parentBeforeDrag);
	        	eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<sInventorySlot>().indexSlot);
	        	ClearSmallInventoryAfterDrag(eventData.pointerDrag);

        	}
        }else if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("chestSlot")){
            if(eventData.pointerDrag.GetComponent<chestSlot>().currentItem != null){
                eventData.pointerDrag.GetComponent<chestSlot>().dragged = false;
                Sprite itemImage = eventData.pointerDrag.GetComponent<chestSlot>().itemIcon.sprite;
                Image newItemImage = eventData.pointerDrag.GetComponent<chestSlot>().itemIcon;
                newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                this.itemIcon.enabled = true;
                this.itemIcon.sprite = itemImage;
                //this.dropButton.gameObject.SetActive(true);

                this.currentItem = eventData.pointerDrag.GetComponent<chestSlot>().currentItem;
                this.quantity = eventData.pointerDrag.GetComponent<chestSlot>().quantity;
                this.quantityText.text = quantity.ToString();

                newItemImage.enabled = false;
                
                eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<chestSlot>().parentBeforeDrag);
                eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<chestSlot>().indexSlot);
                ClearChestInventoryAfterDrag(eventData.pointerDrag);            
            }
        }
        
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
    	itemIcon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    	//dropButton.gameObject.SetActive(true);
    	quantity = fullItem.quantity;
    	quantityText.text = quantity.ToString();
	}

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

	public void stackItem(item fullItem){
		quantity = fullItem.quantity + quantity;
		quantityText.text = quantity.ToString();
	}

	private void ClearInventoryAfterDrag(GameObject slot){
    	InventorySlot slotToEmpty = slot.GetComponent<InventorySlot>();
    	Transform slotTransform = slot.GetComponent<Transform>();
    	slotToEmpty.itemIcon.sprite = null;
    	//slotToEmpty.dropButton.gameObject.SetActive(false);
        slotToEmpty.toChestBtn.gameObject.SetActive(false);
        slotToEmpty.manyToChestBtn.gameObject.SetActive(false);
    	slotToEmpty.currentItem = null;
    	slotToEmpty.Item = null;
    	slotToEmpty.quantityText.text = "";
    	slotToEmpty.quantity = 0;
    }

    private void ClearSmallInventoryAfterDrag(GameObject slot){
    	sInventorySlot slotToEmpty = slot.GetComponent<sInventorySlot>();
    	Transform slotTransform = slot.GetComponent<Transform>();
    	slotToEmpty.itemIcon.sprite = null;
    	slotToEmpty.dropButton.gameObject.SetActive(false);
        slotToEmpty.toChestBtn.gameObject.SetActive(false);
        slotToEmpty.manyToChestBtn.gameObject.SetActive(false);
    	slotToEmpty.currentItem = null;
    	slotToEmpty.Item = null;
    	slotToEmpty.quantityText.text = "";
    	slotToEmpty.quantity = 0;
    }

    private void ClearChestInventoryAfterDrag(GameObject slot){
        chestSlot slotToEmpty = slot.GetComponent<chestSlot>();
        Transform slotTransform = slot.GetComponent<Transform>();
        slotToEmpty.itemIcon.sprite = null;
        slotToEmpty.oneToInvBtn.SetActive(false);
        slotToEmpty.manyToInvBtn.SetActive(false);
        //slotToEmpty.toChestBtn.gameObject.SetActive(false);
        slotToEmpty.currentItem = null;
        //slotToEmpty.Item = null;
        slotToEmpty.quantityText.text = "";
        slotToEmpty.quantity = 0;
    }

    public void showChestBtn(){
        if(currentItem != null){
            toChestBtn.SetActive(true);
            manyToChestBtn.SetActive(true);
        }
    }

    public void hiddeChestBtn(){
        //if(currentItem != null){
            toChestBtn.SetActive(false);
            manyToChestBtn.SetActive(false);
        //}
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

    private void toChestButtonClick(){
        if(quantity == 1){
            inventoryManager.sendItem(currentItem, 1);
            ClearSlot();
            hiddeChestBtn();
        }
        if(quantity >= 2){
            quantity--;
            quantityText.text = quantity.ToString();
            inventoryManager.sendItem(currentItem, 1);
        }
    }

}
