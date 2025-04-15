namespace HeadphoneStore.Domain.Exceptions;

public enum ExceptionType
{
    Failure = 0,
    Unexpected = 1,
    Validation = 2,
    Conflict = 3,
    NotFound = 4,
    Unauthorized = 5,
    Forbidden = 6,
}

public abstract class DomainException : Exception
{
    public string Code { get; }

    public string Description { get; }

    public virtual ExceptionType Type { get; }

    protected DomainException(string code, string message) : base(message)
    {
        Code = code;
        Description = message;
    }

    public abstract class FailureException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Failure;

        protected FailureException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class UnexpectedException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Unexpected;

        protected UnexpectedException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class ValidationException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Validation;

        protected ValidationException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class ConflictException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Conflict;

        protected ConflictException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class NotFoundException : DomainException
    {
        public override ExceptionType Type => ExceptionType.NotFound;

        protected NotFoundException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class UnauthorizedException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Unauthorized;

        protected UnauthorizedException(string code, string message) : base(code, message)
        {
        }
    }

    public abstract class ForbiddenException : DomainException
    {
        public override ExceptionType Type => ExceptionType.Forbidden;

        protected ForbiddenException(string code, string message) : base(code, message)
        {
        }
    }
}
