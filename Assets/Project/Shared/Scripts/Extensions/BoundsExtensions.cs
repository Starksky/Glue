using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.Shared.Scripts.Extensions
{
    public static class BoundsExtensions
    {
        public static Vector3 ClosestBoundInBounds(this Bounds self, Bounds bounds, Quaternion? rotation = null, Vector3? offset = null)
        {
            rotation ??= Quaternion.identity;
            offset ??= Vector3.zero;
            
            var center = bounds.center + offset.Value;
            center.z = 0;
            
            var p1 = (Vector3)(Vector2)(center + rotation * (center - new Vector3(bounds.min.x, bounds.min.y, 0f)));
            var p2 = (Vector3)(Vector2)(center + rotation * (center - new Vector3(bounds.min.x, bounds.max.y, 0f)));
            var p3 = (Vector3)(Vector2)(center + rotation * (center - new Vector3(bounds.max.x, bounds.max.y, 0f)));
            var p4 = (Vector3)(Vector2)(center + rotation * (center - new Vector3(bounds.max.x, bounds.min.y, 0f)));
            
            
            var b1 = PointInBounds(self, p1, rotation);
            var b2 = PointInBounds(self, p2, rotation);
            var b3 = PointInBounds(self, p3, rotation);
            var b4 = PointInBounds(self, p4, rotation);
            
            if (b1 && b2 && b3 && b4)
                return bounds.center;
            
            var c1 = ClosestPointInBounds(self, p1, rotation);
            var c2 = ClosestPointInBounds(self, p2, rotation);
            var c3 = ClosestPointInBounds(self, p3, rotation);
            var c4 = ClosestPointInBounds(self, p4, rotation);
            
            var d1 = Vector3.Distance(p1, c1);
            var d2 = Vector3.Distance(p2, c2);
            var d3 = Vector3.Distance(p3, c3);
            var d4 = Vector3.Distance(p4, c4);
            
            using (ListPool<(float, (Vector3, Vector3))>.Get(out var pool))
            {
                pool.Add((d1, (p1, c1)));
                pool.Add((d2, (p2, c2)));
                pool.Add((d3, (p3, c3)));
                pool.Add((d4, (p4, c4)));
                
                var find = pool.OrderByDescending(i => i.Item1).First();
                if (find.Item1 > 0f)
                    return find.Item2.Item2 + (bounds.center - find.Item2.Item1);
            }
           
            return bounds.center;
        }

        public static bool PointInBounds(this Bounds self, Vector3 point, Quaternion? rotation = null, Vector3? offset = null)
        {
            rotation ??= Quaternion.identity;
            offset ??= Vector3.zero;
            
            var center = self.center + offset.Value;
            center.z = 0f;
            
            var a1 = (Vector2)(center + rotation * (center - new Vector3(self.min.x, self.min.y, 0f)));
            var b1 = (Vector2)(center + rotation * (center - new Vector3(self.min.x, self.max.y, 0f)));
            var c1 = (Vector2)(center + rotation * (center - new Vector3(self.max.x, self.max.y, 0f)));

            var a2 = a1;
            var b2 = c1;
            var c2 = (Vector2)(center + rotation * (center - new Vector3(self.max.x, self.min.y, 0f)));

            return PointInTriangle(point, a1, b1, c1) ||
                   PointInTriangle(point, a2, b2, c2);
        }
        
        public static Vector3 ClosestPointInBounds(this Bounds self, Vector3 point, Quaternion? rotation = null, Vector3? offset = null)
        {
            rotation ??= Quaternion.identity;
            offset ??= Vector3.zero;
            
            var center = self.center + offset.Value;
            center.z = 0f;

            var a1 = (Vector2)(center + rotation * (center - new Vector3(self.min.x, self.min.y, 0f)));
            var b1 = (Vector2)(center + rotation * (center - new Vector3(self.min.x, self.max.y, 0f)));
            var c1 = (Vector2)(center + rotation * (center - new Vector3(self.max.x, self.max.y, 0f)));

            var a2 = a1;
            var b2 = c1;
            var c2 = (Vector2)(center + rotation * (center - new Vector3(self.max.x, self.min.y, 0f)));

            var cl1 = ClosestPointInTriangle(point, a1, b1, c1);
            var cl2 = ClosestPointInTriangle(point, a2, b2, c2);

            var result = Vector3.Distance(cl1, point) < Vector3.Distance(cl2, point) ? cl1 : cl2;
            
            return result;
        }

        public static void DebugDrawBounds(this Bounds bounds, Color color)
        {
            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(bounds.min.x, bounds.min.y, 0);
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, 0);
            corners[2] = new Vector3(bounds.max.x, bounds.max.y, 0);
            corners[3] = new Vector3(bounds.min.x, bounds.max.y, 0);
        
            for (int i = 0; i < 4; i++)
                Debug.DrawLine(corners[i], corners[(i + 1) % 4], color);
        }
        
        private static Vector3 ClosestPointInTriangle(Vector3 p, Vector3 t0, Vector3 t1, Vector3 t2)
        {
            Plane plane = new Plane(t0, t1, t2);
            p = plane.ClosestPointOnPlane(p);

            if (PointInTriangle(p, t0, t1, t2)) {
                return p;
            }

            var c1 = ClosestPointInLine(p, t0, t1);
            var c2 = ClosestPointInLine(p, t1, t2);
            var c3 = ClosestPointInLine(p, t2, t0);

            float mag1 = (p - c1).magnitude;
            float mag2 = (p - c2).magnitude;
            float mag3 = (p - c3).magnitude;

            float min = Mathf.Min(mag1, mag2);
            min = Mathf.Min(min, mag3);

            if (min == mag1)
                return c1;
            
            if (min == mag2)
                return c2;
            
            return c3;
        }

        private static Vector3 ClosestPointInLine(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector3 lineDirection = b - a;
            float lineDirectionMagnitude = lineDirection.magnitude;
            lineDirection.Normalize();
            float projectLength = Mathf.Clamp(Vector3.Dot(point - a, lineDirection), 0f, lineDirectionMagnitude);
            return a + lineDirection * projectLength;
        }
        
        private static bool PointInTriangle(Vector3 p, Vector3 t0, Vector3 t1, Vector3 t2) 
        {
            t0 -= p;
            t1 -= p;
            t2 -= p;

            Vector3 u = Vector3.Cross(t1, t2);
            Vector3 v = Vector3.Cross(t2, t0);
            Vector3 w = Vector3.Cross(t0, t1);
            
            if (Vector3.Dot(u, v) < 0f) {
                return false;
            }
            if (Vector3.Dot(u, w) < 0.0f) {
                return false;
            }
            
            return true;
        }
    }
}