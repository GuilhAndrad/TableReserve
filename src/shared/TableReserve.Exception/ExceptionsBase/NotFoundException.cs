using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TableReserve.Exception.ExceptionsBase;

public class NotFoundException(string message) : TableReserveException
{
    private readonly string _message = message;

    public override List<string> GetErrorMessages() => [_message];

    public override int StatusCode() => (int)HttpStatusCode.NotFound;
}