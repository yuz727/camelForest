using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemController : MonoBehaviour
{
  public static ItemController S_Instance;
  public PlayerController playerController;
  public CanvasController canvasController;
  public DialogueTrigger dialogueTrigger;
  public SpecialItems SpecialItem;
  public List<Items> ItemsOwned;
  public Rigidbody2D PlayerBody;
  private int _index;
  private int _itemSpaceRemaining;
  private bool _canCucumber = true;
  private bool _canMushroom = true;
  private int _arrowCount;
  private int _cucumberCount;

  void Update()
  {
    if (InputHandling.CheckUseItem()) UseItem(_index);
  }

  public void SetSpecialItem(SpecialItems specialItem)
  {
    if (SpecialItem != specialItem)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      SpecialItem = specialItem;
      FindObjectOfType<DialogueTrigger>().Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {specialItem}!" } };
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      StartCoroutine(DialogueUITimer());
    }
    else
    {
      Debug.Log("Item Owned Already");
    }
  }
  public void AddItem(Items item)
  {
    if (ItemsOwned.Count >= 3)
    {
      if (!MaxHandle())
      {
        return;
      }
    }
    if (!ItemsOwned.Contains(item))
    {
      canvasController.OpenDialogueBox();
      switch (item)
      {
        case Items.Bow:
          _arrowCount = 20;
          goto default;
        case Items.Cucumber:
          _cucumberCount = 5;
          goto default;
        default:
          ItemsOwned.Add(item);
          break;
      }
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {item}!" } };
      dialogueTrigger.TriggerDialogue();
      StartCoroutine(DialogueUITimer());
    }
  }

  private IEnumerator DialogueUITimer()
  {
    yield return new WaitForSeconds(2f);
    canvasController.CloseDialogueBox();
  }
  public void ShootBow()
  {
    //TODO: Implement
    _arrowCount--;
    if (_arrowCount <= 0)
    {
      RemoveItem(Items.Bow);
    }
  }

  private void UseItem(int index)
  {
    switch (ItemsOwned[index])
    {
      case Items.Bow:
        ShootBow();
        break;
      case Items.Cucumber:
        StartCoroutine(CucumberTrigger());
        playerController.Invincibility = true;
        _canCucumber = false;
        break;
      case Items.Mushroom:
        StartCoroutine(MushroomTrigger());
        playerController.Mushroomed = true;
        _canMushroom = false;
        break;
      default:
        break;
    }
  }

  // Overloaded method, 
  public bool UseItem(Items item)
  {
    if (!ItemsOwned.Contains(item))
    {
      return false;
    }
    RemoveItem(item);
    return true;
  }

  private bool MaxHandle()
  {
    canvasController.OpenDialogueBox();
    playerController.Talking = true;
    dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "Item bag full, choose the item to discard." } };

    playerController.Talking = false;
    canvasController.CloseDialogueBox();
    return false;
  }

  private void RemoveItem(Items item)
  {
    switch (item)
    {
      case Items.Bow:
        _arrowCount = -1;
        goto default;
      case Items.Cucumber:
        _cucumberCount = -1;
        goto default;
      default:
        ItemsOwned.Remove(item);
        break;
    }
  }

  private IEnumerator CucumberTrigger()
  {
    yield return new WaitForSeconds(5f);
    playerController.Invincibility = false;
    _canCucumber = true;
  }

  private IEnumerator MushroomTrigger()
  {
    yield return new WaitForSeconds(10f);
    playerController.Mushroomed = false;
    _canMushroom = true;
  }
  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      SpecialItem = SpecialItems.None;
      _cucumberCount = -1;
      _arrowCount = -1;
      ItemsOwned = new() { Items.Empty, Items.Empty, Items.Empty };
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log("Entering " + scene.name);
    if (!scene.name.Equals("Menu"))
    {
      canvasController = FindFirstObjectByType<CanvasController>();
      playerController = FindFirstObjectByType<PlayerController>();
      dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    }
  }
}

public enum SpecialItems
{
  None,
  Crowbar,
  Dynamite,
  Sword
}
public enum Items
{
  Empty,
  Key,
  Notebook,
  Mushroom,
  Bow,
  Cucumber
}