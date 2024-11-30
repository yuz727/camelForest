using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDespawnTrigger : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    if (FindFirstObjectByType<ItemController>().cartState != CartState.Slime)
    {
      this.gameObject.SetActive(false);
    }
  }

}
