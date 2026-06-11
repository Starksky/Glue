using _Project.Scripts.Contracts.Interfaces;
using SaintsField;
using UnityEngine;
using VContainer;

namespace _Project.Features.UI.MainMenu.Scripts.View
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField, Scene] private string newGameScene;
        
        private IMainMenuPresenter _mainMenuPresenter;

        [Inject]
        public void Construct(IMainMenuPresenter mainMenuPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        public void StartNewGame() => _mainMenuPresenter.StartNewGame(newGameScene);
    }
}