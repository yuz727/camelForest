using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform playerLocation;

  public float translationFactor = 20;

  void LateUpdate()
  {
    if (transform.position != playerLocation.position)
    {
      transform.position = new Vector3(transform.position.x + (playerLocation.position.x - transform.position.x) / translationFactor,
                             3 + playerLocation.position.y + (playerLocation.position.y - transform.position.y) / translationFactor,
                             -10);
    }
  }
}
