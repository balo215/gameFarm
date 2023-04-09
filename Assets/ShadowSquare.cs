using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowSquare : MonoBehaviour
{
    [SerializeField] private Image shadowImage;
    [SerializeField] private float scrollSpeed = 10f;
    [SerializeField] private RectTransform shadowRect;
    
    private void Start()
    {
        shadowImage.enabled = false;
        //shadowRect = shadowObject.GetComponent<RectTransform>();
    }
    
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            shadowImage.enabled = true;
            
            Vector2 pos = shadowRect.anchoredPosition;
            pos.y += scroll * scrollSpeed;
            shadowRect.anchoredPosition = pos;
        }
        else
        {
            shadowImage.enabled = false;
        }
    }
}
