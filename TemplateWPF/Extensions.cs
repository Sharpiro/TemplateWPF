using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace TemplateWPF.Tools
{
    public static class Extensions
    {
        public static DateTime GetEpochStartTime => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        public static List<T> ToList<T>(this IList list)
        {
            //yield return does not work here, if collection is modified before next yield, exception will be thrown
            var genericList = new List<T>();
            foreach (var item in list)
            {
                genericList.Add((T)item);
            }
            return genericList;
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            TValue value = default(TValue);
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static string GetDescriptionOrName(this Enum enumElement)
        {
            var type = enumElement.GetType();
            var info = type.GetField(enumElement.ToString());
            var attribute = info.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute != null ? attribute.Description : enumElement.ToString();
        }
    }
}
