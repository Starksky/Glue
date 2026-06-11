using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Repositories.Player
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