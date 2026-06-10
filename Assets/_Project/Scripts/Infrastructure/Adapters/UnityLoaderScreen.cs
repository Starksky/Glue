using _Project.Scripts.Contracts.Interfaces;
using _Project.Scripts.View.UI;
using SaintsField;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Adapters
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