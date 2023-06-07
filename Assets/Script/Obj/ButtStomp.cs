using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtStomp : MonoBehaviour
{   
    public GameObject DashObj;
    public GameObject GoodObj;
    public void Break()
    {
        DashObj.SetActive(false);
        GoodObj.SetActive(true);
        Destroy(gameObject);
    }
}
