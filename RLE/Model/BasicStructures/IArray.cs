using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicStructures
{
    public interface IArray<T>
    {
        /// <summary>
        /// Метод добавления и удаления элементов
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        void Add(T item, long index);
        /// <summary>
        /// Возвращает удаляемый элемент по индексу
        /// </summary>
        /// <returns></returns>
        T Remove(long index);
        /// <summary>
        /// Возвращает массив
        /// </summary>
        /// <returns></returns>
        T[] Array();
        T Get(int index);
    }
}
