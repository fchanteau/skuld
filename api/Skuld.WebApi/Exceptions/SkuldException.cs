using System;
using System.Net;

namespace Skuld.WebApi.Exceptions
{
	public class SkuldException : Exception
	{
		public HttpStatusCode HttpStatusCode { get; }

		public SkuldExceptionType SkuldExceptionType { get; }

		public string?[] Parameters { get; }

		public SkuldException (HttpStatusCode httpStatusCode, SkuldExceptionType skuldExceptionType, params string?[] parameters)
		{
			HttpStatusCode = httpStatusCode;
			SkuldExceptionType = skuldExceptionType;
			Parameters = parameters;
		}
	}
}
