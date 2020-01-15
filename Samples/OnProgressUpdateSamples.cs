﻿using IEnumerableCorrelater;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class OnProgressUpdateSamples
    {
        private MyUi<string> myUi = new MyUi<string>();
        string[] collection1 = { "A", "D", "C" };
        string[] collection2 = { "A", "B", "C" };

        public void OnProgressUpdate()
        {
            ICorrelater<string> correlater = new LevenshteinCorrelater<string>(10, 7, 7);
            correlater.OnProgressUpdate += (int currentProgress, int totalProgress) =>
            {
                // Do something with the progress update here
                myUi.UpdateProgress(currentProgress, totalProgress);
            };

            EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);
            CorrelaterResult<string> result = enumerableCorrelater.Correlate(collection1, collection2);
        }
    }
}