using System;

namespace DefaultNamespace
{
    public class PersonModel
    {
        public event Action<int> NumJumpsChangedEvent;

        private int _numJumps;
        public int NumJumps
        {
            get => _numJumps;
            set
            {
                _numJumps = value;
                NumJumpsChangedEvent?.Invoke(_numJumps);
            } }
    }
}