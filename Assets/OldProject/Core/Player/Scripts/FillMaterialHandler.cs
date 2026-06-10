using SaintsField;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeRenderer))]
public class FillMaterialHandler : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponent(typeof(SpriteShapeRenderer))]
    private SpriteShapeRenderer shapeRenderer;

    [SerializeField] private float speed;
    
    private Material _material;
    private float _eulerAngleZ;
    private void Awake()
    {
        _material = shapeRenderer.materials[0];
    }

    private void Update()
    {
        _eulerAngleZ = Mathf.Lerp(_eulerAngleZ, transform.rotation.eulerAngles.z, speed * Time.deltaTime);
        _material.SetFloat("_RotateZ", _eulerAngleZ);
    }
}
