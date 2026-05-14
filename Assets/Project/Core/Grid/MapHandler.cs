using Project.Core.Services;
using SaintsField;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Grid))]
public class MapHandler : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponent(typeof(Grid))]
    private Grid grid;
    
    [SerializeField] private int besTryCount = 3;
    [SerializeField, RichLabel("Best Time <color=grey>(sec)</color>")] private int bestTime = 10;
    
    [Inject] private GameService _gameService;

    private void Awake()
    {
        _gameService.SetBestTryCount(besTryCount);
        _gameService.SetBestTime(bestTime);
        _gameService.SetMap(grid);
    }
    
    public class Factory : PlaceholderFactory<MapHandler>{}
}
