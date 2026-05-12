using UnityEngine;

namespace Project.Shared.Scripts.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Absolute(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }
    }
}