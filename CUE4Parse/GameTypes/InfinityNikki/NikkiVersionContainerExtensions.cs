using System.Diagnostics;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.GameTypes.InfinityNikki.Encryption.Aes;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.GameTypes.InfinityNikki;

public static class NikkiVersionContainerExtensions
{
    public static bool IsInfinityNikkiVersion(this VersionContainer container)
        => container.Game
            is EGame.GAME_InfinityNikkiCbt1
            or EGame.GAME_InfinityNikkiCbt3
            or EGame.GAME_InfinityNikki
            or EGame.GAME_InfinityNikkiV19;

    public static bool IsInfinityNikkiCustomAesVersion(this VersionContainer container)
        => container.Game is EGame.GAME_InfinityNikki or EGame.GAME_InfinityNikkiV19;

    public static bool IsInfinityNikkiIoStoreBlockProcessNeeded(this VersionContainer container)
        => container.Game is EGame.GAME_InfinityNikki;

    public static bool IsInfinityNikkiPakEntryBitfieldChanged(this VersionContainer container)
        => container.Game is EGame.GAME_InfinityNikkiV19;

    public static byte[] InfinityNikkiPostProcessDecryptedData(this VersionContainer container, byte[] decrypted, FAesKey key)
        => container.Game switch
        {
            EGame.GAME_InfinityNikki => InfinityNikkiAes.PostProcessDecryptedData(decrypted, key),
            EGame.GAME_InfinityNikkiV19 => InfinityNikkiAes.PostProcessDecryptedDataV19(decrypted, key),
            _ => throw new UnreachableException()
        };
}
