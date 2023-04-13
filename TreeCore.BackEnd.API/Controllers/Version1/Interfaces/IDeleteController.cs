using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers
{
    interface IDeleteController<T>
    {
        Task<ResultDto<T>> Delete(string code);
    }
}
