using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L4EntryTrigger : MonoBehaviour
{
  public GameObject L3Transition;
  public GameObject L5Transition;
  // Start is called before the first frame update
  void Start()
  {
    PlayerController playerController = FindFirstObjectByType<PlayerController>();
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    GameObject block = GameObject.FindGameObjectWithTag("wall");
    GameObject rana = GameObject.FindGameObjectWithTag("NPC");
    GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
    if (playerController.CanDoubleJump)
    {
      L3Transition.SetActive(false);
      block.SetActive(false);
      camera.transform.position = new(233.82f, -23.59f, -10f);
      player.transform.position = new(233.8f, -26.99f, -5f);
      rana.transform.position = new(231.98f, -26.31f, 1f);
      rana.GetComponent<SpriteRenderer>().flipX = true;
    }
    else
    {
      L5Transition.SetActive(false);
      camera.transform.position = new(6.33f, 0.6f, -10f);
      player.transform.position = new(6.31f, -2.8f, -5f);
      rana.transform.position = new(15.28f, -2.42f, 1f);

    }
  }

}
