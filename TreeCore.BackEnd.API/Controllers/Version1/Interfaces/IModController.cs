using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers
{
    interface IModController<T>
    {
        public Task<ResultDto<T>> Post(T oDTO);
        public Task<ResultDto<T>> Put(T oDTO, string sCode);
    }
}
