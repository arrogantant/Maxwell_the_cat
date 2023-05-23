using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject laserPrefab;
    public int size = 20; // 초기 레이저 풀의 크기

    private Queue<GameObject> lasers;

    private void Awake()
    {
        // 싱글톤 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 레이저 풀 초기화
        lasers = new Queue<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            GameObject laser = Instantiate(laserPrefab);
            laser.SetActive(false);
            lasers.Enqueue(laser);
        }
    }

    public GameObject GetLaser()
    {
        // 풀에서 레이저 가져오기
        if (lasers.Count == 0)
        {
            // 풀이 비어있는 경우 새 레이저 생성
            GameObject laser = Instantiate(laserPrefab);
            laser.SetActive(false);
            return laser;
        }
        else
        {
            return lasers.Dequeue();
        }
    }

    public void ReturnLaser(GameObject laser)
    {
        // 풀에 레이저 반환
        laser.SetActive(false);
        lasers.Enqueue(laser);
    }
}
