using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpikeTrigger : MonoBehaviour
{
  public PolygonCollider2D Spike;
  public LayerMask Player;
  private PlayerController playerController;
  public float Bouncespeed;

  void Start()
  {
    playerController = FindObjectOfType<PlayerController>();
    Spike = GetComponent<PolygonCollider2D>();
  }

  void FixedUpdate()
  {
    if (Spike.IsTouchingLayers(Player) && !playerController.Invincibility)
    {
      playerController.KillPlayer();
    }
  }

}
