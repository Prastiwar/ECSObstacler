using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("In Game References")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ScoreText;
    public Button PauseButton;
    [Header("In Menu References")]
    public Button PlayButton;
    [Header("Settings")]
    public float StartHealth;
    public float PlayerSpeed;
    public float ObstacleSpeed;
    public float SpawnCooldown;
}
