using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrigger : MonoBehaviour
{
  public PlayerController playerController;
  // Start is called before the first frame update
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    if (playerController.SpecialItem != SpecialItems.Crowbar)
    {
      this.gameObject.SetActive(false);
    }
  }

}
