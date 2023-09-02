// ----- C#
using System;
using System.Collections;
using System.Collections.Generic;

// ----- Unity
using UnityEngine;
using UnityEngine.UI;

// ----- User Defined
using Utility.FroSettings.ForCurve;

namespace InGame.ForUI
{
    public class CurveView : MonoBehaviour
    {
        // --------------------------------------------------
        // Components
        // --------------------------------------------------
        [Header("Type")]
        [SerializeField] private ECurveType     _curveType      = ECurveType.Unknown;

        [Space][Header("Rect")]
        [SerializeField] private RectTransform  _RECT_Space     = null;
        [SerializeField] private RectTransform  _RECT_Point     = null;

        [Space] [Header("Line Texture")]
        [SerializeField] private Transform      _projectParents = null;
        [SerializeField] private Image          _pointOriginImg = null;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        // ----- Private
        private bool           _isStart        = false;

        private Vector2        _pointPos       = Vector2.zero;
        private List<Image>    _pointGroup     = new List<Image>();

        private float          _sec            = 0.0f;
        private float          _curveTotalSec  = 0.0f;
        private float          _cavasWidth     = 0.0f;
        private float          _cavasHeight    = 0.0f;
        
        private AnimationCurve _targetCurve    = null;

        private Coroutine      _coroutine      = null;

        // --------------------------------------------------
        // Functinos - Event
        // --------------------------------------------------
        private void Start()
        {
            // Rect 초기화
            _cavasWidth  = _RECT_Space.rect.size.x;
            _cavasHeight = _RECT_Space.rect.size.y;
            _targetCurve = CurveSettings.Instance.GetCurve(_curveType);

            // Animation Curve 시간 구하기
            Keyframe[] keyframes = _targetCurve.keys;

            for (int i = 0; i < keyframes.Length - 1; i++)
            {
                float deltaTime = keyframes[i + 1].time - keyframes[i].time;
                _curveTotalSec += deltaTime;
            }

            if (_coroutine == null)
                _coroutine = StartCoroutine(_Co_DrawToProject());
        }

        private void Update()
        {
            if (_isStart)
            {
                if (_sec >= _curveTotalSec) 
                {
                    _sec = 0.0f;
                    for (int i = 0; i < _pointGroup.Count; i++)
                    {
                        var dot = _pointGroup[i];
                        Destroy(dot.gameObject);
                    }
                }
                else _sec += Time.deltaTime / 10f;

                _pointPos.x = (_sec * _cavasWidth) - _cavasWidth / 2f;
                _pointPos.y = (_targetCurve.Evaluate(_sec) * _cavasHeight / 2f);

                _RECT_Point.anchoredPosition = _pointPos;

                if (_coroutine == null)
                    _coroutine = StartCoroutine(_Co_DrawToProject());
            }
        }

        // --------------------------------------------------
        // Functinos - Nomal
        // --------------------------------------------------
        public void Active(bool isStart)
        {
            _isStart = isStart;

            if (!isStart)
            {
                _sec = 0.0f;

                for (int i = 0; i < _pointGroup.Count; i++)
                {
                    var dot = _pointGroup[i];
                    Destroy(dot.gameObject);
                }
                _pointGroup.Clear();
                
                StopCoroutine(_Co_DrawToProject());
                _coroutine = null;

                _RECT_Point.anchoredPosition = new Vector2(-1 * (_cavasWidth / 2f), 0f);
            }
        }

        // --------------------------------------------------
        // Functinos - Coroutine
        // --------------------------------------------------
        private IEnumerator _Co_DrawToProject()
        {
            var pointImg = Instantiate(_pointOriginImg, _projectParents);

            var RECT_Point              = pointImg.GetComponent<RectTransform>();
            RECT_Point.anchoredPosition = _RECT_Point.anchoredPosition;
            
            _pointGroup.Add(pointImg);


            yield return new WaitForSeconds(0.2f);
            _coroutine = null;
        }
    }
}