using Jimu;
using Manphi.DBOperate.Model.PermissionFolder;
using Manphi.DBOperate.Service;
using System.Collections.Generic;

namespace Manphi.Business.Menu.IService
{
    [Jimu("/Menu/{Service}")]
    public interface ICorporationService: IJimuService, IBaseService<Corporation>
    {
        [JimuGet()]
        IEnumerable<Corporation> GetAllCorporations();
    }
}
