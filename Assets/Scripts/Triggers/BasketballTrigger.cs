using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasketballTrigger : MonoBehaviour
{
  public PlayerController playerController;
  public LayerMask player;
  public CircleCollider2D box;
  public Renderer render;
  private bool _work;
  // Start is called before the first frame update
  void Start()
  {
    _work = true;
    playerController = FindFirstObjectByType<PlayerController>();
    render = GetComponent<Renderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (_work && box.IsTouchingLayers(player))
    {
      playerController.ExtraJump = 1;
      _work = false;
      render.enabled = false;
      StartCoroutine(ResetTimer());
    }
  }
  IEnumerator ResetTimer()
  {
    yield return new WaitForSeconds(2f);
    _work = true;
    render.enabled = true;
  }
}
