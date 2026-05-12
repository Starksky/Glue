using System;
using UnityEngine;
using DG.Tweening;
using SaintsField;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponent(typeof(CanvasGroup))] private CanvasGroup canvasGroup;
    
    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private bool disableOnFadeOut = true;
    [SerializeField] private UnityEvent onFadeInComplete;
    [SerializeField] private UnityEvent onFadeOutComplete;
    
    private Tweener _activeTween;

    private void OnDestroy()
    {
        // Останавливаем анимацию, чтобы избежать ошибок
        _activeTween?.Kill();
    }

    /// <summary>
    /// Показать элемент (плавное появление)
    /// </summary>
    public void FadeIn()
    {
        // Останавливаем текущую анимацию
        _activeTween?.Kill();
        
        // Включаем объект и делаем его активным для лучей
        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        
        // Запускаем анимацию появления
        _activeTween = canvasGroup.DOFade(1f, fadeDuration)
            .SetUpdate(true)  // Анимация игнорирует Time.scale
            .OnComplete(() =>
            {
                _activeTween = null;
                onFadeInComplete?.Invoke();
            });
    }

    /// <summary>
    /// Скрыть элемент (плавное исчезновение)
    /// </summary>
    public void FadeOut()
    {
        _activeTween?.Kill();
        
        // Запускаем анимацию исчезновения
        _activeTween = canvasGroup.DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // Главное: отключаем блокировку лучей!
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                
                if (disableOnFadeOut)
                    gameObject.SetActive(false);
                
                _activeTween = null;
                onFadeOutComplete?.Invoke();
            });
    }

    /// <summary>
    /// Мгновенная установка прозрачности (без анимации)
    /// </summary>
    public void SetAlphaImmediate(float alpha)
    {
        _activeTween?.Kill();
        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = alpha > 0;
        canvasGroup.interactable = alpha > 0;
        gameObject.SetActive(alpha > 0 || !disableOnFadeOut);
    }

    /// <summary>
    /// Переключение (если виден - скрыть, если скрыт - показать)
    /// </summary>
    public void Toggle()
    {
        if (canvasGroup.alpha > 0.1f)
            FadeOut();
        else
            FadeIn();
    }
}