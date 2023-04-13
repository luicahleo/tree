using System.Collections.Generic;
using TreeAPI.DTO.Interfaces;

namespace TreeAPI.API.TBO.Interfaces
{
    public interface ICollective
    {
        TBOResponse _CI_Create(TreeDataObject Element);
        TreeDataObject _CI_FindByPrimaryKey(long lIdentifier);
        TBOResponse _CI_FindByPrimaryCode(string sIdentifier);
        List<TreeDataObject> _CI_FindAll(bool bActive, int iMax);
        TBOResponse _CI_Remove(long lIdentifier);
        TBOResponse _CI_Enable(long lIdentifier);
        TBOResponse _CI_Disable(long lIdentifier);
        TBOResponse _CI_Update(TreeDataObject Element);

    }
}
