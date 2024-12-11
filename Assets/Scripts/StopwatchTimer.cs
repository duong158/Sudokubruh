using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StopwatchTimer : MonoBehaviour
{
    public TMP_Text timerText; // Text UI để hiển thị thời gian
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private bool isRunning = true; // Trạng thái đồng hồ

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    

    public void StopTimer()
    {
        isRunning = !isRunning;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}