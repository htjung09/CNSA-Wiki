using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiApi.Data;
using CNSAWiki.Models;
using Microsoft.AspNetCore.Authorization;

namespace WikiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WikiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WikiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/wiki
        [HttpGet]
        public async Task<ActionResult<List<WikiPage>>> GetWikiPages()
        {
            return await _context.WikiPages
                                 .OrderByDescending(w => w.UpdatedAt)
                                 .ToListAsync();
        }

        // GET: api/wiki/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WikiPage>> GetWikiPage(long id)
        {
            var page = await _context.WikiPages.FindAsync(id);
            if (page == null) return NotFound();
            return page;
        }

        // POST: api/wiki
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<WikiPage>> CreateWikiPage(WikiPage wikiPage)
        {
            wikiPage.CreatedAt = DateTime.UtcNow;
            wikiPage.UpdatedAt = DateTime.UtcNow;

            _context.WikiPages.Add(wikiPage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWikiPage), new { id = wikiPage.PageId }, wikiPage);
        }

        // PUT: api/wiki/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWikiPage(long id, WikiPage updatedPage)
        {
            if (id != updatedPage.PageId) return BadRequest();

            var page = await _context.WikiPages.FindAsync(id);
            if (page == null) return NotFound();

            page.Title = updatedPage.Title;
            page.Content = updatedPage.Content;
            page.AuthorId = updatedPage.AuthorId;
            page.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/wiki/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWikiPage(long id)
        {
            var page = await _context.WikiPages.FindAsync(id);
            if (page == null) return NotFound();

            _context.WikiPages.Remove(page);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/wiki/upload
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file" });

            string[] allowed = { ".png", ".jpg", ".jpeg", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowed.Contains(ext))
                return BadRequest(new { error = "Invalid file type" });

            var fileName = Guid.NewGuid().ToString("N") + ext;
            var uploadPath = Path.Combine("wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            // 웹에서 접근 가능한 URL
            string fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new { url = fileUrl });
        }
    }
}
