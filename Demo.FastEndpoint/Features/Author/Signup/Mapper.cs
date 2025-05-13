using System.Globalization;

namespace Author.Signup;

internal sealed class Mapper : Mapper<Request, Response, Dom.Author>
{
    static readonly CultureInfo _cultureInfo = new CultureInfo("en-US");

    public override Dom.Author ToEntity(Request r) => new()
    {
        Email = r.Email.ToLower(),
        FirstName = _cultureInfo.TextInfo.ToTitleCase(r.FirstName),
        LastName = _cultureInfo.TextInfo.ToTitleCase(r.LastName),
        PasswordHash = r.Password,
        SignUpDate = DateOnly.FromDateTime(DateTime.UtcNow),
        UserName = r.UserName,
    };

}