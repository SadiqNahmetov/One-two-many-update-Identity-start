using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index() => View(await _context.Employees.AsNoTracking().ToListAsync());
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int id)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (employee is null) return NotFound();

            if (employee.IsActive)
            {
                employee.IsActive = false;
            }
            else
            {
                employee.IsActive = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
