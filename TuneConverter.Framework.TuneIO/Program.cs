using Newtonsoft.Json;
using System;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO;
using TuneConverter.Framework.TuneIO.TuneReader;

public class Program
{
    public static void Main(string[] args)
    {
       
        TuneReader reader = new TuneReader();
        var file = reader.readFile("Kesh Jig.txt");

        var tuneFull = TuneAssembler.AssembleTune(file);

        //var jsonString = JsonConvert.SerializeObject(tuneFull);

        //byte[] r1 = TuneCompressor.Zip(jsonString);

        //string base64 = Convert.ToBase64String(r1);
        //byte[] r1Back = Convert.FromBase64String(base64);

        //string r2 = TuneCompressor.Unzip(r1Back);

        var cpTune = TuneCompressor.CompressTune(tuneFull);

        var dcpTune = TuneCompressor.DeCompressString(cpTune);

        var obj = JsonConvert.DeserializeObject<TuneFull>(dcpTune);


    }

    //public static string GetHashString(string inputString)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    foreach (byte b in GetHash(inputString))
    //        sb.Append(b.ToString("X2"));

    //    return sb.ToString();
    //}

    //public static byte[] GetHash(string inputString)
    //{
    //    using (HashAlgorithm algorithm = SHA256.Create())
    //        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    //}
}