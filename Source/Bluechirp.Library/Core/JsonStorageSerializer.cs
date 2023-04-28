using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Helpers;
using Newtonsoft.Json;

namespace Bluechirp.Library.Core
{
    /// <summary>
    /// Custom serializer for application storage based on Newtonsoft.Json
    /// </summary>
    public class JsonStorageSerializer : IObjectSerializer
    {
        /// <summary>
        /// Serializes an object into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type to serialize from.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The serialized JSON string.</returns>
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Deserializes a JSON string back into a type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="value">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
