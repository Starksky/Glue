using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class SoftBodyHealth : MonoBehaviour
{
    [Header("Deformation Detection")]
    [Tooltip("Предел растяжения (1.0 = без изменений, 1.5 = растянута на 50%)")]
    public float maxStretchFactor = 1.5f;
    
    [Tooltip("Предел сжатия (0.5 = сжата вдвое)")]
    public float maxShrinkFactor = 0.5f;
    
    [Tooltip("Как часто проверять состояние (сек)")]
    public float checkInterval = 0.2f;
    
    // Хранилище эталонных расстояний между узлами
    private float[] referenceDistances;
    private Transform[] nodes;
    
    // Событие, на которое можно подписаться из другого скрипта
    public UnityEvent EventDeformation;

    private void Start()
    {
        // Находим все дочерние узлы (Rigidbody2D)
        nodes = GetComponentsInChildren<Transform>()
            .Where(t => t != transform && t.GetComponent<Rigidbody2D>() != null)
            .ToArray();
            
        StoreReferenceDistances();
    }

    private void Update()
    {
        if (Time.frameCount % (int)(60 * checkInterval) == 0)
            CheckDeformation();
    }
    
    void StoreReferenceDistances()
    {
        referenceDistances = new float[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            int nextIndex = (i + 1) % nodes.Length;
            referenceDistances[i] = (nodes[i].position - nodes[nextIndex].position).magnitude;
        }
    }

    void CheckDeformation()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            int nextIndex = (i + 1) % nodes.Length;
            float currentDistance = (nodes[i].position - nodes[nextIndex].position).magnitude;
            float reference = referenceDistances[i];
            
            float stretchFactor = currentDistance / reference;
            
            // Проверяем, не вышли ли за пределы
            if (stretchFactor > maxStretchFactor || stretchFactor < maxShrinkFactor)
            {
                Debug.LogWarning($"[SoftBody] Критическая деформация! Узел {nodes[i].name} -> {nodes[nextIndex].name} | Фактор: {stretchFactor:F2}");
                
                // Вызываем событие для внешних подписчиков
                EventDeformation.Invoke();
                break;
            }
        }
    }
}