using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class MarketPanel : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private RectTransform marketPanel;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float slideDistance = 300f;
    [SerializeField] private AnimationCurve showCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve hideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Optional")]
    [SerializeField] private Image overlay;
    [SerializeField] private float overlayAlpha = 0.5f;

    private Vector2 originalPosition;
    private Vector2 hiddenPosition;
    private bool isPopupShown = false;
    private Coroutine currentAnimation;

    private void Awake()
    {
        // Проверяем, что панель назначена
        if (marketPanel == null)
        {
            Debug.LogError("Popup Panel is not assigned!", this);
            return;
        }

        // Получаем оригинальную позицию панели
        originalPosition = marketPanel.anchoredPosition;
        originalPosition.y = 0;
        originalPosition.x = 0;

        // Вычисляем скрытую позицию (снизу за экраном)
        hiddenPosition = originalPosition - new Vector2(0, slideDistance);

        // Устанавливаем начальное положение (скрытое)
        marketPanel.anchoredPosition = hiddenPosition;
        marketPanel.gameObject.SetActive(false);

        // Настраиваем оверлей
        if (overlay != null)
        {
            overlay.color = new Color(0, 0, 0, 0);
            overlay.raycastTarget = false;
            overlay.gameObject.SetActive(false);
        }

        // Добавляем обработчик клика на кнопку
        GetComponent<Button>().onClick.AddListener(TogglePopup);
    }

    public void TogglePopup()
    {
        if (isPopupShown)
        {
            HidePopup();
        }
        else
        {
            ShowPopup();
        }
    }

    public void ShowPopup()
    {
        if (isPopupShown) return;

        // Останавливаем текущую анимацию, если есть
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Активируем панель
        marketPanel.gameObject.SetActive(true);

        // Активируем оверлей
        if (overlay != null)
        {
            overlay.gameObject.SetActive(true);
            overlay.raycastTarget = true;
        }

        // Запускаем анимацию
        currentAnimation = StartCoroutine(AnimatePopup(
            startPos: hiddenPosition,
            endPos: originalPosition,
            curve: showCurve,
            onComplete: () => {
                isPopupShown = true;
                currentAnimation = null;
            },
            overlayAlpha: overlayAlpha
        ));
    }

    public void HidePopup()
    {
        if (!isPopupShown) return;

        // Останавливаем текущую анимацию, если есть
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Запускаем анимацию
        currentAnimation = StartCoroutine(AnimatePopup(
            startPos: originalPosition,
            endPos: hiddenPosition,
            curve: hideCurve,
            onComplete: () => {
                marketPanel.gameObject.SetActive(false);
                isPopupShown = false;
                currentAnimation = null;

                // Деактивируем оверлей
                if (overlay != null)
                {
                    overlay.gameObject.SetActive(false);
                    overlay.raycastTarget = false;
                }
            },
            overlayAlpha: 0f
        ));
    }

    private IEnumerator AnimatePopup(Vector2 startPos, Vector2 endPos, AnimationCurve curve, System.Action onComplete, float overlayAlpha)
    {
        float elapsed = 0f;

        // Анимация перемещения
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            float curveValue = curve.Evaluate(t);

            // Плавное перемещение
            marketPanel.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, curveValue);

            // Плавное изменение прозрачности оверлея
            if (overlay != null)
            {
                float currentAlpha = Mathf.Lerp(overlay.color.a, overlayAlpha, curveValue);
                overlay.color = new Color(0, 0, 0, currentAlpha);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что достигли конечной позиции
        marketPanel.anchoredPosition = endPos;

        // Убедимся, что оверлей достиг нужной прозрачности
        if (overlay != null)
        {
            overlay.color = new Color(0, 0, 0, overlayAlpha);
        }

        onComplete?.Invoke();
    }

    // Метод для закрытия попапа при клике на оверлей
    public void OnOverlayClick()
    {
        HidePopup();
    }
}