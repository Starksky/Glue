using Project.Core.Services;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Core.UI.Scripts
{
    public class StarsView : MonoBehaviour
    {
        [SerializeField] private Toggle[] stars;
        
        [Inject] private GameService _gameService;

        private void Awake()
        {
            _gameService.CurrentStars.Subscribe(count =>
            {
                for(int i = 0; i < stars.Length; i++)
                    stars[i].isOn = i < count;
            }).RegisterTo(destroyCancellationToken);
        }
    }
}