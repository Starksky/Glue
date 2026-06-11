using _Project.Scripts.Contracts.Interfaces;
using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Repositories.Player
{
    [CreateAssetMenu(fileName = "SlingshotConfig", menuName = "Create/SlingshotConfig", order = 0)]
    public class PlayerSlingshotConfigSo : ScriptableObject, IPlayerSlingshotConfig
    {
        [SerializeField] private float maxDragDistanceBody = 0.25f;
        [SerializeField] private float maxDragDistance = 1.8f;
        [SerializeField] private float maxForce = 8f;
        [SerializeField] private float angleForForce = 45f;
        [SerializeField] private float dragForce = 10f;
        
        public float MaxDragDistanceBody => maxDragDistanceBody;
        public float MaxDragDistance => maxDragDistance;
        public float MaxForce => maxForce;
        public float AngleForForce => angleForForce;
        public float DragForce => dragForce;
    }
}