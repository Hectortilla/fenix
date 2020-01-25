using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    Item ItemOnHand;
    List<Item> items = new List<Item>();
    [SerializeField]
    GameObject panelContainer = null;
    [SerializeField]
    GameObject InventorySlot = null;
    [SerializeField]
    GameObject InventoryUI = null;
    [SerializeField]
    Text descriptionText = null;

    bool displaying = false;
    // Use this for initialization
    void Start () {
       InventoryUI.SetActive(false);
    }

    void Update () {
        IfScrollItem();
        IfToggleInventory();
        IfUseItemInventory();
    }


    public void EmptyInvcentory(){
        foreach (Item item in items) {
            GameObject.Destroy(item.gameObject);
        }
        items = new List<Item>();
        UpdateUI();
    }
    public void AddItem(Item item){
        items.Add(item);
        UpdateUI();
    }

    void IfToggleInventory(){
        if (Input.GetKeyDown(KeyCode.I)){
            ToggleInventory();
        }
    }
    void ToggleInventory(){
        if (displaying){
            InventoryUI.SetActive(false);
        } else {
            InventoryUI.SetActive(true);
        }
        displaying = !displaying;
    }

    void IfScrollItem()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectItem(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void SelectItem(float ScrollDirection) {
        if (items.Count > 0){
            int indexItem = 0;
            if (ItemOnHand != null) {
                ItemOnHand.gameObject.SetActive(false);

                int directionSwitchItem = 0;
                if (ScrollDirection > 0)
                {
                    directionSwitchItem = 1;
                }
                else if (ScrollDirection < 0)
                {
                    directionSwitchItem = -1;
                }
                indexItem += items.IndexOf(ItemOnHand) + directionSwitchItem;

                if (indexItem >= items.Count)
                {
                    indexItem = 0;
                } else if (indexItem < 0) {
                    indexItem = items.Count - 1;
                }
            }
            ItemOnHand = items[indexItem];
            ItemOnHand.Equip();
            descriptionText.text = ItemOnHand.description;
        }
    }


    void IfUseItemInventory()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            UseItemInventory();
        }
    }

    void UseItemInventory() {
        if (ItemOnHand)
        {
            ItemOnHand.GetComponent<Item>().Use();
        }
        else {
            Debug.Log("No item equiped");
        }
    }


    void IfDropItemOnHand()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (ItemOnHand)
            {
                DropItem(ItemOnHand);
                ItemOnHand = null;
            }
            else {
                Debug.Log("No item equiped");
            }
        }
    }

    public void DropItem(Item item)
    {
        item.Drop();
        RemoveItem(item);
    }

    public void RemoveItem(Item item) {
        if (ItemOnHand == item)
        {
            ItemOnHand = null;
            descriptionText.text = "";
        }
        items.Remove(item);
        UpdateUI();
    }

    void UpdateUI () {

        foreach (Transform child in panelContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Item item in items) {
            GameObject inventorySlot = Instantiate(InventorySlot);
            foreach (Image image in inventorySlot.GetComponentsInChildren<Image>()){
                if (image.transform.parent != inventorySlot.transform){
                    image.sprite = item.icon;
                }
            }
            inventorySlot.transform.SetParent(panelContainer.transform);
        }
    }
}
