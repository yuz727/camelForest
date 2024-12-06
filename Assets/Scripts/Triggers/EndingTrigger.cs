using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
  public BoxCollider2D HitBox;
  public LayerMask Player;
  public EndingScript endingScript;


  void Update()
  {
    if (HitBox.IsTouchingLayers(Player))
    {
      endingScript.EndingOne();
      this.gameObject.SetActive(false);
    }
  }
}
