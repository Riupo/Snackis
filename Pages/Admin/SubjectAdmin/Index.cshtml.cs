using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Pages.Admin.SubjectAdmin
{
    public class IndexModel : PageModel
    {
        private readonly Snackis.Data.ApplicationDbContext _context;

        public IndexModel(Snackis.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Subject> Subject { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Subject != null)
            {
                Subject = await _context.Subject.ToListAsync();
            }
        }
    }
}
