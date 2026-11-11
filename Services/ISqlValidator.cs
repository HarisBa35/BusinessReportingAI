// Services/ISqlValidator.cs
namespace BusinessReportingAI.Services;

public interface ISqlValidator
{
    (bool IsValid, string ErrorMessage) Validate(string sql);
}