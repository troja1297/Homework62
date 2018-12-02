using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Models.ViewModels;
using WebApplication5.Service;

namespace WebApplication5.Controllers
{

    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }

    public class InstitutionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private IHostingEnvironment _environment;
        private IMemoryCache _cache;
        private IMemoryCache _memoryCache;

        public InstitutionsController(ApplicationDbContext context, FileUploadService fileUploadService,
            IHostingEnvironment environment, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _environment = environment;
            _context = context;
            _fileUploadService = fileUploadService;
        }

        // GET: Institutions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Institution.ToListAsync());
        }

        // GET: Institutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution
                .FirstOrDefaultAsync(m => m.Id == id);
            var dishes = _context.Dish.Where(m => m.InstitutionId == id);
            Institution model = new Institution()
            {
                Description = institution.Description,
                Dishes = dishes,
                Id = institution.Id,
                PhotoPath = institution.PhotoPath,
                Title = institution.Title
            };
            if (institution == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Institutions/Create
        public IActionResult CreateDish()
        {
            return View();
        }

        // POST: Institutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDish([Bind("Id,Name,Description,Price")] Dish dish, Institution institution)
        {
            if (ModelState.IsValid)
            {
                institution.Dishes.Append(dish);
                _context.Add(institution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Institutions/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CacheGetOrCreateAsync()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync(CacheKeys.Entry, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                    return Task.FromResult(DateTime.Now);
                });

            return View("Cache", cacheEntry);
        }

        // POST: Institutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInstitutionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(
                    _environment.WebRootPath,
                    $"images\\{model.Title}\\");
                string photo = $"/images/{model.Title}/{model.File.FileName}";
                _fileUploadService.Upload(path, model.File.FileName, model.File);
                var institutionModel = new Institution()
                {
                    Description = model.Description,
                    Title = model.Title,
                    PhotoPath = photo
                };
                _context.Add(institutionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Institutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }
            return View(institution);
        }

        // POST: Institutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PhotoPath")] Institution institution)
        {
            if (id != institution.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionExists(institution.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(institution);
        }

        // GET: Institutions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institution = await _context.Institution
                .FirstOrDefaultAsync(m => m.Id == id);
            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        public string AddToTrash(int dishId)
        {
            Dish dish = _context.Dish.FirstOrDefault(t => t.Id == dishId);

            List<string> diList = new List<string>();
            diList.Add(dish.Name);
            diList.Add(dish.Price.ToString());
            string dishJson = JsonConvert.SerializeObject(diList);

            return dishJson;
        }

        // POST: Institutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var institution = await _context.Institution.FindAsync(id);
            _context.Institution.Remove(institution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionExists(int id)
        {
            return _context.Institution.Any(e => e.Id == id);
        }
    }
}
