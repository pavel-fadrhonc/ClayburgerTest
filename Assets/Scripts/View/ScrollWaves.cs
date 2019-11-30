using System;
using UnityEngine;

namespace View
{
    public class ScrollWaves : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _scrollSpeed;
        
        public Vector2 ScrollSpeed
        {
            get => _scrollSpeed;
            set => _scrollSpeed = value;
        }
        
        private Material _mat;
        
        private void Awake()
        {
            _mat = GetComponent<Renderer>().sharedMaterial;
        }

        private void Update()
        {
            _mat.SetTextureOffset("_BumpMap", _mat.GetTextureOffset("_BumpMap") + _scrollSpeed * Time.deltaTime);
        }
    }
}