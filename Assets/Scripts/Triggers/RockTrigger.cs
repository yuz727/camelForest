using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrigger : MonoBehaviour
{
  public ItemController itemController;
  // Start is called before the first frame update
  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
    if (itemController.SpecialItem != SpecialItems.Crowbar)
    {
      gameObject.SetActive(false);
    }
  }

}
