﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.Service.Mappers
{
    public abstract class BaseMapper<DTO, Entity>
    {
        public abstract Task<DTO> Map(Entity entity);
    }
}
