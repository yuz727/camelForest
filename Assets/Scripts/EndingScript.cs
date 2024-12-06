using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using System;
public class EndingScript : NPCController
{
  public TextAsset E2;
  public TextAsset E1C;
  public Animator Anim;
  public Animator transition;
  public Animator WaterAnim;
  public GameObject Tank;
  public LayerMask Water;
  private bool _isTalking;
  private bool _inEnding;
  private bool _E1E;
  public bool _E1Transition;
  private int _status;

  // Start is called before the first frame update
  void Start()
  {
    _inEnding = false;
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = FindFirstObjectByType<ItemController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (_E1Transition)
    {
      if (NPC.IsTouchingLayers(Water))
      {
        GetComponent<SpriteRenderer>().enabled = false;
        Anim.enabled = false;
        WaterAnim.SetBool("isSaki", true);
        Tank.transform.position = this.transform.position;
        LoadDialogue(E1C);
        _E1Transition = false;
        _E1E = true;
      }
    }
    if (_inEnding && InputHandling.CheckInteract())
    {
      _isTalking = DisplayDialogue();
      if (!_isTalking && _status == 2)
      {
        StartCoroutine(LoadCredits("EndingFade"));
      }
      if (!_isTalking && _status == 1 && _E1E)
      {
        StartCoroutine(LoadCredits("FadeOut"));
      }
      if (!_isTalking && _status == 1 && !_E1E)
      {
        _E1Transition = true;
        Tank.GetComponent<Rigidbody2D>().gravityScale = 1;
        playerController.Talking = true;
      }
    }
    if (!_inEnding && NPC.IsTouchingLayers(Player))
    {
      EndingTwo();
    }
  }

  public void EndingOne()
  {
    // NPC.enabled = false;
    _inEnding = true;
    _status = 1;
    LoadDialogue(NPCDialogue);
  }

  public void EndingTwo()
  {
    _inEnding = true;
    _status = 2;
    Anim.SetBool("isDie", true);
    LoadDialogue(E2);
  }

  void LoadDialogue(TextAsset dialogue)
  {
    Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(dialogue.text).CollectionToQueue();
    playerController.Talking = true;
    canvasController.OpenDialogueBox();
    _isTalking = DisplayDialogue();
  }
  IEnumerator LoadCredits(string trigger)
  {
    transition.SetTrigger(trigger);
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene("Credits");
  }
}
