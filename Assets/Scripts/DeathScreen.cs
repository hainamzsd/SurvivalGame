using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreenPanel;
    public float fadeDuration = 1.0f;

    private CanvasRenderer canvasRenderer;
    private bool isFading = false;

    public GameSystem gameSystem;
    public Text killCountText;
    int killCount;

    public Button restart;

    private void Start()
    {
        killCount = 0;
        deathScreenPanel.SetActive(false);
        canvasRenderer = deathScreenPanel.GetComponent<CanvasRenderer>();
    }

    private void Update()
    {
        killCount = gameSystem.killCount;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FadeToBlack()
    {
        if (isFading) return;

        isFading = true;
        deathScreenPanel.SetActive(true);
        killCountText.text = "Kills: " + killCount.ToString();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float elapsedTime = 0.0f;
        float startAlpha = 0f;
        float targetAlpha = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetPanelAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        SetPanelAlpha(targetAlpha);
    }

    private void SetPanelAlpha(float alpha)
    {
        // Set the alpha value of the panel's color through its CanvasRenderer component
        canvasRenderer.SetAlpha(alpha);
    }
}
