using SaintsField;
using UnityEngine;

namespace Project.Core.Surfaces.Scripts
{
    public class StickSurfaceMaterial : MonoBehaviour
    {
        [SerializeField] private bool isGravity;
        [SerializeField] private float stickThresholdMin = 0f;
        [SerializeField] private float stickThresholdMax = 1f;
        [SerializeField] private float stickForce = 50f;
        [SerializeField] private float unstickThreshold = 0.5f;
        [SerializeField] private float linearDumping = 0.5f;
        [SerializeField] private float angularDamping = 0.5f;
        
        
    }
}