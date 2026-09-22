using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TableReserve.Exception.ExceptionsBase;

public class NotFoundException : TableReserveException
{
    private readonly string _message;
    public NotFoundException(string message) => _message = message;
    public override List<string> GetErrorMessages() => [_message];

    public override int StatusCode() => (int)HttpStatusCode.NotFound;
}