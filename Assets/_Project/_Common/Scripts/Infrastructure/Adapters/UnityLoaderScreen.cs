using _Project._Common.Scripts.Contracts.Interfaces;
using _Project._Common.Scripts.Infrastructure.Components;
using SaintsField;
using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Adapters
{
    [RequireComponent(typeof(CanvasGroupFader))]
    public class UnityLoaderScreen : MonoBehaviour, ILoaderScreen
    {
        [SerializeField, ReadOnly, GetComponent(typeof(CanvasGroupFader))] 
        private CanvasGroupFader canvasGroupFader;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        public void FadeIn() => canvasGroupFader.FadeIn();
        public void FadeOut() => canvasGroupFader.FadeOut();
    }
}