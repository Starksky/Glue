using System;
using R3;
using SaintsField.Playa;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.Shared.Scripts.Common
{
    [Serializable]
    public enum ETypeMask
    {
        mask,
        I2L
    }
    
    [Serializable]
    public enum ETypeValue
    {
        str,
        TermI2L
    }
    
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(TMP_Text))]
    public class ValueToText : MonoBehaviour
    {
        [SerializeField] private ETypeMask typeMask;
        [SerializeField, ShowIf("typeMask", ETypeMask.mask)] private string mask = "{0}";
        [SerializeField, ShowIf("typeMask", ETypeMask.I2L)] private string term = "";
        [SerializeField] private ETypeValue typeValue;

        [SerializeField] private string format;

        private string Mask
        {
            get
            {
                return typeMask switch
                {
                    ETypeMask.mask => mask,
                    ETypeMask.I2L => null,//I2.Loc.LocalizationManager.GetTranslation(term),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private string Value
        {
            get
            {
                switch (typeValue)
                {
                    case ETypeValue.str:
                        if (_value is { Value: not null })
                            return String.Format(Mask, _value.Value);
                        if (_values is { Value: not null })
                            return String.Format(Mask, _values.Value);
                        break;

                    case ETypeValue.TermI2L:
                        return null;//I2.Loc.LocalizationManager.GetTranslation(String.Format(Mask, _value.Value));
                }
                return null;
            }
        }

        private TMP_Text _text;
        private bool _isAwake;
        private ReactiveProperty<string> _value = new ReactiveProperty<string>();
        private ReactiveProperty<string[]> _values = new ReactiveProperty<string[]>();
        public TMP_Text Text => _text;

        private void Awake()
        {
            if (_isAwake)
                return;
            
            _isAwake = true;
            _text = GetComponent<TMP_Text>();
            _value.CombineLatest(_values, Tuple.Create).Subscribe(_ => UpdateText())
                .RegisterTo(destroyCancellationToken);
        }

        private void OnEnable()
        {
            if (typeMask == ETypeMask.I2L)
            {
                //I2.Loc.LocalizationManager.OnLocalizeEvent += UpdateText;
                UpdateText();
            }
        }
        private void OnDisable()
        {
            //if (typeMask == ETypeMask.I2L)
            //    I2.Loc.LocalizationManager.OnLocalizeEvent -= UpdateText;
        }

        private void UpdateText()
        {
            _text.text = Value;
        }
        public void SetText(string value) 
        {
            Awake();
            _values.Value = null;
            _value.Value = null;
            _text.text = value;
        }
        public void ToText(string value)
        {
            Awake();
            _values.Value = null;
            _value.Value = value;
        }
        public void ToText(int value) 
        {
            Awake();
            _values.Value = null;
            _value.Value = $"{value.ToString(format)}";
        }
        public void ToText(float value) 
        {
            Awake();
            _values.Value = null;
            _value.Value = $"{value.ToString(format)}";
        }
        
        public void ToText(long value) 
        {
            Awake();
            _values.Value = null;
            _value.Value = $"{value.ToString(format)}";
        }

        public void ToText(TimeSpan value)
        {
            Awake();
            _values.Value = null;
            _value.Value = $"{value.ToString(format)}";
        }
        
        public void ToText(params object[] values) 
        {
            Awake();
            
            _value.Value = null;
            
            using(ListPool<string>.Get(out var pool))
            {
                foreach (var value in values)
                    if (value is int {} iv)
                        pool.Add($"{iv.ToString(format)}");
                    else if (value is float {} fv)
                        pool.Add($"{fv.ToString(format)}");
                    else pool.Add($"{value}");
                
                _values.Value = pool.ToArray();
            }
        }
    }
}