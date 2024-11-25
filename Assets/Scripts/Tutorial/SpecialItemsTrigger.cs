using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemsTrigger : MonoBehaviour
{
  public LayerMask Player;
  public GameObject RockSprite;
  public BoxCollider2D Rock;
  public ItemController itemController;
  // Start is called before the first frame update
  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Rock.IsTouchingLayers(Player) && itemController.SpecialItem != SpecialItems.None)
    {
      if (InputHandling.CheckUseItem())
      {
        RockSprite.SetActive(false);
      }
    }
  }
}
