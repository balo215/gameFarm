using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public new string itemName = "New Item";
    public string description = "Item description";
    public Sprite icon = null;
    public int sellPrice = 10;
    public GameObject itemPrefab;
    public ItemType itemType;
    public bool stackable = false;
    public bool DOU = false;

    public enum ItemType{
    	hoe,
    	seed,
    	food
    }
}