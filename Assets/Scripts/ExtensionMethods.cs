using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class ExtensionMethods
{
    // Deep clone
    public static T DeepClone<T>(this T a)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
    public static bool IsEqualTo(this Color me, Color other)
    {
        return me.r == other.r && me.g == other.g && me.b == other.b && me.a == other.a;
    }
}
