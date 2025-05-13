namespace Author.Signup;

internal sealed class Request
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;


    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("first name is required!")
                .MinimumLength(3).WithMessage("name is too short")
                .MaximumLength(25).WithMessage("name is too long");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email address is required!")
                .EmailAddress().WithMessage("the formar of your email address is wrong");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password can't be blank");

        }
    }
}

internal sealed class Response
{
    public string Message { get; set; } = "This endpoint hasn't been implemented yet!";
}
