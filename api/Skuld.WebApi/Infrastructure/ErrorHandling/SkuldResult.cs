using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.ErrorHandling;

public class SkuldResult<T>
{
	private HttpStatusCode HttpStatusCode { get; }
	private SkuldErrorType SkuldErrorType { get; } = SkuldErrorType.None;
	private object?[] Parameters { get; } = Array.Empty<object> ();
	private T? Data { get; }

	private bool IsSuccess => Data is not null && SkuldErrorType == SkuldErrorType.None;
	private bool IsError => !IsSuccess;

	private SkuldResult (HttpStatusCode httpStatusCode, SkuldErrorType skuldErrorType, object?[] parameters)
	{
		HttpStatusCode = httpStatusCode;
		SkuldErrorType = skuldErrorType;
		Parameters = parameters;
	}

	private SkuldResult (HttpStatusCode httpStatusCode, T data)
	{
		HttpStatusCode = httpStatusCode;
		Data = data;
	}

	public static SkuldResult<T> Success (T data, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
	{
		return new SkuldResult<T> (httpStatusCode, data);
	}

	public static SkuldResult<T> Error (HttpStatusCode httpStatusCode, SkuldErrorType skuldExceptionType, params object?[] parameters)
	{
		return new SkuldResult<T> (httpStatusCode, skuldExceptionType, parameters);
	}

	public IActionResult Match (
		Func<HttpStatusCode, T, IActionResult> onSuccess,
		Func<HttpStatusCode, SkuldErrorType, object?[], IActionResult> onError)
	{
		return IsSuccess ? onSuccess (HttpStatusCode, Data!) : onError (HttpStatusCode, SkuldErrorType, Parameters);
	}

	public static SkuldResult<T> MapFromError<TSource> (SkuldResult<TSource> source)
	{
		return Error (source.HttpStatusCode, source.SkuldErrorType, source.Parameters);
	}

	public SkuldResult<TD> ContinueWith<TD> (Func<T, SkuldResult<TD>> lambda)
	{
		if (IsError)
		{
			return new SkuldResult<TD> (HttpStatusCode, SkuldErrorType, Parameters);
		}

		return lambda (Data!);
	}

	public SkuldResult<TD> ContinueWithAsync<TD> (Func<T, Task<SkuldResult<TD>>> lambda)
	{
		if (IsError)
		{
			return new SkuldResult<TD> (HttpStatusCode, SkuldErrorType, Parameters);
		}

		return lambda (Data!).Result;
	}
}
