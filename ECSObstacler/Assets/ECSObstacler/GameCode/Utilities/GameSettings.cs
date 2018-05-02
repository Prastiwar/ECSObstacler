using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Hud References")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ScoreText;
    [Header("Canvas References")]
    public GameObject HUDCanvas;
    public GameObject PauseCanvas;
    public GameObject MenuCanvas;
    public GameObject GameOverCanvas;
    [Header("Button References")]
    public Button PlayButton;
    public Button PauseButton;
    public Button[] QuitButtons;
    public Button[] MenuButtons;
    [Header("Settings")]
    public float StartHealth;
    public float PlayerSpeed;
    public float ObstacleSpeed;
    public float SpawnCooldown;
}
