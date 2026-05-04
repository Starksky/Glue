using Project.Core.Surfaces.Scripts;
using UnityEngine;

namespace Project.Shared.Scripts.Extensions
{
    public static class Rigidbody2DExtensions
    {
        public static void ApplyParams(this Rigidbody2D body, Rigidbody2DParams parameters)
            => parameters.Apply(body);
    }
}