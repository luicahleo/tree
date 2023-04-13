using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.WorkerServices
{
    public static class WorksRepository
    {
        private static Hashtable WorksList = new Hashtable();

        public static Task ClearList() {
            foreach (IDisposable item in WorksList)
            {
                item.Dispose();
            }
            return Task.CompletedTask;
        }

        public static Task AddWork(object key, IBaseWork work) {
            if (WorksList[key] == null)
            {
                WorksList[key] = work;
                work.RunWork();
            }
            return Task.CompletedTask;
        }

        public static IBaseWork RemoveWork(object key) {
            IBaseWork oWork = null;
            if (WorksList[key] != null)
            {
                oWork = (IBaseWork)WorksList[key];
                oWork.Dispose();
                WorksList[key] = null;
            }
            return oWork;
        }

        public static IBaseWork GetWork(object key) {
            IBaseWork oWork = null;
            if (WorksList[key] != null)
            {
                oWork = (IBaseWork)WorksList[key];
            }
            return oWork;
        }
    }
}
