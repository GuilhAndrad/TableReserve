namespace TableReserve.Communication.Responses;

public class ErrorResponse
{
    public List<string> Errors { get; private set; } = [];
    public bool AccestokenExpired { get; private set; }

    public ErrorResponse(List<string> errorsMessages) => Errors = errorsMessages;

    public ErrorResponse(string errorMessage) => Errors = [errorMessage];

    public ErrorResponse(string errorMessage, bool accestokenExpired)
    {
        Errors = [errorMessage];
        AccestokenExpired = accestokenExpired;
    }
}
