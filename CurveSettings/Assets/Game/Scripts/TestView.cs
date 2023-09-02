// ----- C#
using System.Collections;
using System.Collections.Generic;

// ----- Unity
using UnityEngine;
using UnityEngine.UI;

// ----- User Defined
using InGame.ForUI;
using TMPro;

namespace InGame.ForTest
{
    public class TestView : MonoBehaviour
    {
        // --------------------------------------------------
        // Components
        // --------------------------------------------------
        [SerializeField] private CurveView _targetCurveView_Sine   = null;
        [SerializeField] private CurveView _targetCurveView_CoSine = null;
        [SerializeField] private CurveView _targetCurveView_Custom = null;

        [SerializeField] private Button          _BTN_Active   = null;
        [SerializeField] private TextMeshProUGUI _TMP_Contents = null;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        private bool _active = false;

        // --------------------------------------------------
        // Functions - Nomal
        // --------------------------------------------------
        public void OnInit()
        {
            _BTN_Active.onClick.AddListener
            (
                () => 
                {
                    if (_active) 
                    {
                        _TMP_Contents.text = $"START";
                        _active = false;
                    }
                    else
                    {
                        _TMP_Contents.text = $"STOP";
                        _active = true;
                    }
                    
                    _targetCurveView_Sine.  Active(_active);
                    _targetCurveView_CoSine.Active(_active);
                    _targetCurveView_Custom.Active(_active);
                }
            );

        }
    }
}