using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class chestSlot : MonoBehaviour, IDropHandler
{
    public bool dragged = false;
    public int quantity;
    public int indexSlot;
    //public Text quantityText;

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

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag);
        Debug.Log(this);
    }


}

