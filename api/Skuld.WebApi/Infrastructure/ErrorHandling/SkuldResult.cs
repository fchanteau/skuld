using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Exceptions;
using System;
using System.Net;

namespace Skuld.WebApi.Infrastructure.ErrorHandling;

public class SkuldResult<T>
{
	private HttpStatusCode HttpStatusCode { get; }
	private SkuldExceptionType SkuldExceptionType { get; } = SkuldExceptionType.None;
	private object[] Parameters { get; } = Array.Empty<object> ();
	private T? Data { get; }

	private bool IsSuccess => Data is not null && SkuldExceptionType == SkuldExceptionType.None;
	private bool IsError => !IsSuccess;

	private SkuldResult (HttpStatusCode httpStatusCode, SkuldExceptionType skuldExceptionType, object[] parameters)
	{
		HttpStatusCode = httpStatusCode;
		SkuldExceptionType = skuldExceptionType;
		Parameters = parameters;
	}

	private SkuldResult (HttpStatusCode httpStatusCode, T data)
	{
		HttpStatusCode = httpStatusCode;
		Data = data;
	}

	public static SkuldResult<T> Success (HttpStatusCode httpStatusCode, T data)
	{
		return new SkuldResult<T> (httpStatusCode, data);
	}

	public static SkuldResult<T> Error (HttpStatusCode httpStatusCode, SkuldExceptionType skuldExceptionType, params object[] parameters)
	{
		return new SkuldResult<T> (httpStatusCode, skuldExceptionType, parameters);
	}

	public IActionResult Match (
		Func<HttpStatusCode, T, IActionResult> onSuccess,
		Func<HttpStatusCode, SkuldExceptionType, object[], IActionResult> onError)
	{
		return IsSuccess ? onSuccess (HttpStatusCode, Data) : onError (HttpStatusCode, SkuldExceptionType, Parameters);
	}

	public static SkuldResult<T> MapFromError<TSource> (SkuldResult<TSource> source)
	{
		return Error (source.HttpStatusCode, source.SkuldExceptionType, source.Parameters);
	}
}
