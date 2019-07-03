using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace M5.Xamarin.Forms.Form.KeyList
{
    /// <summary>
    /// This list use an internal dictionary for items that are inherited from IKey<TKey>.
    /// The add/insert whill be slower, but the select whill be faster than a normal list.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TListElement">The element type</typeparam>
    public class FormKeyList<TKey, TListElement, TDictionaryElement> : IList<TListElement> where TDictionaryElement : TListElement, IKey<TKey>
    {
        private readonly IList<TListElement> list;
        private readonly IDictionary<TKey, TDictionaryElement> dictionary;

        public FormKeyList(IList<TListElement> listToManipulate)
        {
            if (listToManipulate == null)
            {
                list = new List<TListElement>();
            }
            else
            {
                list = listToManipulate;
            }

            dictionary = new Dictionary<TKey, TDictionaryElement>();
            RemakeDictionary();
        }

        public void RemakeDictionary()
        {
            dictionary.Clear();
            foreach (var item in list)
            {
                if (item is TDictionaryElement keyItem)
                {
                    dictionary.Add(keyItem.Key, keyItem);
                }
            }
        }

        public TDictionaryElement this[TKey key]
        {
            get => dictionary[key];
            set
            {
                int index = IndexOf(dictionary[key]);
                Remove(dictionary[key]);
                Insert(index, value);
            }
        }

        public ICollection<TDictionaryElement> DictionaryElements { get => dictionary.Values; }

        public TListElement this[int index]
        {
            get => list[index];
            set
            {
                bool remakeDictionary = false;

                //if last value was a IKey we remove it from dictionary
                if (list[index] is TDictionaryElement keyItem)
                {
                    if (!dictionary.Remove(keyItem.Key))
                    {
                        remakeDictionary = true;
                    }
                }
                //change value in the list
                list[index] = value;

                //if the last value is not in the dictionary (never was or we succeed in removein it)
                if (!remakeDictionary)
                {
                    //if the new value need to be in dictionary
                    if (value is TDictionaryElement key)
                    {
                        dictionary.Add(key.Key, key);
                    }
                }
                //if a dictionar need to be remake (the old value is still in the dictionary (not able to remove it))
                if (remakeDictionary)
                {
                    RemakeDictionary();
                }
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => list.IsReadOnly;

        public void Add(TListElement item)
        {
            list.Add(item);
            if (item is TDictionaryElement key)
            {
                dictionary.Add(key.Key, key);
            }
        }

        public void Clear()
        {
            list.Clear();
            dictionary.Clear();
        }

        public bool Contains(TListElement item)
        {
            return list.Contains(item);
        }

        public void CopyTo(TListElement[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TListElement> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(TListElement item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, TListElement item)
        {
            list.Insert(index, item);
            if (item is TDictionaryElement key)
            {
                dictionary.Add(key.Key, key);
            }
        }

        public bool Remove(TListElement item)
        {
            if (list.Remove(item))
            {
                if (item is IKey<TKey> key)
                {
                    if (!dictionary.Remove(key.Key))
                    {
                        RemakeDictionary();
                    }
                }
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            var item = list[index];
            list.RemoveAt(index);
            if (item is IKey<TKey> key)
            {
                if (!dictionary.Remove(key.Key))
                {
                    RemakeDictionary();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
