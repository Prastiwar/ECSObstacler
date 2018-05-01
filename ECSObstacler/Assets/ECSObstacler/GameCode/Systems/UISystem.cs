using System;
using TMPro;
using Unity.Entities;

public class UISystem : ComponentSystem
{
    [Inject] private PlayerUIData data;

    private TextMeshProUGUI healthText;
    private TextMeshProUGUI scoreText;
    private int cachedHealthValue;
    private int cachedScoreValue;

    public void InitializeUI(TextMeshProUGUI healthText, TextMeshProUGUI scoreText)
    {
        this.healthText = healthText;
        this.scoreText = scoreText;
        cachedHealthValue = -1;
        cachedScoreValue = -1;
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; i++)
        {
            UpdateHealthText((int)data.health[i].Value);
            UpdateScoreText(data.scoreHolder[i].Value);
        }
    }

    private void UpdateHealthText(int newValue)
    {
        if (newValue != cachedHealthValue)
        {
            healthText.text = newValue.ToString();
            cachedHealthValue = newValue;
        }
    }

    private void UpdateScoreText(int newValue)
    {
        if (newValue != cachedScoreValue)
        {
            scoreText.text = newValue.ToString();
            cachedScoreValue = newValue;
        }
    }
}
