using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEvent : MonoBehaviour
{
  public static GameObject S_Key;
  public static GameObject S_Mushroom;
  public static GameObject S_Bow;
  public static ItemController S_ItemController;
  public GameObject ThisItem;
  public BoxCollider2D Item;
  public LayerMask player;

  void Start()
  {
    S_ItemController = FindFirstObjectByType<ItemController>();
    S_Key = GameObject.Find("Key");
    S_Mushroom = GameObject.Find("Mushroom");
    S_Bow = GameObject.Find("Bow");
  }

  void FixedUpdate()
  {
    if (Item.IsTouchingLayers(player))
    {
      switch (ThisItem)
      {
        case var ThisItem when ThisItem == S_Key:
          S_ItemController.AddItem(Items.Key);
          ThisItem.SetActive(false);
          break;
        case var ThisItem when ThisItem == S_Mushroom:
          if (FindObjectOfType<ShroomEvent>().StartRepeat)
          {
            S_ItemController.AddItem(Items.Mushroom);
            ThisItem.SetActive(false);
          }
          break;
        case var ThisItem when ThisItem == S_Bow:
          S_ItemController.AddItem(Items.Bow);
          ThisItem.SetActive(false);
          break;
        default:
          break;
      } // Switch
    } // if in contact
  }

  void Respawn()
  {
    ThisItem.SetActive(true);
  }

}
