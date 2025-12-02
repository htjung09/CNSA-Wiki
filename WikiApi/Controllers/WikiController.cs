using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiApi.Data;
using CNSAWiki.Models;

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
                                 .OrderBy(w => w.Title)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWikiPage(long id)
        {
            var page = await _context.WikiPages.FindAsync(id);
            if (page == null) return NotFound();

            _context.WikiPages.Remove(page);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
