using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingsPopup;  // 환경설정 팝업
    public GameObject setMain;

    public void OnClick()
    {
        settingsPopup.SetActive(true);  // 환경설정 팝업을 활성화
        setMain.SetActive(false);
    }
}

