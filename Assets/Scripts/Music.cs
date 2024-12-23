using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    private static Music instance;
    private AudioSource audioSource;

    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không phá hủy đối tượng này khi load scene mới
        }
        else
        {
            Destroy(gameObject); // Xóa AudioManager mới nếu đã có một cái tồn tại
        }
    }
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }
}
