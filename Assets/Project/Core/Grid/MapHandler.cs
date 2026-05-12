using Project.Core.Services;
using SaintsField;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Grid))]
public class MapHandler : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponent(typeof(Grid))]
    private Grid grid;
    [Inject] private GameService _gameService;

    private void Awake()
    {
        _gameService.SetMap(grid);
    }
    
    public class Factory : PlaceholderFactory<MapHandler>{}
}
