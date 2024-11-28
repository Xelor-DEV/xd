using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DepartmentBudgetAllocator : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject budgetPanel;
    [SerializeField] private Slider devSlider;
    [SerializeField] private Slider marketingSlider;
    [SerializeField] private Slider supportSlider;
    [SerializeField] private TMP_Text devText;
    [SerializeField] private TMP_Text marketingText;
    [SerializeField] private TMP_Text supportText;
    private float partialBudget;
    [Range(0.0001f, 1f)]
    [SerializeField] private float percentage;

    void Start()
    {
        UpdateSliders();
    }

    void UpdateSliders()
    {
        partialBudget = gameManager.moneyAdminister * percentage;
        devSlider.value = marketingSlider.value = supportSlider.value = 0.33f;
        UpdateDepartmentTexts();
    }

    public void OnSliderValueChanged()
    {
        float totalPercentage = devSlider.value + marketingSlider.value + supportSlider.value;
        devSlider.value /= totalPercentage;
        marketingSlider.value /= totalPercentage;
        supportSlider.value /= totalPercentage;

        UpdateDepartmentTexts();
    }

    private void UpdateDepartmentTexts()
    {
        devText.text = $"Desarrollo: {Mathf.RoundToInt(devSlider.value * partialBudget)}";
        marketingText.text = $"Marketing: {Mathf.RoundToInt(marketingSlider.value * partialBudget)}";
        supportText.text = $"Servicio al Cliente: {Mathf.RoundToInt(supportSlider.value * partialBudget)}";
    }
}
