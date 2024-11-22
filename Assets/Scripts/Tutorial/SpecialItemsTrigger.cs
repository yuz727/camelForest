using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemsTrigger : MonoBehaviour
{
  public LayerMask Player;
  public GameObject RockSprite;
  public BoxCollider2D Rock;
  public PlayerController playerController;
  // Start is called before the first frame update
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Rock.IsTouchingLayers(Player) || playerController.SpecialItem != SpecialItems.None)
    {
      if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button3))
      {
        RockSprite.SetActive(false);
      }
    }
  }
}
