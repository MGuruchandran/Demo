using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace Author.Signup;

internal sealed class Endpoint : Endpoint<Request, Results<Ok<Response>,NotFound,Conflict,UnauthorizedHttpResult,ProblemDetails>, Mapper>
{
    public override void Configure()
    {
        Post("/author/signup");
        AllowAnonymous();
        //Idempotency();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var author = Map.ToEntity(r);
        var emailsTaken = await Data.EmailAddressIsTaken(author.Email);
        if (emailsTaken)
        {
            AddError(r => r.Email, "Email is already taken");
        }
        var userNameTaken = await Data.UserNameIsTaken(author.UserName.ToLower());
        if (userNameTaken) AddError(r=>r.UserName,"Username is not available");
        ThrowIfAnyErrors();
        await Data.CreateNewAuthor(author);
        await SendAsync(TypedResults.Ok(new Response() { Message = $"{r.FirstName} has been created with the username {r.UserName}" }));
    }
}