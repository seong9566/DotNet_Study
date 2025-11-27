using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Utilities.Utils;

public static class Common
{
    public static void SetDataProtection(
        WebApplicationBuilder builder,
        string keyPath,
        string appName,
        Enums.CryptoType cryptoType
    )
    {
        var protection = builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(keyPath))
            .SetApplicationName(appName)
            .SetDefaultKeyLifetime(TimeSpan.FromDays(7));

        switch (cryptoType)
        {
            case Enums.CryptoType.Unmanaged:
                protection.UseCryptographicAlgorithms(
                    new AuthenticatedEncryptorConfiguration
                    {
                        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                        ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
                    });
                break;

            case Enums.CryptoType.Managed:
                throw new PlatformNotSupportedException(
                    "ManagedAuthenticatedEncryptorConfiguration은 .NET 9에서 삭제되었고 MacOS에서는 사용할 수 없습니다."
                );

            case Enums.CryptoType.CngCbc:
            case Enums.CryptoType.CngGcm:
                throw new PlatformNotSupportedException(
                    "CNG 기반 암호 알고리즘은 Windows 전용입니다. (MacOS에서는 지원 안됨)"
                );
        }
    }
}