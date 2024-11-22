using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikiEvent : NPCController
{
  private bool _canTalk;
  private bool _isTalking;

  void Update()
  {
    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) && _canTalk)
    {
      FindObjectOfType<PlayerController>().SetSpecialItem(SpecialItems.Dynamite);
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }

}
