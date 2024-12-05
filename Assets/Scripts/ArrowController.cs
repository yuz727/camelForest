using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
  public Rigidbody2D ArrowBody;
  public BoxCollider2D Hitbox;
  public LayerMask Enemy;
  private bool _flying;
  public bool Hit;
  public float Timer;
  // Start is called before the first frame update
  void Awake()
  {
    _flying = false;
    Hitbox = GetComponent<BoxCollider2D>();
    ArrowBody = GetComponent<Rigidbody2D>();
    Enemy = LayerMask.GetMask("Enemy");
  }

  // Update is called once per frame
  void Update()
  {
    if (_flying)
    {
      Timer += Time.deltaTime;
      if (Hitbox.IsTouchingLayers(Enemy))
      {
        Hit = true;
      }
    }
  }

  public void Fire(int direction)
  {
    _flying = true;
    Hit = false;
    this.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
    ArrowBody.velocity = new Vector2(direction * 10f, 0);
    Timer = 0;
  }
}

