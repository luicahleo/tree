using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.WorkerServices
{
    public interface IBaseWork: IDisposable
    {
        public Task RunWork();
    }
}
