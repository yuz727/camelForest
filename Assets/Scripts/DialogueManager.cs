using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
  public Text characterName;
  public Text sentence;

  public Queue<string> dialogueSentences = new Queue<string>();

  public void StartDialogue(Dialogue dialogue)
  {
    dialogueSentences.Clear();

    characterName.text = dialogue.NPCName;
    foreach (string dialogueSentence in dialogue.Sentences)
    {
      dialogueSentences.Enqueue(dialogueSentence);
    }
    var nextSetence = dialogueSentences.Dequeue();
    StopAllCoroutines();
    StartCoroutine(TypeSentence(nextSetence));
  }

  IEnumerator TypeSentence(string s)
  {
    sentence.text = "";
    foreach (char letter in s.ToCharArray())
    {
      sentence.text += letter;
      yield return null;
    }
  }

  public void DisplayNextSentence()
  {
    if (dialogueSentences.Count == 0)
    {
      // FindObjectOfType<NPCOneEvent>().NextTrigger();
      return;
    }
    var nextSetence = dialogueSentences.Dequeue();
    StopAllCoroutines();
    StartCoroutine(TypeSentence(nextSetence));
  }

}
