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
    public RectTransform image1Rect;
    public RectTransform image2Rect;
    public float moveAmount = 100f;  // 이동할 양
    public float duration = 2.0f;  // 애니메이션에 걸리는 시간

    private Vector2 image1OriginalPos;
    private Vector2 image2OriginalPos;
    private bool isDialogueInProgress = false;
    public bool hasPlayedCutscene = false;

    void Start()
    {
        player = Player.Instance.gameObject;
        zAction = new InputAction("ZPress", InputActionType.Button, "<Keyboard>/z");
        zAction.Enable();
        currentDialogueIndex = 0;
        director.played += Director_played;
        director.stopped += Director_stopped;
        image1OriginalPos = image1Rect.anchoredPosition;
        image2OriginalPos = image2Rect.anchoredPosition;
        if (PlayerPrefs.HasKey("HasPlayedCutscene"))
        {
            hasPlayedCutscene = PlayerPrefs.GetInt("HasPlayedCutscene") == 1;
        }
    }

    private void Director_played(PlayableDirector obj)
    {
        PlayerPrefs.SetInt("HasPlayedCutscene", 1);
        hasPlayedCutscene = true;
        cutsceneCam.Priority = 11;
        player.SetActive(false); // 컷신이 시작되면 player를 비활성화
        StartCoroutine(MoveImage(image1Rect, image1Rect.anchoredPosition.y + moveAmount, duration)); // Image1을 +y 방향으로 이동
        StartCoroutine(MoveImage(image2Rect, image2Rect.anchoredPosition.y - moveAmount, duration));
    }

    private void Director_stopped(PlayableDirector obj)
    {
        cutsceneCam.Priority = 0;
        if (player != null) 
        {
            player.transform.position = playerPositionAfterCutscene; // player를 특정 위치로 이동
            player.SetActive(true); // 컷신이 끝나면 player를 활성화

            // 플레이어가 오른쪽을 바라보게 만드는 코드
            Vector3 playerScale = player.transform.localScale;
            player.transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z); // 플레이어가 오른쪽을 바라보도록 설정
        }
        StartCoroutine(MoveImage(image1Rect, image1OriginalPos.y, duration)); // Image1을 원래 위치로 이동
        StartCoroutine(MoveImage(image2Rect, image2OriginalPos.y, duration)); // Image2를 원래 위치로 이동
        Input.ResetInputAxes();
        player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
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
        if (currentDialogueIndex < dialogues.Count && !isDialogueInProgress)
        {
            StartCoroutine(ShowDialogueAfterDelay(0.2f));
        }
        else if (!isDialogueInProgress)
        {
            dialogueUI.HideDialogue();
            isDialogueStarted = false;

            // 대화가 끝났으므로, 컷신을 종료합니다.
            director.Stop();

            // 대화가 끝나면 'cat' 게임 오브젝트를 비활성화
            cat.SetActive(false);
        }
    }
    private IEnumerator ShowDialogueAfterDelay(float delay)
    {
        isDialogueInProgress = true;
        
        yield return new WaitForSeconds(delay);

        StartCoroutine(dialogueUI.ShowDialogue(dialogues[currentDialogueIndex]));
        currentDialogueIndex++;

        yield return StartCoroutine(WaitForDialogueToFinish());  // 이 코루틴이 완료될 때까지 대기합니다.

        isDialogueInProgress = false;
    }
        private IEnumerator WaitForDialogueToFinish()
    {
        while (dialogueUI.isDialogueRunning)  // dialogueUI에 isDialogueRunning 이라는 bool 변수가 있어야 합니다.
        {
            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // player가 인터렉션 영역에 들어왔을 때

            // 대화가 아직 시작되지 않았거나 대화가 끝난 경우에만 재생
            if (!hasPlayedCutscene && (!isDialogueStarted || (isDialogueStarted && !dialogueUI.isActiveAndEnabled)))
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
    IEnumerator MoveImage(RectTransform imageRect, float targetY, float duration)
    {
        float startTime = Time.time;
        float originalY = imageRect.anchoredPosition.y;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            Vector2 newPosition = imageRect.anchoredPosition;
            newPosition.y = Mathf.Lerp(originalY, targetY, t);
            imageRect.anchoredPosition = newPosition;

            yield return null;
        }

        Vector2 finalPosition = imageRect.anchoredPosition;
        finalPosition.y = targetY;
        imageRect.anchoredPosition = finalPosition;
    }
}