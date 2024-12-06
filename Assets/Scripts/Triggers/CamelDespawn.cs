using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamelDespawn : MonoBehaviour
{
  public BoxCollider2D HitBox;
  public LayerMask Sakiko;
  public GameObject CamelA;
  // Update is called once per frame

  void Update()
  {
    if (HitBox.IsTouchingLayers(Sakiko))
    {
      CamelA.SetActive(false);
      this.gameObject.SetActive(false);
    }
  }
}
