using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using TuneConverter.Framework.TuneStorage;

public class Program
{
    public static void Main(string[] args)
    {


        //var file_full = ts.ReadFile();
        string[] fyles = Directory.GetFiles("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes");


        // Sample list of strings
        List<string> stringsList = new List<string>
        {
            //TuneSerializer.SerializeTune(TuneAssembler.AssembleTune(ReadTune("Cooley's.txt"))),
            //TuneSerializer.SerializeTune(TuneAssembler.AssembleTune(ReadTune("Boys of Bluehill.txt"))),
            //TuneSerializer.SerializeTune(TuneAssembler.AssembleTune(ReadTune("Byrne's Mill.txt"))),
            //TuneSerializer.SerializeTune(TuneAssembler.AssembleTune(ReadTune("Connie Flemings'.txt"))),
            //TuneSerializer.SerializeTune(TuneAssembler.AssembleTune(ReadTune("Allistrum's.txt")))

        };

        foreach (string fyle in fyles)
        {
            var tuneName = Path.GetFileName(fyle);
            var tuneText = ReadTune(tuneName);
            if(tuneText.Count == 0)
            {
                continue;
            }
            var fullTune = TuneAssembler.AssembleTune(tuneText);
            stringsList.Add(TuneSerializer.SerializeTune(fullTune));
        }

        // Convert list of strings to list of byte arrays
        List<byte[]> byteArrayList = TuneSerializer.ConvertStringListToByteArrayList(stringsList);

        // Save list of byte arrays to a file
        TuneSerializer.SaveByteArrayListToFile(byteArrayList, "data.dat");

        // Read list of byte arrays from file and convert it back to list of strings
        List<string> reversedStringList = TuneSerializer.ConvertByteArrayListToStringList("data.dat");

        // Display reversed list of strings
        Console.WriteLine("Reversed List of Strings:");
        foreach (string str in reversedStringList)
        {
            Console.WriteLine(str + "\n\n");
        }


        List<TuneFull> tuneList = new List<TuneFull>();

        foreach(var t in reversedStringList)
        {
            tuneList.Add(TuneSerializer.DeserializeTune(t));
        }

    }

    public static List<List<string>> ReadTune(string fileName)
    {
        TuneReader reader = new();
        return reader.readFile(fileName);
    }

}