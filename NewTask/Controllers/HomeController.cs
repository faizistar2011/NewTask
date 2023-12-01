using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewTask.DAL;
using NewTask.Models;
using System.Diagnostics;

namespace NewTask.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            var data = _context.Users
                .Select(x => new AppUser
                {
                    UserName = x.UserName,
                    Email = x.Email,
                    Id = x.Id
                }).ToList();
            var Roles = _context.UserRoles
                .Join(_context.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
                .ToList();
            foreach(var item in data)
            {
                item.Role = Roles.Where(x => x.u.UserId == item.Id).FirstOrDefault()?.r.Name;
            }
                //.GroupJoin(_context.UserRoles,u => u.Id,r => r.UserId,(u,r) => new { u, r = r.FirstOrDefault()})
                //.GroupJoin(_context.Roles,rr => rr.r.RoleId,rn => rn.Id,(rr,rn) => new { rr,rn = rn.FirstOrDefault()})
                //.Select(x => new AppUser
                //{
                //    UserName = x.rr.u.UserName,
                //    Email = x.rr.u.Email,
                //    Role = x.rn.Name
                //})
                //.ToList();

            return View(data);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
