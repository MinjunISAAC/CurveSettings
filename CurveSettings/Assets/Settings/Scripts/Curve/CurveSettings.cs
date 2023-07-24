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
                    Debug.LogError("[CurveSettings] Curve Settings ������ �ε����� ���߽��ϴ�. ������ �����ϴ��� Ȯ���Ͻʽÿ�.");

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
        /// Curve Type�� ���� Curve ������ ���� �� �ִ� �Լ�.
        /// Curve Type�� �����ϴ��� CurveSettings�� Curve�� ��ϵǾ� ���� ������ �������� ���� �� �ֽ��ϴ�.
        /// Curve Settings ������ Resources ������ �����մϴ�.
        /// </summary>
        public AnimationCurve GetCurve(ECurveType type)
        {
            if (_infoSet.TryGetValue(type, out var info))
                return info.Curve;

            if (null == _curveInfoList || 0 == _curveInfoList.Count)
            {
                Debug.LogError("[CurveSettings.GetCurve] �ش� Type�� Curve Info�� �����ϴ�.");
                return default;
            }

            Debug.LogWarning($"[CurveSettings.GetCurve] �ش� Type�� ���� Curve Info�� �����ϴ�. type: {type}");
            return _curveInfoList[0].Curve;
        }

        /// <summary>
        /// Curve Info�� �ʱ�ȭ�ϴ� ����Դϴ�.
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