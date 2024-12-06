using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class CamelController : NPCController
{
  public Animator Chair;
  public GameObject Intro;
  private bool _isTalking;
  private bool _continue;
  public TextAsset ContinueDialogue;
  public GameObject Sakiko;
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    _continue = false;
    Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
  }

  void Update()
  {
    if (NPC != null)
    {
      if (NPC.IsTouchingLayers(Player))
      {
        NPC = null;
        LoadDialogue(NPCDialogue);
      }
    }
    if (InputHandling.CheckInteract())
    {
      if (!_isTalking && !_continue)
      {
        StartCoroutine(ChairAnim());
        Chair.SetBool("isPlay", true);
      }
      else if (!_isTalking && _continue)
      {
        this.gameObject.SetActive(false);
      }
      _isTalking = DisplayDialogue();
      if (!_isTalking && !_continue)
      {
        StartCoroutine(ChairAnim());
        Chair.SetBool("isPlay", true);
      }
      else if (!_isTalking && _continue)
      {
        Sakiko.GetComponent<Rigidbody2D>().velocity = new Vector3(20f, 20f, 0);
        this.gameObject.SetActive(false);
      }
    }
  }

  void LoadDialogue(TextAsset dialogue)
  {
    Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(dialogue.text).CollectionToQueue();
    playerController.Talking = true;
    canvasController.OpenDialogueBox();
    _isTalking = DisplayDialogue();
  }

  IEnumerator ChairAnim()
  {
    yield return new WaitForSeconds(0.8f);
    Intro.SetActive(false);
    Sakiko.GetComponent<SpriteRenderer>().enabled = true;
    _continue = true;
    LoadDialogue(ContinueDialogue);
  }
}
