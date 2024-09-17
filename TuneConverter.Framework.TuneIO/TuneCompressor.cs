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

public class TuneCompressor
{
    public static string CompressTune(TuneFull tune)
    {
        var jsonString = JsonConvert.SerializeObject(tune);
        var foo = System.Text.Json.JsonSerializer.Serialize(tune);


        var cp = CompressString(jsonString);
        return cp;

    }

    public static string CompressString(string tune)
    {

        byte[] r1 = Zip(tune);

        string base64 = Convert.ToBase64String(r1);

        return base64;

    }

    public static string DeCompressString(string tune)
    {
        byte[] r1Back = Convert.FromBase64String(tune);

        string r2 = TuneCompressor.Unzip(r1Back);

        return r2;

    }

    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    public static byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                //msi.CopyTo(gs);
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }
    }

    public static string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                //gs.CopyTo(mso);
                CopyTo(gs, mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
}
