namespace Services.StatusCode;

public enum CodeStatus
{
    Successful = 200,
    ErrorOnServer = 500,
    Empty = 404,
    ErrorWithData = 403
}
