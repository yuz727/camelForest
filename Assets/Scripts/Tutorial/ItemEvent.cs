using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvent : MonoBehaviour
{
  public GameObject ThisItem;
  public BoxCollider2D Item;
  public LayerMask player;
  void FixedUpdate()
  {
    if (Item.IsTouchingLayers(player))
    {
      FindObjectOfType<PlayerController>().AddItem(Items.Bow);
      ThisItem.SetActive(false);
    }
  }
}
