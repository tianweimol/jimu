using Manphi.Business.Menu.IService;
using Manphi.DbOperate.EF;
using Manphi.DBOperate.Model;
using Manphi.DBOperate.Model.PermissionFolder;
using Manphi.DBOperate.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manphi.Business.Menu.Service
{
    public class CorporationService : BaseService<Corporation>, ICorporationService
    {
        private IRepository<Corporation> _corporationRepository;

        public CorporationService(IRepository<Corporation> corporationRepository) : base(corporationRepository)
        {
            _corporationRepository = corporationRepository;
        }

        public IEnumerable<Corporation> GetAllCorporations()
        {
            var re = GetListAll();
            return re;
        }
    }
}
