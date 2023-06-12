using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;
public class DialogueManager : MonoBehaviour
{
    public GameObject player; // player 객체
    public Vector3 playerPositionAfterCutscene; // 컷신 후 player를 이동시킬 위치
    public List<string> dialogues;
    private int currentDialogueIndex;

    public DialogueUI dialogueUI;

    private InputAction zAction;
    private InputAction upArrowAction; // 윗화살표를 위한 InputAction

    public PlayableDirector director;
    public CinemachineVirtualCamera cutsceneCam;
    
    private bool isDialogueStarted = false;
    private bool isPlayerNear = false; // player가 인터렉션 영역에 있는지

    void Start()
    {
        player = Player.Instance.gameObject;
        zAction = new InputAction("ZPress", InputActionType.Button, "<Keyboard>/z");
        upArrowAction = new InputAction("UpArrowPress", InputActionType.Button, "<Keyboard>/upArrow"); // 윗화살표를 위한 InputAction 초기화

        zAction.Enable();
        upArrowAction.Enable(); // 윗화살표를 위한 InputAction 활성화

        currentDialogueIndex = 0;

        director.played += Director_played;
        director.stopped += Director_stopped;
    }

    private void Director_played(PlayableDirector obj)
    {
        cutsceneCam.Priority = 11;
        player.SetActive(false); // 컷신이 시작되면 player를 비활성화
    }

    private void Director_stopped(PlayableDirector obj)
    {
        cutsceneCam.Priority = 0;
        if (player != null) 
        {
            player.transform.position = playerPositionAfterCutscene; // player를 특정 위치로 이동
            player.SetActive(true); // 컷신이 끝나면 player를 활성화
        }
    }
    public void StartDialogue()
    {
        ShowNextDialogue();
        isDialogueStarted = true;
    }

    void Update()
    {
        if (isDialogueStarted && zAction.triggered)
        {
            ShowNextDialogue();
        }
            
        if (isPlayerNear && upArrowAction.triggered) // player가 인터렉션 영역에 있고 윗화살표를 눌렀을 때
        {
            director.Play();
            // ShowNextDialogue();
            // isDialogueStarted = true;
        }
    }

    public void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            dialogueUI.ShowDialogue(dialogues[currentDialogueIndex]);
            currentDialogueIndex++;
        }
        else
        {
            dialogueUI.HideDialogue();
            isDialogueStarted = false;

            // 대화가 끝났으므로, 컷신을 종료합니다.
            StopCutscene();
        }
    }
        private void StopCutscene()
    {
        director.Stop();  // 타임라인을 직접 종료합니다.
        // 타임라인이 종료되면 `Director_stopped`가 호출됩니다.
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // player가 인터렉션 영역에 들어왔을 때
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false; // player가 인터렉션 영역에서 나갔을 때
        }
    }

    private void OnDisable()
    {
        zAction.Disable();
        upArrowAction.Disable(); // 윗화살표를 위한 InputAction 비활성화
    }
}