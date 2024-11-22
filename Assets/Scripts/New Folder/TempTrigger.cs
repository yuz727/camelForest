using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTrigger : MonoBehaviour
{
  public PlayerController playerController;
  public LayerMask player;
  public BoxCollider2D box;
  // Start is called before the first frame update
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (box.IsTouchingLayers(player))
    {
      playerController.RespawnPoint = box.transform.position;
      playerController.CanDoubleJump = true;
    }
  }
}
