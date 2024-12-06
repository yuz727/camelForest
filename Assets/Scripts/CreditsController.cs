using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
  public TMP_Text title;
  public TMP_Text producer;
  public TMP_Text returnButton;
  public bool Done;
  // Start is called before the first frame update
  void Start()
  {
    title.enabled = true;
    producer.enabled = false;
    returnButton.enabled = false;
    StartCoroutine(Timer1());
  }
  void Update()
  {
    if (Done && Input.anyKeyDown)
    {
      SceneManager.LoadScene("Menu");
    }
  }

  IEnumerator Timer1()
  {
    yield return new WaitForSeconds(2f);
    title.enabled = false;
    producer.enabled = true;
    StartCoroutine(Timer2());
  }

  IEnumerator Timer2()
  {
    yield return new WaitForSeconds(2f);
    Done = true;
    returnButton.enabled = true;
  }
}
