using System;
using Project.Core.Services;
using Project.Shared.Scripts.Common;
using R3;
using SaintsField;
using UnityEngine;
using Zenject;

namespace Project.Core.UI.Scripts
{
    [RequireComponent(typeof(ValueToText))]
    public class TimerView : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(ValueToText))]
        private ValueToText valueToText;
        
        [Inject] private GameService _gameService;
        
        private void Awake()
        {
            _gameService.CurrentTime.Subscribe(time => valueToText.ToText(TimeSpan.FromSeconds(time)))
                .RegisterTo(destroyCancellationToken);
        }
    }
}