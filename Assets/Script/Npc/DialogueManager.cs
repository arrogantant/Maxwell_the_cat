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

    public PlayableDirector director;
    public CinemachineVirtualCamera cutsceneCam;
    
    private bool isDialogueStarted = false;
    private bool isPlayerNear = false; // player가 인터렉션 영역에 있는지
    public GameObject cat;

    void Start()
    {
        player = Player.Instance.gameObject;
        zAction = new InputAction("ZPress", InputActionType.Button, "<Keyboard>/z");

        zAction.Enable();

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

            Vector3 playerScale = player.transform.localScale;
            player.transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z); // 플레이어가 오른쪽을 바라보도록 설정
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
    }

    public void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            StartCoroutine(dialogueUI.ShowDialogue(dialogues[currentDialogueIndex]));
            currentDialogueIndex++;
        }
        else
        {
            dialogueUI.HideDialogue();
            isDialogueStarted = false;

            // 대화가 끝났으므로, 컷신을 종료합니다.
            StopCutscene();

            // 대화가 끝나면 'cat' 게임 오브젝트를 비활성화
            cat.SetActive(false);
        }
    }
        private void StopCutscene()
    {
        director.Stop();  // 타임라인을 직접 종료합니다.
        // 타임라인이 종료되면 `Director_stopped`가 호출됩니다.
        DisableAnimationTrack(); // 애니메이션 트랙 비활성화

        director.gameObject.SetActive(false); // 타임라인 오브젝트를 비활성화합니다.
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // player가 인터렉션 영역에 들어왔을 때

            // 대화가 아직 시작되지 않았거나 대화가 끝난 경우에만 재생
            if (!isDialogueStarted || (isDialogueStarted && !dialogueUI.isActiveAndEnabled))
            {
                director.Play();
            }
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
    }
    private void DisableAnimationTrack()
    {
        Animator animator = cat.GetComponent<Animator>();
        animator.enabled = false; 
    }
}