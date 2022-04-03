using System;
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
        var xString = x.ToString();
        var encoded = new StringBuilder(xString.Length);

        foreach (var c in xString)
        {
            encoded.Append((char) (c + offset - _integerCharOffset));
        }

        return encoded.ToString();
    }

    private static string Zip(string a, string b)
    {
        var zipped = new StringBuilder(a.Length + b.Length);
    
        foreach (var (first, second) in a.Zip(b))
        {
            zipped.Append(first).Append(second);
        }
    
        if (a.Length > b.Length) zipped.Append(a[b.Length .. a.Length]);
        else if (b.Length > a.Length) zipped.Append(b[a.Length .. b.Length]);
    
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

        var idString = new StringBuilder(10);

        foreach (var c in decoded)
        {
            var x = c - _idOffset;
            
            if (x < 10) idString.Append(x);
        }

        return uint.TryParse(idString.ToString(), out id);
    }

    public static void Test()
    {
        var endAt = int.MaxValue;
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (var t = 0; t < endAt; t++)
        {
            var (ePt, eT) = EncodeReferralId((uint) t);
            var success = TryParseReferralCode(eT, out var oT);
            
            if (t % 1111 == 0 || !success || t != oT)
                Console.WriteLine($"{t} -> {ePt} -> {eT} ({eT.Length}) -> {oT} ({t == oT})");

            if (t != oT) return;
        }

        stopwatch.Stop();
        
        Console.WriteLine($"Finished! {stopwatch.ElapsedMilliseconds}ms | {(stopwatch.ElapsedTicks / (float) 100) / endAt}ns avg");
    }
}