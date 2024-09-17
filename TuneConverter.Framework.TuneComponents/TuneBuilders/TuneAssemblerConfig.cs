using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.Types;
using System.Text.Json;

namespace TuneConverter.Framework.TuneComponents.TuneBuilders;

internal class TuneAssemblerConfig
{
    static readonly JsonSerializerOptions jsonSerializerOptions = new(){  PropertyNameCaseInsensitive = true };

    public static Dictionary<TuneType, List<int>> BarAndLineLengths { get; set; } = [];
    public static Dictionary<string, NoteType> NoteType { get; set; } = [];
    public static Dictionary<string, AccidentalType> AccidentalType { get; set; } = [];
    public static Dictionary<string, OctaveType> OctaveType { get; set; } = [];
    public static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> ScaleType { get; set; } = [];
    public static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> MinorScaleType { get; set; } = [];
    public static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> DorianScaleType { get; set; } = [];
    public static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> MixolydianScaleType { get; set; } = [];

    /// <summary>
    /// Loads the Dictionaries necessary for TuneAssembler
    /// </summary>
    public static void LoadFromJson()
    {
        DeserialiseBarAndLineLengths();
    }

    private static void DeserialiseBarAndLineLengths()
    {
        List<string> list = ReadJSON();

        var deserializationActions = new List<Action<string>>
        {
            line => BarAndLineLengths = DeserializeJsonToDictionary<TuneType, List<int>>(line),
            line => NoteType = DeserializeJsonToDictionary<string, NoteType>(line),
            line => AccidentalType = DeserializeJsonToDictionary<string, AccidentalType>(line),
            line => OctaveType = DeserializeJsonToDictionary<string, OctaveType>(line),
            line => ConstructDict(line, ScaleType),
            line => ConstructDict(line, MinorScaleType),
            line => ConstructDict(line, DorianScaleType),
            line => ConstructDict(line, MixolydianScaleType)
        };

        for (int i = 0; i < deserializationActions.Count && i < list.Count; i++)
        {
            deserializationActions[i](list[i]);
        }
    }

    public static Dictionary<T1, T2> DeserializeJsonToDictionary<T1, T2>(string jsonString) where T1 : notnull
        => JsonSerializer.Deserialize<Dictionary<T1, T2>>(jsonString, jsonSerializerOptions) ?? [];
    
    private static void ConstructDict(string oldJson, Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> scale)
    {
        // Find starting curly brace of the first sub dictionary
        int startIndex = oldJson.IndexOf('{',1);

        while (startIndex > -1)
        {
            // Find the closest closing curly brace
            int endIndex = oldJson.IndexOf('}', startIndex);
            if (endIndex == -1)
            {
                break; 
            }

            // extract JSON object
            string jsonObject = oldJson.Substring(startIndex, endIndex - startIndex + 1);

            try
            {
                // Extract dictionary from JSON, if valid, take first note from Dictionary and set it as the key in
                // main dictionary.
                Dictionary<NoteType, AccidentalType> subDictionary = JsonSerializer.Deserialize<Dictionary<NoteType, AccidentalType>>(jsonObject, jsonSerializerOptions) ?? [];

                if (subDictionary != null && subDictionary.Count != 0)
                {
                    var firstPair = subDictionary.First();
                    var noteType = firstPair.Key;
                    var accidentalType = firstPair.Value;

                    scale.TryAdd((noteType, accidentalType), subDictionary);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}");
            }

            // Find the next opening curly brace
            startIndex = oldJson.IndexOf('{', endIndex);
        }
    }

    private static List<string> ReadJSON()
    {
        string filePath = "C:\\Users\\Isaac\\source\\repos\\TuneConverter\\TuneConverter.Framework.TuneComponents\\TuneBuilders\\TuneAssemblerConfig.json";
        var testFoo = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var testFoo2 = Directory.GetCurrentDirectory();
        string jsonString = File.ReadAllText(filePath);

        List<string> jsonList = [];

        // Get all main dictionary closing braces
        var closingBraceIndices = Enumerable.Range(0, jsonString.Length)
                                            .Where(i => jsonString[i] == '}')
                                            .ToList();
        string pattern = "\r\n  ";

        var validClosingBraceIndices = closingBraceIndices
            .Where(i => i >= pattern.Length && jsonString.Substring(i - pattern.Length, pattern.Length) == pattern)
            .ToList();


        // Get all main dictionary opening braces
        string startPattern = "\r\n    \"";
        var openingBraceIndices = Enumerable.Range(0, jsonString.Length)
                                            .Where(i => jsonString[i] == '{')
                                            .ToList();

        var validOpeningBraceIndices = openingBraceIndices
            .Where(i => i + startPattern.Length <= jsonString.Length &&
                        jsonString.Substring(i + 1, startPattern.Length) == startPattern)
            .ToList();


        // Get all substrings between each pair of braces
        for (int i = 0; i < validOpeningBraceIndices.Count; i++)
        {
            int startIndex = validOpeningBraceIndices[i];
            int endIndex = validClosingBraceIndices[i];
            if (startIndex >= 0)
            {
                string jsonObject = jsonString.Substring(startIndex, endIndex - startIndex + 1);
                jsonList.Add(jsonObject.Replace("\r\n", ""));
            }
        }

        return jsonList;
    }
}
