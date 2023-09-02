// ----- C#
using System.Collections;
using System.Collections.Generic;

// ----- Unity
using UnityEngine;

// ----- User Defined
using InGame.ForTest;

namespace InGame
{
    public class Main : MonoBehaviour
    {
        // --------------------------------------------------
        // Components
        // --------------------------------------------------
        [SerializeField] private TestView _textView = null;

        // --------------------------------------------------
        // Functinos - Event
        // --------------------------------------------------
        private void Start()
        {
            _textView.OnInit();
        }
    }
}