using System;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class SoftBodyRenderer : MonoBehaviour
{
    [SerializeField] private BonePhysics[] bones;
    private SpriteShapeController _shapeController;

    private void Awake()
    {
        _shapeController = GetComponent<SpriteShapeController>();
    }
    private void Start()
    {
        UpdatePoints(true);
    }
    private void LateUpdate()
    {
        UpdatePoints(true);
    }
    private void UpdatePoints(bool withTangent = false)
    {
        int index = 0;
        foreach (var bone in bones)
        {
            try
            {
                _shapeController.spline.SetPosition(index, bone.GetTowardLocalPoint());
            }
            catch (Exception e)
            {
                _shapeController.spline.SetPosition(index, bone.GetTowardLocalPoint() + Vector2.one);
            }
            
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
}
