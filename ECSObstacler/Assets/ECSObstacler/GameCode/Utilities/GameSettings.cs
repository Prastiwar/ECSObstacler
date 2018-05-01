using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ScoreText;
    public Button PauseButton;
    [Header("Settings")]
    public float StartHealth;
    public float PlayerSpeed;
    public float ObstacleSpeed;
    public float SpawnCooldown;
}
