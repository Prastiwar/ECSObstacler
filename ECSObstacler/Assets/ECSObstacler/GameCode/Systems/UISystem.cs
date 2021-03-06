﻿using Unity.Entities;

[AlwaysUpdateSystem]
public class UISystem : ComponentSystem
{
    [Inject] private PlayerUIData data;
    private int cachedHealthValue = -1;
    private int cachedScoreValue = -1;

    protected override void OnUpdate()
    {
        for (int i = 0; i < data.Length; i++)
        {
            UpdateHealthText((int)data.health[i].Value);
            UpdateScoreText(data.scoreHolder[i].Value);
        }
        if (data.Length < 1)
        {
            UpdateHealthText(0);
            ECSObstaclerBootstrap.GameOver();
        }
    }

    private void UpdateHealthText(int newValue)
    {
        if (newValue != cachedHealthValue)
        {
            ECSObstaclerBootstrap.GameSettings.HealthText.text = newValue.ToString();
            cachedHealthValue = newValue;
        }
    }

    private void UpdateScoreText(int newValue)
    {
        if (newValue != cachedScoreValue)
        {
            ECSObstaclerBootstrap.GameSettings.ScoreText.text = newValue.ToString();
            cachedScoreValue = newValue;
        }
    }
}
