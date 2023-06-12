using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; // 추가

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector director;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            director.Play();
        }
    }
}