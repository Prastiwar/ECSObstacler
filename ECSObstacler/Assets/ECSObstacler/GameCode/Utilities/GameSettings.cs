using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Hud References")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ScoreText;
    public Button PauseButton;
    public Button MenuButton;
    [Header("Canvas References")]
    public GameObject HUDCanvas;
    public GameObject PauseCanvas;
    public GameObject MenuCanvas;
    public Button PlayButton;
    public Button[] QuitButtons;
    [Header("Settings")]
    public float StartHealth;
    public float PlayerSpeed;
    public float ObstacleSpeed;
    public float SpawnCooldown;
}
