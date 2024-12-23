using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Button easyButton;      // Nút chọn độ dễ
    public Button mediumButton;    // Nút chọn độ trung bình
    public Button hardButton;      // Nút chọn độ khó

    private void Start()
    {
        // Gán sự kiện nhấn nút
        easyButton.onClick.AddListener(() => SelectDifficulty(Generator.DifficultyLevel.EASY));
        mediumButton.onClick.AddListener(() => SelectDifficulty(Generator.DifficultyLevel.MEDIUM));
        hardButton.onClick.AddListener(() => SelectDifficulty(Generator.DifficultyLevel.DIFFICULT));
    }

    private void SelectDifficulty(Generator.DifficultyLevel difficultyLevel)
    {
        // Lưu độ khó được chọn vào PlayerPrefs
        PlayerPrefs.SetInt("SelectedDifficulty", (int)difficultyLevel);

        // Chuyển sang scene game
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}


