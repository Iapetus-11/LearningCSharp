using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace LearningCSharp;

public static class FakeHashCode
{
    private const int _idOffset = 97;
    private const int _paddingOffset = 107;
    private const int _integerCharOffset = 48;
    
    private static string EncodeUint(uint x, int offset)
    {
        var encoded = new StringBuilder();
        offset -= _integerCharOffset;

        foreach (var c in x.ToString())
        {
            encoded.Append((char) (c + offset));
        }

        return encoded.ToString();
    }

    private static string Zip(string a, string b)
    {
        var zipped = new StringBuilder();

        foreach (var (first, second) in a.Zip(b))
        {
            zipped.Append(first).Append(second);
        }

        if (a.Length > b.Length) zipped.Append(a[..^b.Length]);
        else if (b.Length > a.Length) zipped.Append(b[..^a.Length]);

        return zipped.ToString();
    }

    public static (string, string) EncodeReferralId(uint id)
    {
        var encoded = EncodeUint(id, _idOffset);

        encoded = Zip(encoded, EncodeUint((id / int.MaxValue + 1) << 24, _paddingOffset)[.. Math.Max(8 - encoded.Length, 0)]);
        
        return (encoded, WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(encoded)));
    }

    public static bool TryParseReferralCode(string code, out uint id)
    {
        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var idString = new StringBuilder();

        foreach (var c in decoded)
        {
            var x = c - 97;
            if (x < 10) idString.Append(x);
        }

        return uint.TryParse(idString.ToString(), out id);
    }

    public static void Test()
    {
        var stopwatch = new Stopwatch();
        var times = new List<long>(5000);
            
        for (var t = 0; t < 5000; t++)
        {
            stopwatch.Restart();

            string ePt = "Test", eT = "Test";
            uint oT = 69;

            for (var i = 0; i < 100; i++)
            {
                (ePt, eT) = EncodeReferralId((uint) t);
                TryParseReferralCode(eT, out oT);
            }

            var elapsedNs = stopwatch.ElapsedTicks / 100;
            times.Add(elapsedNs);
            Console.WriteLine($"{t} -> {ePt} -> {eT} ({eT.Length}) -> {oT} ({t == oT}) -- {elapsedNs}ns -|- avg: {(float) times.Sum() / times.Count()}");
        }
    }
}