using System.Security.Cryptography;
using System.Text;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Infrastructure.Security;

public sealed class VerificationCodeService
    : IVerificationCodeService
{
    private readonly byte[] _key;

    public VerificationCodeService(
        VerificationCodeSettings settings)
    {
        if (string.IsNullOrWhiteSpace(
                settings.HashKey))
        {
            throw new InvalidOperationException(
                "Verification hash key is missing.");
        }

        if (settings.HashKey.Length < 32)
        {
            throw new InvalidOperationException(
                "Verification hash key must contain at least 32 characters.");
        }

        _key =
            Encoding.UTF8.GetBytes(
                settings.HashKey);
    }

    public string GenerateCode()
    {
        var number =
            RandomNumberGenerator.GetInt32(
                0,
                1_000_000);

        return number.ToString("D6");
    }

    public string HashCode(
        string code)
    {
        using var hmac =
            new HMACSHA256(_key);

        var codeBytes =
            Encoding.UTF8.GetBytes(code);

        var hash =
            hmac.ComputeHash(codeBytes);

        return Convert.ToBase64String(
            hash);
    }

    public bool VerifyCode(
        string code,
        string codeHash)
    {
        try
        {
            using var hmac =
                new HMACSHA256(_key);

            var codeBytes =
                Encoding.UTF8.GetBytes(code);

            var calculatedHash =
                hmac.ComputeHash(codeBytes);

            var expectedHash =
                Convert.FromBase64String(
                    codeHash);

            return CryptographicOperations
                .FixedTimeEquals(
                    calculatedHash,
                    expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}