using UnityEngine;

namespace _Project._Common.Scripts.Utils
{
    public static class Vector2Extensions
    {
        public static Vector2 Absolute(this Vector2 vector)
        {
            var normalized = vector.normalized;
            return new Vector2(Mathf.Abs(normalized.x), Mathf.Abs(normalized.y));
        }
        public static Vector2 InvertAbsolute(this Vector2 vector)
        {
            var abs = vector.Absolute();
            return new Vector2(Mathf.Abs(1 - abs.x), Mathf.Abs(1 - abs.y));
        }
    }
}