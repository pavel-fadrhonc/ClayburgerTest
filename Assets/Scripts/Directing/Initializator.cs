using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Initializator : MonoBehaviour
    {
        private void Awake()
        {
            Locator.Instance.PersonModel = new PersonModel() { NumJumps = 0 };
        }
    }
}