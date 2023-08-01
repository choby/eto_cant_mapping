using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evo.Scm.Positions
{
    public interface IPositionSevice
    {
     
        Task<IEnumerable<UserSelectOptionDto>> GetUserSelectOptionFromPosistion(SearchUserFromPosistionDto input);
    }
}
