using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeAPI.DTO.Interfaces
{
    public class ListReturnTreeObject<T> : TreeObject
    {
        public int iPage;
        public int iTotalSize;
        public int iPageSize;
        public List<T> list;
    }
}