using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using TrovTver.Data;
using TrovTver.Models;

namespace TrovTver.Controllers;

public class TodoItemsController : Controller
{
    private readonly AppDbContext _context;

    public TodoItemsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TodoItems
    public async Task<IActionResult> Index()
    {
        // Order by not completed first, then by due date
        var items = await _context.TodoItems
                                  .OrderBy(t => t.IsDone)
                                  .ThenBy(t => t.DueDate)
                                  .ToListAsync();
        return View(items);
    }

    // GET: Hello
    public String Hello(String name)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}!");
    }


    // GET: TodoItems/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TodoItems/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,DueDate")] TodoItems todoItem)
    {
        if (ModelState.IsValid)
        {
            todoItem.CreatedDate = DateTime.UtcNow;
            todoItem.IsDone = false;
            _context.Add(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todoItem);
    }

    // GET: TodoItems/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }
        return View(todoItem);
    }

    // POST: TodoItems/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsDone,DueDate,CreatedDate")] TodoItems todoItem)
    {
        if (id != todoItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(todoItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(todoItem.Id))
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
        return View(todoItem);
    }

    // GET: TodoItems/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoItem = await _context.TodoItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todoItem == null)
        {
            return NotFound();
        }

        return View(todoItem);
    }

    // POST: TodoItems/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem != null)
        {
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TodoItemExists(int id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

    // POST: TodoItems/ToggleIsDone/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleIsDone(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.IsDone = !todoItem.IsDone; // Flip the boolean value
        _context.Update(todoItem);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
