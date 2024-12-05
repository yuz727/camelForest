using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public Animator transition;
  public PlayerController playerController;
  public string levelName;
  public BoxCollider2D transitionTrigger;
  public LayerMask playerLayer;

  void Start()
  {
    transition = GameObject.FindGameObjectWithTag("LevelTransition").GetComponent<Animator>();
    playerController = FindFirstObjectByType<PlayerController>();
  }

  public void Update()
  {
    if (transitionTrigger.IsTouchingLayers(playerLayer))
      StartCoroutine(LoadLevel());
  }


  IEnumerator LoadLevel()
  {
    transition.SetTrigger("FadeOut");
    yield return new WaitForSeconds(1f);
    // if (SceneManager.GetActiveScene().name.Equals("level0"))
    // {
    //   playerController.d
    // }
    SceneManager.LoadScene(levelName);
  }
}
