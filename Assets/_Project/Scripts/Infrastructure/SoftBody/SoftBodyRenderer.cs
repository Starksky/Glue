using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

namespace _Project.Scripts.Infrastructure.SoftBody
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class SoftBodyRenderer : MonoBehaviour
    {
        [SerializeField] private BonePhysics[] bones;
        [SerializeField] private UnityEvent EventFix;
        private SpriteShapeController _shapeController;
    
        private Vector3[] _defaultPositions;
    
        private void Awake()
        {
            _shapeController = GetComponent<SpriteShapeController>();

        }
        private void Start()
        {
            _defaultPositions = new Vector3[bones.Length];
            int index = 0;
            foreach (var bone in bones)
                _defaultPositions[index] = bone.GetTowardLocalPoint();
        
            UpdatePoints(true);
        }
        private void LateUpdate()
        {
            UpdatePoints(true);
        }
        private void UpdatePoints(bool withTangent = false)
        {
            int index = 0;

            try
            {
                foreach (var bone in bones)
                {
                    _shapeController.spline.SetPosition(index, bone.GetTowardLocalPoint());
                    if (withTangent)
                    {
                        var lt = _shapeController.spline.GetLeftTangent(index);
                        var newRt = Vector2.Perpendicular(bone.GetTowardCenter()) * lt.magnitude;
                        var newLt = Vector2.zero - newRt;
                        _shapeController.spline.SetLeftTangent(index, newLt);
                        _shapeController.spline.SetRightTangent(index, newRt);
                    }
                    index++;
                }
            }
            catch (Exception e)
            {
                var bone = bones[index];
                _shapeController.spline.SetPosition(index, _defaultPositions[index]);
                var lt = _shapeController.spline.GetLeftTangent(index);
                var newRt = Vector2.Perpendicular(bone.GetTowardCenter()) * lt.magnitude;
                var newLt = Vector2.zero - newRt;
                _shapeController.spline.SetLeftTangent(index, newLt);
                _shapeController.spline.SetRightTangent(index, newRt);
                EventFix.Invoke();
            }
        }
    }
}
