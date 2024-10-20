using cinema_web_app.Data;
using cinema_web_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cinema_web_app.Controllers;

public class ScreeningRoomsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ScreeningRoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ScreeningRooms
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.ScreeningRooms.Include(s => s.Cinema);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: ScreeningRooms/Details/5
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var screeningRoom = await _context.ScreeningRooms
            .Include(s => s.Cinema)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (screeningRoom == null) return NotFound();

        return View(screeningRoom);
    }

    // GET: ScreeningRooms/Create
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public IActionResult Create()
    {
        ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
        return View();
    }

    // POST: ScreeningRooms/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Create([Bind("Id,CinemaId,Name,TotalNoOfSeats,Is3D")] ScreeningRoom screeningRoom)
    {
        ModelState.Remove(nameof(ScreeningRoom.Cinema));
        ModelState.Remove(nameof(ScreeningRoom.Screenings));

        if (ModelState.IsValid)
        {
            screeningRoom.Id = Guid.NewGuid();
            _context.Add(screeningRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screeningRoom.CinemaId);
        return View(screeningRoom);
    }

    // GET: ScreeningRooms/Edit/5
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var screeningRoom = await _context.ScreeningRooms.FindAsync(id);
        if (screeningRoom == null) return NotFound();
        ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screeningRoom.CinemaId);
        return View(screeningRoom);
    }

    // POST: ScreeningRooms/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Id,CinemaId,Name,TotalNoOfSeats,Is3D")] ScreeningRoom screeningRoom)
    {
        ModelState.Remove(nameof(ScreeningRoom.Cinema));
        ModelState.Remove(nameof(ScreeningRoom.Screenings));

        if (id != screeningRoom.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(screeningRoom);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreeningRoomExists(screeningRoom.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screeningRoom.CinemaId);
        return View(screeningRoom);
    }

    // GET: ScreeningRooms/Delete/5
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var screeningRoom = await _context.ScreeningRooms
            .Include(s => s.Cinema)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (screeningRoom == null) return NotFound();

        return View(screeningRoom);
    }

    // POST: ScreeningRooms/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ApplicationAdmin, ContentAppAdmin")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var screeningRoom = await _context.ScreeningRooms.FindAsync(id);
        if (screeningRoom != null) _context.ScreeningRooms.Remove(screeningRoom);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ScreeningRoomExists(Guid id)
    {
        return _context.ScreeningRooms.Any(e => e.Id == id);
    }
}