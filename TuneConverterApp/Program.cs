﻿using Emgu.CV;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using TuneConverter.Framework.TuneStorage;
using System.Data.SqlTypes;

namespace Main.TuneConverterApp;

/* 
 * Publish Order
 * _____________
 * 
 * TuneConverter.Framework.ABCConverter
 * TuneConverter.Framework.MIDIConverter
 * TuneConverter.Framework.PageComponents
 * 
 * TuneConverter.Framework.TuneComponents
 * 
 * TuneConverter.Framework.PageImageIO
 * 
 * TuneConverter.Framework.TuneIO
 * 
 * TuneConverter.Framework.TuneStorage
 * 
 * Main.TuneConverterApp
 * 
 * WPFTuneConverter
 * 
 */

public class Program
{ 
    public static void Main(string[] args)
    {
        //Console.BackgroundColor = ConsoleColor.Green;
        //var file = ReadTune("Rosco's.txt");

        var file = ArrangeTuneList(args);

        var start = DateTime.Now;
        var tuneFull = TuneAssembler.AssembleTune(file);

        //var jsonString = JsonConvert.SerializeObject(tuneFull);
        //var hash = GetHashString(jsonString);


        //var obj = JsonConvert.DeserializeObject<TuneFull>(jsonString);

        //var foo = tuneFull == obj;
        //var foo2 = tuneFull == tuneFull with { };

        var middle = DateTime.Now;
        var assembledPage = AssemblePage(tuneFull);

        var end = DateTime.Now;

        var assembleTuneTime = (middle - start);
        var assemblePageTime = (end - middle);

        //Console.WriteLine("AssembleTune: " + assembleTuneTime.ToString());
        //Console.WriteLine("AssemblePage: " + assemblePageTime.ToString());
        //DisplayImage(assembledPage);

        WriteImage(assembledPage, tuneFull.Title, tuneFull.TuneType);

        var subString = "Title\":\"" + tuneFull.Title;

        var tuneDataList = TuneSerializer.ConvertByteArrayListToStringList("data.dat");
        tuneDataList = tuneDataList.Where(s => !s.Contains(subString)).ToList();

        tuneDataList.Add(TuneSerializer.SerializeTune(tuneFull));



        // Convert list of strings to list of byte arrays
        List<byte[]> byteArrayList = TuneSerializer.ConvertStringListToByteArrayList(tuneDataList);

        // Save list of byte arrays to a file
        TuneSerializer.SaveByteArrayListToFile(byteArrayList, "data.dat");
    }

    public static List<List<string>> ReadTune(string fileName)
    {
        TuneReader reader = new();
        return TuneReader.ReadFile(fileName);
    }

    public static List<List<string>> ArrangeTuneList(string[] lines)
    {
        TuneReader reader = new();
        return TuneReader.ArrangeTuneList(lines.ToList());
    }

    public static List<Image<Gray, byte>> AssemblePage(TuneFull tuneFull)
    {
        PageAssembler assemble = new PageAssembler();
        return assemble.CreateTune(tuneFull).Result;
    }

    public static void DisplayImage(List<Image<Gray, byte>> assembledPages)
    {
        foreach (var assembledPage in assembledPages)
        {
            Image<Gray, byte> resizedPage = new(assembledPage.Width, assembledPage.Height);

            CvInvoke.Resize(assembledPage, resizedPage, new Size(), 0.5, 0.5);

            CvInvoke.Imshow("s", resizedPage);
            CvInvoke.WaitKey();
        }
    }

    public static bool WriteImage(List<Image<Gray, byte>> assembledPages, string fileName, TuneType tuneType)
    {
        bool outVal = true;
        foreach (var assembledPage in assembledPages.Select((value, i) => (value, i)))
        {
            var fileDirec = assembledPages.Count > 1 ? fileName : "";
            var pageNum = assembledPages.Count > 1 ? "_page_" + (assembledPage.i + 1) : "";

            TuneImageWriter writer = new(tuneType);
            if (!writer.WriteImage(assembledPage.value, fileName + pageNum, fileDirec) ) 
            {
                outVal = false;
            }
            
        }
        return outVal;
    }

    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

}
