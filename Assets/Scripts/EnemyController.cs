using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public float moveSpeed;
  public float moveDistance;
  public float idelTime;
  public float h = -1;
  float distance = 0;
  float time = 0;
  bool isDead = false;
  Animator anim;
  SpriteRenderer sprt;
  public PolygonCollider2D Hitbox;
  public LayerMask Arrow;
  // Start is called before the first frame update
  void Start()
  {
    anim = transform.GetComponent<Animator>();
    sprt = transform.GetComponent<SpriteRenderer>();
    Hitbox = GetComponent<PolygonCollider2D>();
    // Arrow = LayerMask.GetMask("Arrow");
  }

  // Update is called once per frame
  void Update()
  {
    if (isDead)
    {
      return;
    }
    if (Hitbox.IsTouchingLayers(Arrow))
    {
      Die();
      return;
    }
    if (distance > moveDistance)
    {
      anim.SetBool("isRun", false);
      time += Time.deltaTime;
      if (time > idelTime)
      {
        h = -h;
        distance = 0;
        time = 0;
        if (h > 0)
        {
          sprt.flipX = true;
        }
        else if (h < 0)
        {
          sprt.flipX = false;
        }
      }
    }
    else
    {
      anim.SetBool("isRun", true);
      float speed = moveSpeed * Time.deltaTime;
      distance += speed;
      transform.Translate(new Vector2(h * speed, 0));
    }

  }
  public void Die()
  {
    if (isDead)
    {
      return;
    }
    isDead = true;
    anim.SetBool("isDie", true);
    Destroy(gameObject, 0.3f);
  }
}
