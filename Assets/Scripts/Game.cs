using System;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static Game Instance;

    [SerializeField]
    public float timeRemaining = 10;

    [SerializeField]
    TextMeshProUGUI TimerText;

    private bool lose;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        AudioManager.Instance.PlayMainMusic();


        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //HideMobileControls(false);
        }
        else
        {
            //HideMobileControls(true);
        }
    }

    public string FormatToMinutesSeconds(float secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);

        return string.Format("{0:D2}:{1:D2}",
                        t.Minutes,
                        t.Seconds);
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;


        if (timeRemaining < 0)
        {
            TimerText.text = $"00:00";
            //SceneManager.LoadScene(0);
            lose = true;
            //Player.Instance.Lose = true;
        }
        else
        {
            if (timeRemaining < 6)
            {
                TimerText.color = Color.red;
                AudioManager.Instance.PlaySFX(AudioManager.AudioId.Beep);
            }
            TimerText.text = $"{FormatToMinutesSeconds(timeRemaining)}";
        }
    }
}
