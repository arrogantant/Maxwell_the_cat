using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerCameraManager : MonoBehaviour
{
    public static PlayerCameraManager Instance { get; private set; } // 싱글톤 인스턴스

    public CinemachineVirtualCamera playerCam;

    private void Awake()
    {
        if (Instance != null && Instance != this) // 싱글톤 인스턴스가 이미 존재하는 경우
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this; // 싱글톤 인스턴스를 설정
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
