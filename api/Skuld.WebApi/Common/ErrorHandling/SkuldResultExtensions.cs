using System;
using System.Threading.Tasks;

namespace Skuld.WebApi.Common.ErrorHandling;

public static class SkuldResultExtensions
{
	// chain synchrone => synchrone
	public static SkuldResult<TOutput> Then<TInput, TOutput> (this SkuldResult<TInput> skuldResult, Func<TInput, SkuldResult<TOutput>> func)
	{
		return skuldResult.IsSuccess ? func (skuldResult.Data!) : SkuldResult<TOutput>.MapFromError (skuldResult);
	}

	// chain synchrone => asynchrone
	public async static Task<SkuldResult<TOutput>> ThenAsync<TInput, TOutput> (this SkuldResult<TInput> skuldResult, Func<TInput, Task<SkuldResult<TOutput>>> func)
	{
		return skuldResult.IsSuccess ? await func (skuldResult.Data!) : SkuldResult<TOutput>.MapFromError (skuldResult);
	}

	// chain asynchrone => synchrone
	public async static Task<SkuldResult<TOutput>> Then<TInput, TOutput> (this Task<SkuldResult<TInput>> skuldResult, Func<TInput, SkuldResult<TOutput>> func)
	{
		var result = await skuldResult;
		return result.IsSuccess ? func (result.Data!) : SkuldResult<TOutput>.MapFromError (result);
	}

	// chain asynchrone => asynchrone
	public async static Task<SkuldResult<TOutput>> ThenAsync<TInput, TOutput> (this Task<SkuldResult<TInput>> skuldResult, Func<TInput, Task<SkuldResult<TOutput>>> func)
	{
		var result = await skuldResult;
		return result.IsSuccess ? await func (result.Data!) : SkuldResult<TOutput>.MapFromError (result);
	}

	public static SkuldResult<TInput> Tap<TInput> (this SkuldResult<TInput> skuldResult, Func<TInput, SkuldResult<TInput>> func)
	{
		if (skuldResult.IsSuccess)
		{
			func (skuldResult.Data!);
		}

		return skuldResult;
	}
}
