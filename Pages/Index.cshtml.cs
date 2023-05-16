using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis.Models;
using System.Reflection.Metadata;
using System.Security.Claims;


namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        public IndexModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Subject> Subjects { get; set; }
        public List<Models.Post> Posts { get; set; }
        [BindProperty]
        public Models.Post Post { get; set; }
        [BindProperty]
        public bool IsAnonymous { get; set; }
		public bool ShowForm { get; set; }
		public IFormFile UploadedImage { get; set; }
        public async Task<IActionResult> OnGetAsync(int deleteid, bool showForm = false)
        {
			ShowForm = showForm;
			Posts = await _context.Post.OrderByDescending(p => p.Date).ToListAsync();
            Subjects = await _context.Subject.ToListAsync();
            if (deleteid != 0)
            {

                Models.Post blog = await _context.Post.FindAsync(deleteid);

                if (blog != null)
                {
                    if (System.IO.File.Exists("./wwwroot/img/" + blog.Image))
                    {
                        System.IO.File.Delete("./wwwroot/img/" + blog.Image);
                    }
                    _context.Post.Remove(blog);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
            }

            Posts = await _context.Post.ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string filename = string.Empty;
            if (UploadedImage != null)
            {
                Random rnd = new();
                filename = rnd.Next(0, 100000).ToString() + UploadedImage.FileName;
                var file = "./wwwroot/img/" + filename;
                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(filestream);
                }
            }

            Post.Date = DateTime.Now;
            Post.Image = filename;

            if (IsAnonymous)
            {
                Post.UserName = null;
            }
            else
            {
                Post.UserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            _context.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
		public IActionResult OnPostToggleForm(bool showForm)
		{
			ShowForm = !showForm;
			return RedirectToPage(new { ShowForm });
		}
	}
}