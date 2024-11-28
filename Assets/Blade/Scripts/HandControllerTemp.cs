using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerTemp : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float handSpeed = 0.1f;
    [SerializeField] private Vector2 moveLimits = new Vector2(1f, 1f);

    [SerializeField] private GameObject report;
    [SerializeField] private GameObject decisiones;

    // añadido
    [SerializeField] private GameObject budgetPanel;
    private Vector3 originalBudgetPosition;
    //

    private Vector3 originalReportPosition;
    private Vector3 originalDecisionesPosition;

    private Vector3 startPosition;
    private bool isXYMovement = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        startPosition = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (report != null)
        {
            originalReportPosition = report.transform.position;
        }
        if (decisiones != null)
        {
            originalDecisionesPosition = decisiones.transform.position;
        }
        // añadido
        if (budgetPanel != null)
        {
            originalBudgetPosition = budgetPanel.transform.position;
        }
        //
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * handSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * handSpeed;

        Vector3 newPosition = transform.position;

        if (isXYMovement)
        {
            newPosition.x += mouseX;
            newPosition.y += mouseY;

            newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - moveLimits.x, startPosition.x + moveLimits.x);
            newPosition.y = Mathf.Clamp(newPosition.y, startPosition.y - moveLimits.y, startPosition.y + moveLimits.y);
        }
        else
        {
            newPosition.x += mouseX;
            newPosition.z += mouseY;

            newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - moveLimits.x, startPosition.x + moveLimits.x);
            newPosition.z = Mathf.Clamp(newPosition.z, startPosition.z - moveLimits.y, startPosition.z + moveLimits.y);
        }

        transform.position = newPosition;

        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10f, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.collider.CompareTag("Handle"))
                {
                    Debug.Log("¡Handle detectado!");
                    hit.collider.GetComponent<PhoneController>().OnMouseDown();
                }
                else if (hit.collider.CompareTag("Report"))
                {
                    ToggleReport();
                }
                else if (hit.collider.CompareTag("Gachas"))
                {
                    ToggleDecisiones();
                }
                //añadido
                else if (hit.collider.CompareTag("Budget"))
                {
                    ToggleBudgetPanel();
                }
                //
            }
        }
    }
    private void ToggleReport()
    {
        if (report != null)
        {
            if (report.transform.position == originalReportPosition)
            {
                StartCoroutine(MoveReport(mainCamera.transform.position + mainCamera.transform.forward * 2f, new Vector3(100f, 100f, 100f), 0.3f));
            }
            else
            {
                StartCoroutine(MoveReport(originalReportPosition, new Vector3(57f, 57f, 57f), 0.3f));
            }
        }
    }

    private void ToggleDecisiones()
    {
        if (decisiones != null)
        {
            if (decisiones.transform.position == originalDecisionesPosition)
            {
                StartCoroutine(MoveDecisiones(mainCamera.transform.position + mainCamera.transform.forward * 2f, new Vector3(100f, 100f, 100f), 0.3f));
                isXYMovement = true;
                transform.position = new Vector3(-0.16f, 0.3f, -9.07f);
            }
            else
            {
                StartCoroutine(MoveDecisiones(originalDecisionesPosition, new Vector3(65f, 65f, 65f), 0.3f));
                transform.position = new Vector3(0f, 0f, -7f);
                isXYMovement = false;
            }
        }
    }

    //añadido
    private void ToggleBudgetPanel()
    {
        if (budgetPanel != null)
        {
            Vector3 targetPosition = budgetPanel.transform.position == originalBudgetPosition
                ? mainCamera.transform.position + mainCamera.transform.forward * 2f
                : originalBudgetPosition;

            StartCoroutine(MoveBudgetPanel(targetPosition, new Vector3(100f, 100f, 100f), 0.3f));
        }
    }
    //

    private IEnumerator MoveReport(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        Vector3 startPosition = report.transform.position;
        Vector3 startScale = report.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            report.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            report.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        report.transform.position = targetPosition;
        report.transform.localScale = targetScale;
    }

    private IEnumerator MoveDecisiones(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        Vector3 startPosition = decisiones.transform.position;
        Vector3 startScale = decisiones.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            decisiones.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            decisiones.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        decisiones.transform.position = targetPosition;
        decisiones.transform.localScale = targetScale;
    }

    //añadido
    private IEnumerator MoveBudgetPanel(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        Vector3 startPosition = budgetPanel.transform.position;
        Vector3 startScale = budgetPanel.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            budgetPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            budgetPanel.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        budgetPanel.transform.position = targetPosition;
        budgetPanel.transform.localScale = targetScale;
    }
    //
}




