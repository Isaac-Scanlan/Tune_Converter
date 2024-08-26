using System;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using TuneConverter.Framework.PageComponents;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

/// <summary>
/// Represents an image of a musical note and related symbols.
/// </summary>
public class NoteImage
{
    /// <summary>
    /// The path to the directory containing the images.
    /// </summary>
    private static readonly string _imagePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/Images/");

    /// <summary>
    /// The local directory path for the project.
    /// </summary>
    private static readonly string _localDirec = Directory.GetParent(
        Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ?? string.Empty;

    /// <summary>
    /// A cache of images loaded from disk, keyed by a string identifier.
    /// </summary>
    private static readonly Dictionary<string, Image<Gray, byte>> _imageCache = new()
    {
        { "A", LoadImage("A.png") },
        { "B", LoadImage("B.png") },
        { "C", LoadImage("C.png") },
        { "D", LoadImage("D.png") },
        { "E", LoadImage("E.png") },
        { "F", LoadImage("F.png") },
        { "G", LoadImage("G.png") },
        { "sharp", LoadImage("sharp.png") },
        { "flat", LoadImage("flat.png") },
        { "natural", LoadImage("natural.png") },
        { "high", LoadImage("high.png") },
        { "low", LoadImage("low.png") },
        { "r", LoadImage("ro.png") },
        { "l", LoadImage("ll.png") },
        { "_", LoadImage("long.png") },
        { "__", LoadImage("small_long.png") },
        { "duplet", LoadImage("duplet.png") },
        { "triplet", LoadImage("triplet.png") },
        { "part1", LoadImage("pt_1.png") },
        { "part2", LoadImage("pt_2.png") },
        { "part3", LoadImage("pt_3.png") },
        { "part4", LoadImage("pt_4.png") },
        { "part5", LoadImage("pt_5.png") },
        { "part6", LoadImage("pt_6.png") },
        { "part7", LoadImage("pt_7.png") },
        { "part8", LoadImage("pt_8.png") },
        { "part9", LoadImage("pt_9.png") },
        { "part10", LoadImage("pt_10.png") },
        { "rep_1", LoadImage("rep_1.png") },
        { "rep_2", LoadImage("rep_2.png") },
        { "rep_3", LoadImage("rep_3.png") },
        { "space", LoadImage("space.png") },
        { "x", LoadImage("x.png") },
        { "linkStart", LoadImage("Link_start.png") },
        { "linkMiddle", LoadImage("Link_mid.png") },
        { "linkEnd", LoadImage("Link_end.png") }
    };

    /// <summary>
    /// A dictionary mapping repeat types to their corresponding images.
    /// </summary>
    private static readonly Dictionary<RepeatType, Image<Gray, byte>> _repeatsDict = new()
    {
        { RepeatType.Single, LoadImage("rep_1.png") },
        { RepeatType.Double, LoadImage("rep_2.png") },
        { RepeatType.Triple, LoadImage("rep_3.png") }
    };

    /// <summary>
    /// Loads an image from the file system.
    /// </summary>
    /// <param name="fileName">The name of the image file to load.</param>
    /// <returns>The loaded image as an <see cref="Image{Gray, Byte}"/> object.</returns>
    private static Image<Gray, byte> LoadImage(string fileName)
    {
        return new Image<Gray, byte>(Path.Combine(_imagePath, fileName));
    }

    /// <summary>
    /// Retrieves an image from the cache based on a string key.
    /// </summary>
    /// <param name="key">The key corresponding to the desired image.</param>
    /// <returns>
    /// The <see cref="Image{Gray, Byte}"/> associated with the key, or null if the key does not exist.
    /// </returns>
    public static Image<Gray, byte>? GetImage(string key)
    {
        return _imageCache.TryGetValue(key, out var image) ? image : null;
    }

    /// <summary>
    /// Retrieves a repeat image based on the <see cref="RepeatType"/> key.
    /// </summary>
    /// <param name="key">The repeat type corresponding to the desired image.</param>
    /// <returns>
    /// The <see cref="Image{Gray, Byte}"/> associated with the repeat type, or null if the key does not exist.
    /// </returns>
    public static Image<Gray, byte>? GetRepImage(RepeatType key)
    {
        return _repeatsDict.TryGetValue(key, out var image) ? image : null;
    }
}
