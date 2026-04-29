using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    public static bool IsPaused = false;
    private bool isPaused = false;
    [SerializeField] private GameObject firstSelectedButton; // Kéo button đầu tiên vào đây trong Inspector

    void Start()
    {
        ResumeGameState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Continue();
        else
            Pause();
    }

    public void Pause()
    {
        if (isPaused) return; // Đã pause thì không làm gì
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Chọn button đầu tiên để UI highlight đúng
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void Continue()
    {
        if (!isPaused) return; // Đã unpause thì không làm gì
        ResumeGameState();
    }

    private void ResumeGameState()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Bỏ chọn button khi tắt menu
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void Home()
    {
        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Quan trọng: trả lại tốc độ thời gian bình thường
        isPaused = false;
        IsPaused = false;
        SceneManager.LoadSceneAsync(0);
    }

}
