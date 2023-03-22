using System.Globalization;
using System.Text.RegularExpressions;

namespace api.Services;

public interface IEmailVerificationService
{
    public bool IsValidEmail(string email);
}

public class EmailVerificationService : IEmailVerificationService
{
    public bool IsValidEmail(string email)
    {
        // Check if email is null, empty or whitespace
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Replace the domain part of the email with its ASCII representation
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
        }
        catch (RegexMatchTimeoutException)
        {
            // If the regex operation times out, return false
            return false;
        }
        catch (ArgumentException)
        {
            // If the input is invalid, return false
            return false;
        }

        try
        {
            // Check if the email matches the specified regex pattern
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            // If the regex operation times out, return false
            return false;
        }
    }

    private string DomainMapper(Match match)
    {
        // Create a new instance of the IdnMapping class to handle the domain name
        var idn = new IdnMapping();

        // Get the ASCII representation of the domain name
        string domainName = idn.GetAscii(match.Groups[2].Value);

        // Return the original '@' symbol and the ASCII domain name
        return match.Groups[1].Value + domainName;
    }
}