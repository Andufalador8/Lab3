using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject3
{ 
[Serializable]
    public class MyJsonDictionary<K, V> : ISerializable
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        public MyJsonDictionary() { }

        protected MyJsonDictionary(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (K key in dict.Keys)
            {
                info.AddValue(key.ToString(), dict[key]);
            }
        }

        public void Add(K key, V value)
        {
            dict.Add(key, value);
        }

        public V this[K index]
        {
            set { dict[index] = value; }
            get { return dict[index]; }
        }
    }
}