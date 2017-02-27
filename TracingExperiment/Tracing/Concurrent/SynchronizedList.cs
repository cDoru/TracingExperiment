using System;
using System.Collections.Generic;

namespace TracingExperiment.Tracing.Concurrent
{
    public sealed class SynchronizedList<T> : ThreadSafeList<T>
    {
        readonly List<T> _mList;
        int _mCount;

        public SynchronizedList()
        {
            _mList = new List<T>();
        }

        public override T this[int index]
        {
            get
            {
                var count = _mCount;
                if (index < 0 || index >= count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return _mList[index];
            }
        }

        public override int Count
        {
            get { return _mCount; }
        }

        public override void Add(T element)
        {
            lock (_mList)
            {
                _mList.Add(element);
                ++_mCount;
            }
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            var count = _mCount;
            _mList.CopyTo(0, array, arrayIndex, count);
        }

        #region "Protected methods"

        protected override bool IsSynchronizedBase
        {
            get { return true; }
        }

        #endregion
    }
}