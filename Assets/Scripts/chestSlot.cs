using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class chestSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool dragged = false;
    public int quantity;
    public int indexSlot;
    public Text quantityText;

    public Image itemIcon;
    public ItemData currentItem;
    private CanvasGroup canvasGroup;
    public Transform parentBeforeDrag;

    // Start is called before the first frame update
    void Start()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;

        //quantityText.text = "";
        canvasGroup = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData){
        if (Input.GetKey(KeyCode.LeftControl) && currentItem != null){
            Debug.Log("dragging with ctrl: " + eventData.pointerDrag + " - " + eventData.pointerDrag.GetComponent<chestSlot>().quantity);
        }
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
        Debug.Log(this.IsEmpty());
        if(eventData.pointerDrag != null && this.IsEmpty() == true && eventData.pointerDrag.gameObject.CompareTag("chestSlot")){
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
                this.currentItem = eventData.pointerDrag.GetComponent<InventorySlot>().currentItem;
                this.quantity = eventData.pointerDrag.GetComponent<InventorySlot>().quantity;
                this.quantityText.text = quantity.ToString();

                newItemImage.enabled = false;
                Debug.Log(eventData.pointerDrag);

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

        slotToEmpty.currentItem = null;
        slotToEmpty.Item = null;
        slotToEmpty.quantityText.text = "";
        slotToEmpty.quantity = 0;
    }

    private void ClearChestInventoryAfterDrag(GameObject slot){
        chestSlot slotToEmpty = slot.GetComponent<chestSlot>();
        Transform slotTransform = slot.GetComponent<Transform>();
        slotToEmpty.itemIcon.sprite = null;
        
        //slotToEmpty.toChestBtn.gameObject.SetActive(false);
        slotToEmpty.currentItem = null;
        //slotToEmpty.Item = null;
        slotToEmpty.quantityText.text = "";
        slotToEmpty.quantity = 0;
    }

}

