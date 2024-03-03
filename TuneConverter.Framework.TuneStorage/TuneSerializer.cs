using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using Newtonsoft.Json;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.IO;

namespace TuneConverter.Framework.TuneStorage
{
   

    public class TuneSerializer
    {
        private Dictionary<string, byte[]> tunes = new Dictionary<string, byte[]>();

        private static readonly string _notesPath = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/TuneRepo/";

        public TuneSerializer()
        {

        }

        public static string SerializeTune(TuneFull tuneFull)
        {
            return JsonConvert.SerializeObject(tuneFull);
        }

        public static TuneFull DeserializeTune(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<TuneFull>(value: jsonString) ?? new TuneFull();
            }
            catch(Exception ex)
            {
                return new TuneFull();
            }
            
        }


        // Method to convert a list of strings to a list of byte arrays using compression
        public static List<byte[]> ConvertStringListToByteArrayList(List<string> stringList)
        {
            List<byte[]> byteArrayList = new List<byte[]>();
            foreach (string str in stringList)
            {
                byte[] compressedBytes = Zip(str);
                byteArrayList.Add(compressedBytes);
            }
            return byteArrayList;
        }

        // Method to save a list of byte arrays to a file
        public static void SaveByteArrayListToFile(List<byte[]> byteArrayList, string filePath)
        {
            using (FileStream fileStream = new FileStream(_notesPath + filePath, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    foreach (byte[] byteArray in byteArrayList)
                    {
                        writer.Write(byteArray.Length);
                        writer.Write(byteArray);
                    }
                }
            }
        }

        // Method to read a list of byte arrays from a file and convert it back to a list of strings
        public static List<string> ConvertByteArrayListToStringList(string filePath)
        {
            List<string> stringList = new List<string>();
            using (FileStream fileStream = new FileStream(_notesPath + filePath, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        int length = reader.ReadInt32();
                        byte[] byteArray = reader.ReadBytes(length);
                        string str = Unzip(byteArray);
                        stringList.Add(str);
                    }
                }
            }
            return stringList;
        }

        public static List<TuneFull> ConvertByteArrayListToTuneFullList(string filePath)
        {
            List<string> list = ConvertByteArrayListToStringList(filePath);

            List<TuneFull> tuneList = new List<TuneFull>();

            foreach (var t in list)
            {
                tuneList.Add(TuneSerializer.DeserializeTune(t));
            }

            return tuneList;
        }

        // Compression method: Zip
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                return mso.ToArray();
            }
        }

        // Compression method: Unzip
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}

