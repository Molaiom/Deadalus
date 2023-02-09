using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{

    public Inventory inventory;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
        inventory.ItemDeleted += InventoryScript_ItemDeleted;
        inventory.ItemChanged += InventoryScript_ItemChanged;
        inventory.InventoryVisibility += InventoryScript_InventoryVisibility;
        inventory.InspectItem += InventoryScript_InspectItem;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");

        foreach (Transform slot in inventoryPanel)
        {
            Image image = slot.GetChild(1).GetComponent<Image>();

            if (!image.enabled)
            {
                slot.GetComponent<Image>().color = Color.white;

                image.enabled = true;
                image.sprite = e.Item.image;

                break;
            }
        }
    }

    private void InventoryScript_ItemDeleted(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");


        int slotIndex = 0;

        foreach (Transform slot in inventoryPanel)
        {
            inventoryPanel.GetChild(inventory.index);

            if (slotIndex == e.Index)
            {
                Image image = slot.GetChild(1).GetComponent<Image>();
                image.enabled = false;
                image.sprite = null;

                slot.GetComponent<Image>().color = new Color(.325f, .325f, .325f, 1f);

                inventory.index = 0;
            }
            slotIndex++;
        }

        Transform slotManager = inventoryPanel.GetChild(0);
        slotManager.GetComponent<Image>().color = Color.red;
    }

    private void InventoryScript_ItemChanged(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");

        Transform slot = inventoryPanel.GetChild(inventory.index);

        slot.GetComponent<Image>().color = Color.white;

        inventory.index++;

        if (inventory.index == inventory.mItems.Count)
        {
            inventory.index = 0;
        }

        slot = inventoryPanel.GetChild(inventory.index);

        slot.GetComponent<Image>().color = Color.red;
    }

    private void InventoryScript_InventoryVisibility(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventoryPanel");

        inventoryPanel.gameObject.SetActive(inventory.isVisible);
    }

    private void InventoryScript_InspectItem(object sender, InventoryEventArgs e)
    {
        if (inventory.isInspecting)
        {
            GameObject item = (GameObject)Instantiate(e.Item.model, e.Item.position.transform.position + (e.Item.position.transform.forward * 1.5f), e.Item.position.transform.rotation);

            item.layer = LayerMask.NameToLayer("Inspect");
            //item.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            item.transform.localScale = new Vector3(.5f, .5f, .5f);

            player.GetComponent<PlayerController>().playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
            player.GetComponent<PlayerController>().otherCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);

            player.GetComponent<PlayerController>().isInspecting = true;
            player.GetComponent<PlayerController>().inspectedItem = item;

        }
        else
        {
            player.GetComponent<PlayerController>().isInspecting = false;
            Destroy(player.GetComponent<PlayerController>().inspectedItem);
            player.GetComponent<PlayerController>().inspectedItem = null;
        }
    }
}
