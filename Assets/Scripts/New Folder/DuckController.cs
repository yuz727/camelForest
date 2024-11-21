using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
  public BoxCollider2D PlayerFeet;
  public PlayerController PlayerController;
  public BoxCollider2D DuckCollider;
  public float Bouncespeed;

  void Start()
  {
    PlayerFeet = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<BoxCollider2D>();
    PlayerController = FindObjectOfType<PlayerController>();
  }

  void FixedUpdate()
  {
    if (PlayerFeet.IsTouching(DuckCollider))
    {
      PlayerController.DuckJump(Bouncespeed);
    }
  }

}
