using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform playerLocation;

  // Update is called once per frame
  void Update()
  {
    this.transform.position = playerLocation.transform.position + new Vector3(0, 3, -10);
  }
}
