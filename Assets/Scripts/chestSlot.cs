using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class chestSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private chestSlot temporarySlot;

    public bool dragged = false;
    public bool singleItem = false;
    public int quantity;
    public int indexSlot;
    public Text quantityText;

    public Image itemIcon;
    public ItemData currentItem;
    private CanvasGroup canvasGroup;
    public Transform parentBeforeDrag;
    public GameObject oneToInvBtn;
    public GameObject manyToInvBtn;
    private InventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        //quantityText.text = "";
        canvasGroup = GetComponent<CanvasGroup>();
        oneToInvBtn.SetActive(false);
        manyToInvBtn.SetActive(false);
        oneToInvBtn.GetComponent<Button>().onClick.AddListener(oneToInvButtonClick);
        manyToInvBtn.GetComponent<Button>().onClick.AddListener(manyToInvButtonClick);
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnBeginDrag(PointerEventData eventData){
        if(currentItem != null){
            eventData.pointerDrag.GetComponent<Transform>().parent.parent.GetComponent<Canvas>().sortingOrder = 1;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .5f;
            dragged = true;
            parentBeforeDrag = eventData.pointerDrag.GetComponent<Transform>().parent;
            indexSlot = eventData.pointerDrag.GetComponent<Transform>().GetSiblingIndex();
            eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<Transform>().parent.parent);
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
            Image newItemImage = eventData.pointerDrag.GetComponent<chestSlot>().itemIcon;
            newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            dragged = false;
        }
    }

    public void OnDrop(PointerEventData eventData){

        eventData.pointerDrag.GetComponent<Transform>().parent.GetComponent<Canvas>().sortingOrder = 0;
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
        //Debug.Log(eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem);
        
        if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("chestSlot")){
            if(eventData.pointerDrag.GetComponent<chestSlot>().currentItem != null){
                eventData.pointerDrag.GetComponent<chestSlot>().dragged = false;
                Sprite itemImage = eventData.pointerDrag.GetComponent<chestSlot>().itemIcon.sprite;
                Image newItemImage = eventData.pointerDrag.GetComponent<chestSlot>().itemIcon;
                newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                this.itemIcon.enabled = true;
                this.itemIcon.sprite = itemImage;
                //this.dropButton.gameObject.SetActive(true);
                this.oneToInvBtn.SetActive(true);
                this.manyToInvBtn.SetActive(true);
                this.currentItem = eventData.pointerDrag.GetComponent<chestSlot>().currentItem;
                this.quantity = eventData.pointerDrag.GetComponent<chestSlot>().quantity;
                this.quantityText.text = quantity.ToString();

                newItemImage.enabled = false;
                eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<chestSlot>().parentBeforeDrag);
                eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<chestSlot>().indexSlot);
                ClearChestInventoryAfterDrag(eventData.pointerDrag);            
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
                this.oneToInvBtn.SetActive(true);
                this.manyToInvBtn.SetActive(true);
                this.currentItem = eventData.pointerDrag.GetComponent<sInventorySlot>().currentItem;
                this.quantity = eventData.pointerDrag.GetComponent<sInventorySlot>().quantity;
                this.quantityText.text = quantity.ToString();

                newItemImage.enabled = false;
                
                eventData.pointerDrag.GetComponent<Transform>().SetParent(eventData.pointerDrag.GetComponent<sInventorySlot>().parentBeforeDrag);
                eventData.pointerDrag.GetComponent<Transform>().SetSiblingIndex(eventData.pointerDrag.GetComponent<sInventorySlot>().indexSlot);
                ClearSmallInventoryAfterDrag(eventData.pointerDrag);

            }
        }else if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("InventorySlot") ){
            if(eventData.pointerDrag.GetComponent<InventorySlot>().currentItem != null){
                Debug.Log("from chestSlot");
                Debug.Log(this);
                Debug.Log(eventData.pointerDrag);
                Debug.Log("---end from chestSlot---");
                eventData.pointerDrag.GetComponent<InventorySlot>().dragged = false;
                Sprite itemImage = eventData.pointerDrag.GetComponent<InventorySlot>().itemIcon.sprite;
                Image newItemImage = eventData.pointerDrag.GetComponent<InventorySlot>().itemIcon;
                newItemImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                this.itemIcon.enabled = true;
                this.itemIcon.sprite = itemImage;
                //this.dropButton.gameObject.SetActive(true);
                this.oneToInvBtn.SetActive(true);
                this.manyToInvBtn.SetActive(true);
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

    public bool IsEmpty(){
        return currentItem == null;
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

    public void SetItem(ItemData itemToAdd, int quantityP){
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
        oneToInvBtn.SetActive(true);
        manyToInvBtn.SetActive(true);
        quantity = quantityP;
        quantityText.text = quantity.ToString();
    }

    public void stackItem(int quantityP){
        quantity = quantity + quantityP;
        quantityText.text = quantity.ToString();
    }

    public bool isStackable(ItemData itemToAdd){
        if(currentItem.stackable && itemToAdd.itemName == currentItem.itemName){ 
            return true;
        }
        return false;
    }

    public void ClearSlot(){
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        currentItem = null;
        //dropButton.gameObject.SetActive(false);
        quantityText.text = "";
        quantity = 0;
        oneToInvBtn.SetActive(false);
        manyToInvBtn.SetActive(false);
    }

    public void oneToInvButtonClick(){
        if(quantity == 1){
            inventoryManager.AddItemToInv(currentItem, 1);
            ClearSlot();
        }
        if(quantity > 1){
            inventoryManager.AddItemToInv(currentItem, 1);
            quantity--;
            quantityText.text = quantity.ToString();
        }
    }

    public void manyToInvButtonClick(){
        inventoryManager.AddItemToInv(currentItem, quantity);
        ClearSlot();
    }

}

