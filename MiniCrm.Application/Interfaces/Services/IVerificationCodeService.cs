namespace MiniCrm.Application.Interfaces.Services;

public interface IVerificationCodeService
{
    string GenerateCode();

    string HashCode(string code);

    bool VerifyCode(
        string code,
        string codeHash);
}