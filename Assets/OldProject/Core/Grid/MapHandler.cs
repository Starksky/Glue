using Project.Core.Services;
using SaintsField;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Grid))]
public class MapHandler : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponent(typeof(Grid))]
    private Grid grid;
    
    [SerializeField] private int countTryForFirstPlace = 3;
    [SerializeField] private int countTryForSecondPlace = 6;
    [SerializeField, RichLabel("Timer <color=grey>(sec)</color>")] private int timer = 10;
    
    [Inject] private GameService _gameService;
    
    public int CountTryForFirstPlace => countTryForFirstPlace;
    public int CountTryForSecondPlace => countTryForSecondPlace;
    public int Timer => timer;
    public Grid Grid => grid;   

    private void Awake()
    {
        _gameService.SetMap(this);
    }
}
