﻿namespace IEnumerableCorrelater.Interfaces
{
    /// <summary>
    /// A class that can calclate the distance between two elements of type T
    /// </summary>
    public interface IDistanceCalculator<T>
    {
        uint Distance(T element1, T element2);
    }
}
