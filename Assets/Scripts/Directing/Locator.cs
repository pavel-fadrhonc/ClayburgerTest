using UnityEngine;

namespace DefaultNamespace
{
    public class Locator : MonoBehaviour
    {
        #region SINGLETON

        private static Locator _instance;

        public static Locator Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("Locator");
                    _instance = go.AddComponent<Locator>();
                }

                return _instance;
            }
        }
        
        #endregion

        public PersonController PersonController;
        
        public PersonModel PersonModel { get; set; }
    }
}