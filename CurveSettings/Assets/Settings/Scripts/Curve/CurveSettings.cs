// ----- C#
using System.Collections.Generic;

// ----- Unity
using UnityEngine;

namespace Settings.ForCurve
{
    [CreateAssetMenu(fileName = "CurveSettings", menuName = "Settings/Create Curve Settings")]
    public class CurveSettings : ScriptableObject
    {
        // --------------------------------------------------
        // Singleton
        // --------------------------------------------------
        // ----- Constructor
        private CurveSettings() { }

        // ----- Instance
        private static CurveSettings _instance = null;

        // ----- Variables
        // If the file location needs to be changed, please modify the corresponding variable.
        private const string SETTING_PATH = "Settings/CurveSettings";

        // ----- Instance Getter
        public static CurveSettings Instance
        {
            get
            {
                if (null != _instance)
                    return _instance;

                _instance = Resources.Load<CurveSettings>(SETTING_PATH);

                if (null == _instance)
                    Debug.LogError("[CurveSettings] Curve Settings 파일을 로드하지 못했습니다. 파일이 존재하는지 확인하십시오.");

                _instance._InitCurveInfoSet();

                return _instance;
            }
        }

        // --------------------------------------------------
        // Curve Info Class
        // --------------------------------------------------
        [System.Serializable]
        public class CurveInfo
        {
            public ECurveType     CurveType = ECurveType.Unknown;
            public AnimationCurve Curve     = null;
        }

        // --------------------------------------------------
        // Componenst
        // --------------------------------------------------
        [SerializeField] private List<CurveInfo> _curveInfoList;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        private Dictionary<ECurveType, CurveInfo> _infoSet = null;

        // --------------------------------------------------
        // Function - Nomal
        // --------------------------------------------------
        /// <summary>
        /// Curve Type에 따른 Curve 정보를 받을 수 있는 함수.
        /// Curve Type이 존재하더라도 CurveSettings에 Curve가 등록되어 있지 않으면 동작하지 않을 수 있습니다.
        /// Curve Settings 파일은 Resources 폴더에 존재합니다.
        /// </summary>
        public AnimationCurve GetCurve(ECurveType type)
        {
            if (_infoSet.TryGetValue(type, out var info))
                return info.Curve;

            if (null == _curveInfoList || 0 == _curveInfoList.Count)
            {
                Debug.LogError("[CurveSettings.GetCurve] 해당 Type의 Curve Info가 없습니다.");
                return default;
            }

            Debug.LogWarning($"[CurveSettings.GetCurve] 해당 Type에 대한 Curve Info가 없습니다. type: {type}");
            return _curveInfoList[0].Curve;
        }

        /// <summary>
        /// Curve Info를 초기화하는 기능입니다.
        /// </summary>
        private void _InitCurveInfoSet()
        {
            _infoSet?.Clear();
            _infoSet = new Dictionary<ECurveType, CurveInfo>();

            for (int i = 0, size = _curveInfoList?.Count ?? 0; i < size; ++i)
            {
                var info = _curveInfoList[i];
                if (null == info)
                    continue;

                _infoSet[info.CurveType] = info;
            }
        }
    }
}