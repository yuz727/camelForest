using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L3EntryTrigger : MonoBehaviour
{
  public GameObject L4Transition;
  public GameObject L5Transition;
  // Start is called before the first frame update
  void Start()
  {
    PlayerController playerController = FindFirstObjectByType<PlayerController>();
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    GameObject block = GameObject.FindGameObjectWithTag("wall");
    GameObject rana = GameObject.FindGameObjectWithTag("NPC");
    GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
    if (playerController.CanDash)
    {
      L4Transition.SetActive(false);
      block.SetActive(false);
      camera.transform.position = new(146.3f, 55.1f, -10f);
      player.transform.position = new(147.02f, 53.23f, -5f);
      rana.transform.position = new(151.3f, 53f, 1f);
    }
    else
    {
      L5Transition.SetActive(false);
      camera.transform.position = new(-1.72f, 8.46f, -10f);
      player.transform.position = new(-1.68f, 5.73f, -5f);
      rana.transform.position = new(-3.78f, 6.22f, 1f);
    }
  }
}
