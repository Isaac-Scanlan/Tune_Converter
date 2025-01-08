using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.TuneComponents;

namespace TuneConverter.Framework.TuneIO;

/// <summary>
/// 
/// </summary>
public class TuneCompressor
{
    /// <summary>
    /// Serializes a <see cref="TuneFull"/> object to a JSON string, compresses it, and returns the compressed data as a Base64-encoded string.
    /// </summary>
    /// <param name="tune">The <see cref="TuneFull"/> object to be serialized and compressed.</param>
    /// <returns>A Base64-encoded <see cref="string"/> representing the GZip-compressed JSON serialization of the <paramref name="tune"/> object.</returns>
    /// <remarks>
    /// This method uses JSON serialization to convert the <see cref="TuneFull"/> object to a string format, 
    /// compresses the string using the GZip algorithm, and then encodes the compressed data in Base64 for easy storage or transmission.
    /// </remarks>
    public static string CompressTune(TuneFull tune)
    {
        return CompressString(JsonConvert.SerializeObject(tune));
    }

    /// <summary>
    /// Compresses a UTF-8 encoded string and returns the compressed data as a Base64-encoded string.
    /// </summary>
    /// <param name="tune">The input <see cref="string"/> to be compressed.</param>
    /// <returns>A Base64-encoded <see cref="string"/> representing the GZip-compressed version of the input string.</returns>
    /// <remarks>
    /// This method compresses the input string using the GZip algorithm and then encodes the compressed byte array
    /// into a Base64 string for easy storage or transmission.
    /// </remarks>
    public static string CompressString(string tune)
    {
        return Convert.ToBase64String(Zip(tune));
    }

    /// <summary>
    /// Compresses a UTF-8 encoded string into a GZip-compressed byte array.
    /// </summary>
    /// <param name="str">The input <see cref="string"/> to be compressed.</param>
    /// <returns>A GZip-compressed <see cref="byte"/> array representing the compressed version of the input string.</returns>
    /// <remarks>
    /// This method converts the input string to a UTF-8 encoded byte array and uses a <see cref="GZipStream"/> to compress it.
    /// The compressed data is written to an output <see cref="MemoryStream"/>, which is then converted to a byte array and returned.
    /// </remarks>
    public static byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }
    }

    /// <summary>
    /// Decompresses a Base64-encoded, GZip-compressed string and returns the original UTF-8 encoded string.
    /// </summary>
    /// <param name="tune">The Base64-encoded <see cref="string"/> to be decompressed.</param>
    /// <returns>The original <see cref="string"/> that was GZip-compressed and Base64-encoded.</returns>
    /// <remarks>
    /// This method decodes the input string from Base64 format to a byte array, decompresses the byte array using the GZip algorithm,
    /// and returns the resulting UTF-8 encoded string.
    /// </remarks>
    public static string DeCompressString(string tune)
    {
        byte[] r1Back = Convert.FromBase64String(tune);

        string r2 = Unzip(r1Back);

        return r2;

    }

    /// <summary>
    /// Decompresses a GZip-compressed byte array and returns the decompressed content as a UTF-8 encoded string.
    /// </summary>
    /// <param name="bytes">The GZip-compressed data as a byte array.</param>
    /// <returns>A <see cref="string"/> containing the decompressed content encoded in UTF-8.</returns>
    /// <remarks>
    /// This method uses a <see cref="MemoryStream"/> to hold the compressed data and another <see cref="MemoryStream"/>
    /// to receive the decompressed data. The <see cref="GZipStream"/> is used to handle the decompression, and the data is
    /// copied from the decompression stream to the output stream. Finally, the decompressed byte array is converted to a UTF-8 string.
    /// </remarks>
    public static string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                CopyTo(gs, mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    /// <summary>
    /// Copies all data from the specified source stream to the specified destination stream.
    /// </summary>
    /// <param name="src">The source <see cref="Stream"/> to read data from.</param>
    /// <param name="dest">The destination <see cref="Stream"/> to write data to.</param>
    /// <remarks>
    /// This method reads data from the source stream in chunks of 4096 bytes and writes it to the destination stream.
    /// It continues reading and writing until all data from the source stream is copied to the destination stream.
    /// </remarks>
    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

}
