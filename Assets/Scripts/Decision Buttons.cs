using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DecisionButtons : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Decision addGachasDecision;
    [SerializeField] private Decision addAdsDecision;
    [SerializeField] private Decision raisePriceDecision;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject borderSelection;
    [SerializeField] private GameObject popupGachas;
    [SerializeField] private GameObject popupAds;  
    [SerializeField] private GameObject popupPrice;

    [SerializeField] private List<GameObject> particlePrefabs; // Lista de prefabs de partículas para cada decisión
    [SerializeField] private List<AudioClip> decisionSounds; // Lista de sonidos para cada decisión
    [SerializeField] private float particleLifetime = 6f; // Tiempo de vida de las partículas
    [SerializeField] private GameObject particleSpawnPosition; // Posiciones específicas para cada decisión
    [SerializeField] private AudioSource audioSource; // Componente AudioSource para reproducir los sonidos


    private int selectedIndex = 0;
    private bool canHandleInput = false;
    private bool hasSelected = false; 

    private void Update()
    {
        if (canHandleInput && !hasSelected)
        {
            HandleArrowInput();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteSelectedButtonAction();
                hasSelected = true;
            }
        }
        else if (!canHandleInput && hasSelected) 
        {
            ResetSelection(); 
        }
    }

    private void HandleArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + buttons.Count) % buttons.Count;
            MoveBorderToSelectedButton();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % buttons.Count;
            MoveBorderToSelectedButton();
        }
    }

    private void MoveBorderToSelectedButton()
    {
        borderSelection.transform.position = buttons[selectedIndex].transform.position;
    }

    public void SetCanHandleInput(bool value)
    {
        canHandleInput = value;
    }

    public void ResetSelection() 
    {
        hasSelected = false;
    }

    private void ExecuteSelectedButtonAction()
    {
        switch (selectedIndex)
        {
            case 0:
                OnAddGachasClicked();
                ShowGachasPopup();
                SpawnParticlesAndSound(0);
                break;
            case 1:
                OnAddAdsClicked();
                ShowAdsPopup();
                SpawnParticlesAndSound(1);
                break;
            case 2:
                OnRaisePriceClicked();
                ShowPricePopup();
                SpawnParticlesAndSound(2);
                break;
            default:
                Debug.LogWarning("Índice de botón no válido.");
                break;
        }
    }

    public void OnAddGachasClicked()
    {
        gameManager.TakeDecision(addGachasDecision);
    }

    public void OnAddAdsClicked()
    {
        gameManager.TakeDecision(addAdsDecision);
    }

    public void OnRaisePriceClicked()
    {
        gameManager.TakeDecision(raisePriceDecision);
    }

    private void SpawnParticlesAndSound(int decisionIndex)
    {
        if (decisionIndex < 0 || decisionIndex >= particlePrefabs.Count)
        {
            Debug.LogWarning("Índice de decisión no válido para partículas o posiciones.");
            return;
        }

        // Instanciar el sistema de partículas en la posición específica
        GameObject particleInstance = Instantiate(
            particlePrefabs[decisionIndex],
            particleSpawnPosition.transform.position, particlePrefabs[decisionIndex].transform.rotation
        );

        // Reproducir el sonido correspondiente
        if (decisionIndex < decisionSounds.Count && decisionSounds[decisionIndex] != null)
        {
            audioSource.PlayOneShot(decisionSounds[decisionIndex]);
        }

        // Destruir las partículas después de un tiempo
        StartCoroutine(DestroyParticlesAfterTime(particleInstance, particleLifetime));
    }

    private IEnumerator DestroyParticlesAfterTime(GameObject particleInstance, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (particleInstance != null)
        {
            Destroy(particleInstance);
        }
    }

    private void ShowGachasPopup()
    {
        popupGachas.SetActive(true);
        popupGachas.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        popupGachas.transform.localPosition = new Vector3(0, -50, 0);

        popupGachas.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                popupGachas.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    popupGachas.SetActive(false);
                });
            });
        });
    }
    private void ShowAdsPopup()
    {
        popupAds.SetActive(true);
        popupAds.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        popupAds.transform.localPosition = new Vector3(-Screen.width, -50, 0); 

        popupAds.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            popupAds.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    popupAds.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        popupAds.SetActive(false);
                    });
                });
            });
        });
    }



    private void ShowPricePopup()
    {
        popupPrice.SetActive(true);
        popupPrice.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        popupPrice.transform.localPosition = new Vector3(Screen.width, -50, 0);

        popupPrice.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            popupPrice.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    popupPrice.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        popupPrice.SetActive(false);
                    });
                });
            });
        });
    }
}

