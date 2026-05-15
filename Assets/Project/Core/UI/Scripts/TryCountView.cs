using Project.Core.Services;
using Project.Shared.Scripts.Common;
using R3;
using SaintsField;
using UnityEngine;
using Zenject;

namespace Project.Core.UI.Scripts
{
    [RequireComponent(typeof(ValueToText))]
    public class TryCountView : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(ValueToText))]
        private ValueToText valueToText;
        
        [Inject] private GameService _gameService;

        private void Awake()
        {
            _gameService.CurrentTryCount.Subscribe(valueToText.ToText).RegisterTo(destroyCancellationToken);
        }
    }
}