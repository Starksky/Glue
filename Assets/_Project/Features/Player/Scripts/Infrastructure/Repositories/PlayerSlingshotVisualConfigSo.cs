using _Project.Features.Player.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Player.Scripts.Infrastructure.Repositories
{
    [CreateAssetMenu(fileName = "SlingshotVisualConfig", menuName = "Create/SlingshotVisualConfig", order = 0)]
    public class PlayerSlingshotVisualConfigSo : ScriptableObject, IPlayerSlingshotVisualConfig
    {
        [SerializeField] private float minWidth = 0.4f;
        [SerializeField] private float directHeight = 2f;
        
        public float MinWidth => minWidth;
        public float DirectHeight => directHeight;
    }
}