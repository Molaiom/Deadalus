using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private const int SLOTS = 8;

    public int index = 0;
    public bool isVisible = false;
    public bool isInspecting = false;

    public List<IInventoryItem> mItems = new List<IInventoryItem>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public event EventHandler<InventoryEventArgs> ItemDeleted;

    public event EventHandler<InventoryEventArgs> ItemChanged;

    public event EventHandler<InventoryEventArgs> InventoryVisibility;

    public event EventHandler<InventoryEventArgs> InspectItem;

    public void AddItem(IInventoryItem item)
    {
        if (mItems.Count < SLOTS)
        {

            foreach (IInventoryItem itemArray in mItems)
            {
                if (itemArray.nameItem.Equals(item.nameItem))
                {
                    return;
                }
            }

            mItems.Add(item);

            item.OnPickup();

            if (ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
        }
    }

    public void RemoveItem(String itemName)
    {
        int index = 0;
        IInventoryItem item = null;
        
        for (int i = 0; i < mItems.Count; i++)
        {
            index = i;

            if (mItems[i].nameItem.Equals(itemName))
            {
                item = mItems[i];
                mItems.Remove(mItems[i]);

                i = mItems.Count + 1;
            }
        }

        if (ItemDeleted != null)
        {
            ItemDeleted(this, new InventoryEventArgs(item, index));
        }

    }

    public void nextIndex()
    {
        if (mItems.Count > index || mItems.Count == SLOTS)
            ItemChanged(this, new InventoryEventArgs(mItems[index]));
    }

    public void openOrCloseInventory(bool visibility)
    {
        isVisible = visibility;
        InventoryVisibility(this, new InventoryEventArgs(mItems[0]));
    }

    public void inspectItem(bool visibility)
    {
        isInspecting = visibility;
        InspectItem(this, new InventoryEventArgs(mItems[index]));
    }

    public void UseItem(PlayerController player)
    {
        AudioSource audioSource = player.gameObject.GetComponent<AudioSource>();
        if (mItems[index].nameItem.Equals("Lantern"))
        {
            player.lanternActive = true;
            player.zippoActive = false;

        }
        else if (mItems[index].nameItem.Equals("Zippo"))
        {
            audioSource.PlayOneShot(Sons.instance.IsqueiroAcender, audioSource.volume);
            player.lanternActive = false;
            player.zippoActive = true;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
