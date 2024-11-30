using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDespawnTrigger : MonoBehaviour
{

  void Start()
  {
    if (FindFirstObjectByType<ItemController>().SpecialItemUsed != SpecialItems.Dynamite)
    {
      this.gameObject.SetActive(false);
    }
  }

}
