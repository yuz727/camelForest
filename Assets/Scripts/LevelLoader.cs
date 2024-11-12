using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  // public Animator transition; 
  public string levelName;
  public BoxCollider2D transitionTrigger;
  public LayerMask playerLayer;

  public void Update()
  {
    if (transitionTrigger.IsTouchingLayers(playerLayer))
      SceneManager.LoadScene(levelName);
    // StartCoroutine(LoadLevel());
  }

  // IEnumerator LoadLevel()
  // {
  // transition.SetTrigger("Start");
  // yield return new WaitForSeconds(1f);

  // }
}
