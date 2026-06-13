using System;
using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Configs/Enum Definition")]
    public class EnumDefinitionSO : ScriptableObject
    {
        [SerializeField] string _enumName;
        [SerializeField] string[] _values;

        public string EnumName => _enumName;
        public string[] Values => _values;
    }
}