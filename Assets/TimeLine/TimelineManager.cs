using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director1;
    public PlayableDirector director2;

    void Start()
    {
        director1.stopped += OnDirector1Stopped;
        director1.Play();
    }

    void OnDirector1Stopped(PlayableDirector pd)
    {
        director1.stopped -= OnDirector1Stopped;
        director2.Play();
    }
}