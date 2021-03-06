﻿using Manphi.DbOperate.EF;
using Manphi.DBOperate.Model;
using Manphi.DBOperate.Model.PermissionFolder;

namespace Manphi.DBOperate.Service.PermissionFolder
{
    public class CorporationService : BaseService<Corporation>, ICorporationService
    {
        private IRepository<Corporation> _corporationRepository;

        public CorporationService(IRepository<Corporation> corporationRepository) : base(corporationRepository)
        {
            _corporationRepository = corporationRepository;
        }
    }
}
