using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int month = 1;
    public int moneyAdminister;
    public float userSatisfaction;
    public TMP_Text reportPanelText;
    public TMP_Text reportSatisfactionFinalText;
    public TMP_Text moneyText;
    public TMP_Text monthText;
    public Image finalImageMonth;
    public GameObject gameOverPanel; // Panel para mostrar derrota

    private int decisionCount = 0;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Oculta el panel de derrota
        Time.timeScale = 1;
        ResetGame();
        ShowInitialMonthText();
        Debug.Log("Dinero inicial: " + moneyAdminister);
    }

    private void Update()
    {
        // Detecta la tecla 'R' solo si el panel de Game Over está activo
        if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reiniciando juego...");
            ReloadScene();
        }

        // Detecta la tecla 'Esc' para salir del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit(); // Cierra el juego
        }

        // Detecta la tecla 'Enter' para ejecutar otra acción (puedes personalizar la acción aquí)
        if (Input.GetKeyDown(KeyCode.Return)) // También puede ser KeyCode.KeypadEnter para teclados numéricos
        {
            Debug.Log("Acción con Enter ejecutada.");
            SceneManager.LoadScene("Menu");
        }
    }

    private void ShowInitialMonthText()
    {
        monthText.text = "Mes " + month;
        monthText.gameObject.SetActive(true);
        StartCoroutine(FadeOutInitialMonthText());
    }

    private IEnumerator FadeOutInitialMonthText()
    {
        Color textColor = monthText.color;
        textColor.a = 1;
        monthText.color = textColor;

        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            textColor.a = t;
            monthText.color = textColor;
            yield return null;
        }
        monthText.gameObject.SetActive(false);
    }

    public void TakeDecision(Decision decision)
    {
        moneyAdminister -= decision.cost;
        moneyAdminister += decision.gain;

        if (moneyAdminister < 0)
        {
            moneyAdminister = 0;
        }

        userSatisfaction += decision.satisfactionImpact;

        // Validación para que no se pase de 100%
        if (userSatisfaction > 100)
        {
            userSatisfaction = 100;
        }
        else if (userSatisfaction < 0) // Asegura que no sea menor a 0
        {
            userSatisfaction = 0;
        }

        UpdateMoneyText();
        UpdateReportPanel();

        CheckGameOver();

        decisionCount++;

        if (decisionCount >= 3)
        {
            ShowEndReport();
            decisionCount = 0;
        }
    }

    private void CheckGameOver()
    {
        if (moneyAdminister <= 0 || userSatisfaction <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        gameOverPanel.SetActive(true); // Muestra el panel de derrota
        Time.timeScale = 0; // Pausa el juego
        Debug.Log("Juego terminado. Has perdido.");
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowEndReport()
    {
        EndHalfYearReport();
        Invoke("ShowMonthText", 2f);
        Invoke("AdvanceMonth", 2f);
    }

    public void AdvanceMonth()
    {
        month++;
        Debug.Log("Mes: " + month);

        ResetMonthValues();

        ShowMonthText();
    }

    private void EndMonth()
    {
        reportPanelText.text = "";
        reportSatisfactionFinalText.text = "";
        UpdateReportPanel();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "DINERO: $" + moneyAdminister;
    }

    private void UpdateReportPanel()
    {
        reportPanelText.text = "Mes: " + month + "\n\n\nDinero: " + moneyAdminister + "\n\n\nSatisfacción: " + userSatisfaction + "%";
    }

    private void UpdateMonthText()
    {
        monthText.text = "Mes " + month;
        monthText.gameObject.SetActive(true);
    }

    private void EndHalfYearReport()
    {
        if (userSatisfaction < 40)
        {
            reportSatisfactionFinalText.text = "Te has ido a la quiebra";
        }
        else if (userSatisfaction < 60)
        {
            reportSatisfactionFinalText.text = "Lograste mantenerte, pero la satisfacción del usuario es baja.";
        }
        else
        {
            reportSatisfactionFinalText.text = "Mantuviste a los usuarios satisfechos.";
        }
    }

    public void ResetGame()
    {


        UpdateMoneyText();
        UpdateReportPanel();
        UpdateMonthText();
        Debug.Log("Juego reiniciado.");
    }

    private void ResetMonthValues()
    {
        UpdateMoneyText();
        UpdateReportPanel();
    }

    public void ShowMonthText()
    {
        StartCoroutine(ShowMonthWithFade());
    }

    private IEnumerator ShowMonthWithFade()
    {
        finalImageMonth.gameObject.SetActive(true);
        monthText.text = "Mes " + month;
        monthText.gameObject.SetActive(true);

        Color imageColor = finalImageMonth.color;
        imageColor.a = 0;
        finalImageMonth.color = imageColor;

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            imageColor.a = t;
            finalImageMonth.color = imageColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Color textColor = monthText.color;
        textColor.a = 0;
        monthText.color = textColor;

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            textColor.a = t;
            monthText.color = textColor;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            textColor.a = t;
            monthText.color = textColor;

            imageColor.a = t;
            finalImageMonth.color = imageColor;

            yield return null;
        }

        monthText.gameObject.SetActive(false);
        finalImageMonth.gameObject.SetActive(false);
    }
}
