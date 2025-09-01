using System;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.UE4.VirtualFileSystem;
using AesProvider = CUE4Parse.Encryption.Aes.Aes;

namespace CUE4Parse.GameTypes.InfinityNikki.Encryption.Aes;

public static class InfinityNikkiAes
{
    private const int AesBlockSize = 16;

    // Not used for any of the CBT packages!
    public static byte[] InfinityNikkiDecrypt(byte[] bytes, int beginOffset, int count, bool isIndex, IAesVfsReader reader)
    {
        if (bytes.Length < beginOffset + count)
            throw new IndexOutOfRangeException("beginOffset + count is larger than the length of bytes");
        if (count % AesBlockSize != 0)
            throw new ArgumentException("count must be a multiple of 16");
        if (reader.AesKey == null)
            throw new NullReferenceException("reader.AesKey");

        var output = bytes.Decrypt(beginOffset, count, reader.AesKey);
        return PostProcessDecryptedData(output, reader.AesKey);
    }

    public static byte[] InfinityNikkiV19Decrypt(byte[] bytes, int beginOffset, int count, bool isIndex, IAesVfsReader reader)
    {
        if (bytes.Length < beginOffset + count)
            throw new IndexOutOfRangeException("beginOffset + count is larger than the length of bytes");
        if (count % AesBlockSize != 0)
            throw new ArgumentException("count must be a multiple of 16");
        if (reader.AesKey == null)
            throw new NullReferenceException("reader.AesKey");

        var output = bytes.Decrypt(beginOffset, count, reader.AesKey);
        return PostProcessDecryptedDataV19(output, reader.AesKey);
    }

    public static byte[] PostProcessDecryptedDataV19(byte[] data, FAesKey key)
    {
        for (var i = 0; i < data.Length; i += AesBlockSize)
        {
            data[i] ^= key.Key[^1];
            if (data.Length >= i + AesBlockSize)
                data[i + AesBlockSize - 1] ^= key.Key[0];
        }

        return data;
    }

    public static byte[] PostProcessDecryptedData(byte[] data, FAesKey key)
    {
        for (var i = 0; i < data.Length; i += AesBlockSize)
        {
            data[i] ^= key.Key[0];
            if (data.Length >= i + AesBlockSize)
                data[i + AesBlockSize - 1] ^= key.Key[^1];
        }

        return data;
    }
}
