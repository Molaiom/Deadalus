using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    int id { get; }

    TypeItem type { get; }

    string nameItem { get; }

    Sprite image { get; }

    GameObject model { get; }

    Transform position { get; }

    void OnPickup();
}

public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs (IInventoryItem item, int index = -1)
    {
        Item = item;
        Index = index;
    }

    public IInventoryItem Item;

    public int Index;
    
}

public class InventoryItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum TypeItem { Key, Tape, Lantern, Zippo }
