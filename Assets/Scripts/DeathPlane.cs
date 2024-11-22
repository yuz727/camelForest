using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
  public BoxCollider2D HitBox;
  public LayerMask Player;
  // Update is called once per frame
  void Update()
  {
    if (HitBox.IsTouchingLayers(Player))
    {
      FindObjectOfType<PlayerController>().KillPlayer();
    }
  }
}
