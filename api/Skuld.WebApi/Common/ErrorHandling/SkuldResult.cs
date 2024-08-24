using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Skuld.WebApi.Common.ErrorHandling;

public class SkuldResult<T>
{
	public HttpStatusCode HttpStatusCode { get; }
	public SkuldErrorType SkuldErrorType { get; } = SkuldErrorType.None;
	public object?[] Parameters { get; } = Array.Empty<object> ();
	public T? Data { get; }

	public bool IsSuccess => Data is not null && SkuldErrorType == SkuldErrorType.None;
	public bool IsError => !IsSuccess;

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
}
