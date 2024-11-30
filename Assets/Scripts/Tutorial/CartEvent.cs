using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartEvent : NPCController
{
  public Animator Anim;
  private bool _canTalk;
  private bool _isTalking;
  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
  }

  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk)
    {
      itemController.cartState = (itemController.cartState == CartState.Slime) ? 0 : itemController.cartState + 1;
      switch (itemController.cartState)
      {
        case CartState.Slime:
          Anim.SetBool("isSlime", true);
          Anim.SetBool("isDuck", false);
          break;
        case CartState.Duck:
          Anim.SetBool("isSlime", false);
          Anim.SetBool("isDuck", true);
          break;
        default:
          Anim.SetBool("isSlime", false);
          Anim.SetBool("isDuck", false);
          break;
      }
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }

}

public enum CartState
{
  Basketball,
  Duck,
  Slime
}