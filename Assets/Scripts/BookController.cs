using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
  public Rigidbody2D Book;
  public BoxCollider2D Hitbox;
  public LayerMask Camel;
  private bool _flying;
  public bool Hit;
  public float Timer;
  // Start is called before the first frame update
  void Awake()
  {
    _flying = false;
    Hitbox = GetComponent<BoxCollider2D>();
    Book = GetComponent<Rigidbody2D>();
    Camel = LayerMask.GetMask("Sakiko");
  }

  // Update is called once per frame
  void Update()
  {
    if (_flying)
    {
      transform.Rotate(Vector3.back, 10f);
      Timer += Time.deltaTime;
      if (Hitbox.IsTouchingLayers(Camel))
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
    Book.velocity = new Vector2(direction * 10f, 0);
    Timer = 0;
  }
}
