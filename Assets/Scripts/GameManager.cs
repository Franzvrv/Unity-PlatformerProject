using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject cameraObject, scoreTextObject, playerObject, pausePanel, gameOverPanel;
    public GameObject dangerPanel, mapDestroyer;
    private bool paused = false;
    private bool gameOver = false;
    public bool Paused { get => paused; }
    private TMP_Text scoreText;
    private Vector2 scoreTextScale;
    private float score = 0;
    

    void Awake() {
        if (!Instance) {
            Instance = this;
        }
        scoreText = scoreTextObject.GetComponent<TMP_Text>();
        scoreTextScale = scoreTextObject.transform.localScale;
    }

    void Start() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 0:
                AudioListener.pause = true;
                break;
            case 1:
                StartGame();
                break;
        }        
    }

    void StartGame() {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    void Update()
    {
        GetPlayerTransform();
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void Pause() {
        if (!gameOver) {
            if (!paused) {
                paused = true;
                AudioListener.pause = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            } else {
                paused = false;
                AudioListener.pause = false;
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public GameObject GetPlayerObject() {
        return playerObject;
    }

    public Transform GetPlayerTransform() {
        if (playerObject != null) {
            return playerObject.GetComponent<Transform>();
        }
        else {
            return null;
        }
    }

    public Transform GetCameraTransform() {
        return cameraObject.transform;
    }

    public void CameraPanChange(float amount) {
        Spawner.Instance.cooldown(amount);
    }

    public void ChangeScore(float amount, float panIntensity) {
        if(amount > panIntensity) {
            scoreTextObject.transform.localScale = new Vector2(scoreTextScale.x + amount / 50, scoreTextScale.y + amount / 50);
        } else {
            scoreTextObject.transform.localScale = scoreTextScale;
        }
        score += amount;
        scoreText.text = "Score: " + Mathf.RoundToInt(score);
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void GameOver() {
        gameOver = true;
        Animator cameraAnimator = cameraObject.GetComponent<Animator>();
        cameraAnimator.SetBool("GameOver", true);
        Time.timeScale = 0;
        AudioListener.pause = true;
        SoundManager.Instance.PlayUntimedSound("GameOverBass");
        StartCoroutine(GameOverCoroutine(cameraAnimator));
    }

    IEnumerator GameOverCoroutine(Animator cameraAnimator) {
        yield return null;
        gameOverPanel.SetActive(true);
        while (!cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Camera Normal")) {
            yield return null;
        }
        SoundManager.Instance.PlayUntimedSound("GameOverMusic");
        yield break;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
