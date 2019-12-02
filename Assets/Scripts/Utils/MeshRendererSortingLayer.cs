using System;
using UnityEngine;

namespace Utils
{
    public class MeshRendererSortingLayer : MonoBehaviour
    {
        [HideInInspector]
        public int sortingLayer=0;
        private int _sortingLayer;
        [HideInInspector]
        public int orderInLayer=0;
        private int _orderInLayer;
        
        private MeshRenderer mr;

        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
        }

        protected void GetReferences()
        {
            mr=GetComponent<MeshRenderer>();
        }

        protected Material GetMaterial()
        {
            return mr.sharedMaterial;
        }

        protected void Update()
        {
            if (sortingLayer!=_sortingLayer || orderInLayer!=_orderInLayer){
                mr.sortingLayerID=sortingLayer;
                mr.sortingOrder=orderInLayer;
                _sortingLayer=sortingLayer;
                _orderInLayer=orderInLayer;
            }
        }        
    }
}