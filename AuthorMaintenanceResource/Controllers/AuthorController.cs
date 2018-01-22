namespace AuthorMaintenanceResource.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuthorMaintenanceResource.DBContext;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AuthorController : Controller
    {
        private AuthorDBContext _authorDBContext;

        public AuthorController(AuthorDBContext authorDBContext)
        {
            _authorDBContext = authorDBContext;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_authorDBContext.Authors.AsQueryable());
        }
    }
}