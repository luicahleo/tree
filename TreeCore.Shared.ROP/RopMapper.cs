using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TreeCore.Shared.ROP
{
    public static class RopMapper
    {
        public static ResultDto<TResult> MapDto<TInput, TResult>(this Result<TInput> input, Func<TInput, TResult> mapper)
            where TResult : class
        {
            if (input.Success)
            {
                return new ResultDto<TResult>()
                {
                    Value = mapper(input.Value),
                    Errors = new List<ErrorDto>(),
                    Success = true,
                    TotalItems = input.TotalItems
                };
            }

            return new ResultDto<TResult>()
            {
                Value = null,
                Errors = input.Errors.Select(ErrorDto.FromError).ToList(),
                Success = false,
                TotalItems = input.TotalItems
            };
        }

        public static async Task<ResultDto<TResult>> MapDto<TInput, TResult>(this Task<Result<TInput>> input, Func<TInput, Task<TResult>> mapper)
            where TResult : class
        {

            var r = await input;

            if (r.Success)
            {
                return new ResultDto<TResult>()
                {
                    Value = await mapper(r.Value),
                    Errors = new List<ErrorDto>(),
                    Success = true,
                    TotalItems = 0
                };
            }

            return new ResultDto<TResult>()
            {
                Value = null,
                Errors = r.Errors.Select(ErrorDto.FromError).ToList(),
                Success = false,
                TotalItems = 0
            };
        }
    }

}
