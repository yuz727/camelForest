using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class ItemController : MonoBehaviour
{
  public static ItemController S_Instance;
  public PlayerController playerController;
  public GameObject Player;
  public ItemUIController itemUIController;
  public BookController bookController;
  public CanvasController canvasController;
  public ArrowController arrowController;
  public DialogueTrigger dialogueTrigger;
  public SpecialItems SpecialItem;
  public SpecialItems SpecialItemUsed;
  public GameObject NoteBook;
  public GameObject _thisBook;
  public GameObject Arrow;
  public GameObject _thisArrow;
  public Renderer Bow;
  public List<Items> ItemsOwned;
  public int _index;
  public int _itemSpaceRemaining;
  private int _nextOpening;
  private bool _canCucumber = true;
  private bool _canMushroom = true;
  private bool _canNotebook = true;
  public int ArrowCount;
  public int CucumberCount;
  public bool ReplacingItem;
  public CartState cartState;
  public int ArrowFire;
  public int BookIndex;

  void Update()
  {

    var newIndex = InputHandling.CheckSwitchItem(_index);
    if (!playerController.Talking)
    {
      if (InputHandling.CheckDiscardItem())
      {
        DiscardItem();
      }
    }
    if (ArrowFire > -1)
    {
      if (arrowController.Hit)
      {
        Destroy(_thisArrow);
        ReduceArrow();
        _thisArrow = null;
        arrowController = null;
      }
      else if (arrowController.Timer >= 2f)
      {
        Destroy(_thisArrow);
        ArrowFire = -1;
        _thisArrow = null;
        arrowController = null;
      }
    }
    if (BookIndex > -1)
    {
      if (bookController.Hit)
      {
        Debug.Log("hit");
        Destroy(_thisBook);
        BookIndex = -1;
        _thisBook = null;
        bookController = null;
        _canNotebook = true;
      }
      else if (bookController.Timer >= 0.75f)
      {
        Destroy(_thisBook);
        BookIndex = -1;
        _thisBook = null;
        bookController = null;
        _canNotebook = true;
      }
    }
    if (newIndex != -1)
    {
      itemUIController.UpdateSelect(newIndex);
      _index = newIndex;
    }
    if (Bow != null)
    {
      if (ItemsOwned[_index] == Items.Bow) Bow.enabled = true;
      else Bow.enabled = false;
    }
    if (ReplacingItem && InputHandling.CheckInteract())
    {
      ReplacingItem = false;
      playerController.Talking = false;
      canvasController.CloseDialogueBox();
      return;
    }
    if (ReplacingItem && InputHandling.CheckUseItem())
    {
      RemoveItem(_index);
      itemUIController.UpdateSlots(Items.Empty, _index);
      ReplacingItem = false;
      playerController.Talking = false;
      canvasController.CloseDialogueBox();
      return;
    }
    else if (!playerController.Talking && InputHandling.CheckUseItem()) UseItem(_index);

  }

  void DiscardItem()
  {
    canvasController.OpenDialogueBox();
    playerController.Talking = true;
    ReplacingItem = true;
    dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "Press Use on the item to discard it, or Press Interact if you want to replace none of it." } };
    dialogueTrigger.TriggerDialogue();
  }

  public void SetSpecialItem(SpecialItems specialItem)
  {
    if (SpecialItem != specialItem)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      SpecialItem = specialItem;
      itemUIController.UpdateSpecialItem(specialItem);
      FindObjectOfType<DialogueTrigger>().Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {specialItem}!" } };
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      StartCoroutine(DialogueUITimer());
    }
  }
  public bool AddItem(Items item)
  {
    if (_itemSpaceRemaining == 0)
    {
      canvasController.OpenDialogueBox();
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "Inventory is full, need to discard item to make space." } };
      dialogueTrigger.TriggerDialogue();
      StartCoroutine(DialogueUITimer());
      return false;
    }
    if (!ItemsOwned.Contains(item))
    {
      for (int i = 0; i < 3; i++)
      {
        if (ItemsOwned[i] == Items.Empty)
        {
          _nextOpening = i;
          break;
        }
      }
      canvasController.OpenDialogueBox();
      switch (item)
      {
        case Items.Bow:
          ArrowCount = 20;
          itemUIController.ArrowCount.text = "20";
          goto default;
        case Items.Cucumber:
          CucumberCount = 5;
          itemUIController.CucumberCount.text = "5";
          goto default;
        default:
          ItemsOwned[_nextOpening] = item;
          itemUIController.UpdateSlots(item, _nextOpening);
          _itemSpaceRemaining--;
          dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {item}!" } };
          dialogueTrigger.TriggerDialogue();
          StartCoroutine(DialogueUITimer());
          return true;
      }
    }
    return false;
  }


  private IEnumerator DialogueUITimer()
  {
    yield return new WaitForSeconds(2f);
    canvasController.CloseDialogueBox();
  }

  public void ShootBow(int index)
  {
    _thisArrow = Instantiate(Arrow, Player.transform.position, Quaternion.identity);
    int dir = playerController._facingRight ? 1 : -1;
    arrowController = _thisArrow.GetComponent<ArrowController>();
    arrowController.Fire(dir);
    ArrowFire = index;
  }

  private void ReduceArrow()
  {
    ArrowCount--;
    itemUIController.ArrowCount.text = ArrowCount.ToString();
    if (ArrowCount <= 0)
    {
      RemoveItem(ArrowFire);
    }
    ArrowFire = -1;
  }
  public void ThrowBook(int index)
  {
    _thisBook = Instantiate(NoteBook, Player.transform.position, Quaternion.identity);
    int dir = playerController._facingRight ? 1 : -1;
    bookController = _thisBook.GetComponent<BookController>();
    bookController.Fire(dir);
    BookIndex = index;
  }
  private void UseItem(int index)
  {
    switch (ItemsOwned[index])
    {
      case Items.Bow:
        if (_thisArrow == null)
        {
          ShootBow(index);
        }
        break;
      case Items.Cucumber:
        if (_canCucumber)
        {
          StartCoroutine(CucumberTrigger());
          playerController.Invincibility = true;
          playerController.Vfx.SetBool("cVfx", true);
          _canCucumber = false;
          CucumberCount--;
          itemUIController.CucumberCount.text = CucumberCount.ToString();
          if (CucumberCount == 0)
          {
            RemoveItem(index);
          }
        }
        break;
      case Items.Mushroom:
        if (_canMushroom)
        {
          StartCoroutine(MushroomTrigger());
          playerController.Vfx.SetBool("mVfx", true);
          playerController.Mushroomed = true;
          _canMushroom = false;
        }
        break;
      case Items.Notebook:
        if (_canNotebook)
        {
          StartCoroutine(NotebookTrigger());
          playerController.Notebooking = true;
          _canNotebook = false;
          ThrowBook(index);
        }
        break;
      default:
        break;
    }
  }

  public void UseItem(Items item)
  {
    for (int i = 0; i < ItemsOwned.Count; i++)
    {
      if (ItemsOwned[i] == item)
      {
        RemoveItem(i);
      }
    }
  }


  private void RemoveItem(int index)
  {
    itemUIController.UpdateSlots(Items.Empty, index);
    ItemsOwned[index] = Items.Empty;
    _itemSpaceRemaining++;
  }

  private IEnumerator CucumberTrigger()
  {
    yield return new WaitForSeconds(5f);
    playerController.Vfx.SetBool("cVfx", false);
    playerController.Anim.SetBool("isCucumber", false);
    playerController.Invincibility = false;
    _canCucumber = true;
  }

  private IEnumerator MushroomTrigger()
  {
    yield return new WaitForSeconds(10f);
    playerController.Vfx.SetBool("mVfx", false);
    playerController.Mushroomed = false;
    _canMushroom = true;
  }

  private IEnumerator NotebookTrigger()
  {
    yield return new WaitForSeconds(0.6f);
    playerController.Notebooking = false;
    playerController.Anim.SetBool("isThrow", false);
  }

  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      SpecialItem = SpecialItems.None;
      cartState = CartState.Basketball;
      CucumberCount = 5;
      ArrowCount = 20;
      _itemSpaceRemaining = 3;
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
    if (scene.name.Equals("level1"))
    {
      if (ItemsOwned[0] == Items.Bow)
      {
        RemoveItem(0);
      }
    }
    if (scene.name.Equals("Menu") || scene.name.Equals("Credits"))
    {
      this.gameObject.SetActive(false);
      Destroy(gameObject);
    }
    else if (!scene.name.Equals("Menu") || !scene.name.Equals("Credits"))
    {
      ArrowFire = -1;
      BookIndex = -1;
      itemUIController = FindFirstObjectByType<ItemUIController>();
      canvasController = FindFirstObjectByType<CanvasController>();
      playerController = FindFirstObjectByType<PlayerController>();
      dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
      bookController = FindFirstObjectByType<BookController>();
      Player = GameObject.FindGameObjectWithTag("Player");
      Arrow = GameObject.FindGameObjectWithTag("Arrow");
      Bow = GameObject.FindGameObjectWithTag("Bow").GetComponent<SpriteRenderer>();
      NoteBook = GameObject.FindGameObjectWithTag("Notebook");
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