using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class NumJumpsView : MonoBehaviour
    {
        public Button resetNumJumpsButton;
        public TextMeshProUGUI _text;
        
        private PersonModel _personModel;

        private void Start()
        {
            _personModel = Locator.Instance.PersonModel;
            _personModel.NumJumpsChangedEvent += OnNumJumpsChanged;
            
            resetNumJumpsButton.onClick.AddListener(OnResetButtonClicked);
        }

        private void OnResetButtonClicked()
        {
            _personModel.NumJumps = 0;
        }

        private void OnNumJumpsChanged(int numJumps)
        {
            _text.text = numJumps.ToString();
        }
    }
}