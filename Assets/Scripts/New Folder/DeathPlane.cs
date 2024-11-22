using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
  public BoxCollider2D HitBox;
  public LayerMask Player;
  private PlayerController playerController;
  // Update is called once per frame
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
  }
  void Update()
  {
    if (HitBox.IsTouchingLayers(Player))
    {
      playerController.KillPlayer();
    }
  }
}
