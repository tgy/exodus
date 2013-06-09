using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Exodus.Serialize
{
    public static class Serializer
    {
        static Stream _stream;
        static BinaryFormatter _formatter = new BinaryFormatter();
        static object o;
        static public void Serialize(object o, string path)
        {
            _stream = File.Open(path, FileMode.Create);
            _formatter.Serialize(_stream, o);
            _stream.Close();
        }
        static public object DeSerialize(string path)
        {
            _stream = File.Open(path, FileMode.Open);
            o = _formatter.Deserialize(_stream);
            _stream.Close();
            return o;
        }
        static public byte[] ObjectToByteArray(object o)
        {
            if (o == null)
                return null;
            MemoryStream _memoryStream = new MemoryStream();
            _formatter.Serialize(_memoryStream, o);
            return _memoryStream.ToArray();
        }
        static public object ByteArrayToObject(byte[] b)
        {
            MemoryStream _memoryStream = new MemoryStream();
            _memoryStream.Write(b, 0, b.Length);
            _memoryStream.Seek(0, SeekOrigin.Begin);
            return _formatter.Deserialize(_memoryStream);
        }
        static public object ByteArrayToGame(byte[] b)
        {
            _formatter.Binder = new ExodusGameManagerBinderToExodus();
            return ByteArrayToObject(b);
        }
        sealed class ExodusGameManagerBinderToExodus : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                // The following line of code returns the type.
                /*typeToDeserialize = Type.GetType(String.Format("{0}, {1}", 
                    typeName, assemblyName));*/
                Type typeToDeserialize = Type.GetType("Exodus.Network.Game, Exodus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                return typeToDeserialize;
            }
        }
    }
}