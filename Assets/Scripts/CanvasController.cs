using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
  public GameObject DialogueBox;
  public DialogueManager dialogueManager;
  public Image Wood;
  public Image Anon;
  public Image Tomori;
  public Image Soyo;
  public Image Cat;
  public Image Riki;
  public Image Gabe;
  public Image Kun;
  public Image Sakiko;
  public Image Current;
  public Image Statue;
  public Image Empty;
  public bool Active;
  void Start()
  {
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    Current = Wood;
  }
  void Update()
  {
    if (Active)
    {
      string name = dialogueManager.characterName.text;
      Current.enabled = false;
      Current = name switch
      {
        "Wood" => Wood,
        "London" => Anon,
        "GOAT" => Tomori,
        "Veggie" => Soyo,
        "Cat" => Cat,
        "Shiitake" => Riki,
        "ShroomTrainer" => Kun,
        "Duck" => Gabe,
        "Camel" => Sakiko,
        "Statue" => Statue,
        _ => Empty,
      };
      Current.enabled = true;
    }
  }
  public void OpenDialogueBox()
  {
    DialogueBox.SetActive(true);
    Active = true;
  }
  public void CloseDialogueBox()
  {
    DialogueBox.SetActive(false);
    Active = false;
    Current.enabled = false;
  }
}
