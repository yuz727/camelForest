using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DuckController : MonoBehaviour
{
  public BoxCollider2D PlayerFeet;
  public PlayerController PlayerController;
  public BoxCollider2D DuckCollider;
  public Animator DuckAnim;
  public float Bouncespeed;

  void Start()
  {
    PlayerFeet = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<BoxCollider2D>();
    PlayerController = FindObjectOfType<PlayerController>();
  }

  void Update()
  {
    if (PlayerFeet.IsTouching(DuckCollider))
    {
      Debug.Log("Did'not");
      DuckAnim.SetBool("isJump", true);
      PlayerController.DuckJump(Bouncespeed);
      StartCoroutine(AnimTimer());
    }
  }

  private IEnumerator AnimTimer()
  {
    yield return new WaitForSeconds(0.3f);
    DuckAnim.SetBool("isJump", false);
  }

}
