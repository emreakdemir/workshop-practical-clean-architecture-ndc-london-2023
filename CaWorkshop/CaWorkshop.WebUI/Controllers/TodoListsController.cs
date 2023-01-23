using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CaWorkshop.WebUI.Data;
using CaWorkshop.WebUI.Models;

namespace CaWorkshop.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            var b = await _context.TodoLists
                                  .Include(a => a.Items)
                                  .ToListAsync();
            var c = await _context.TodoLists
                                  .Include(a => a.Items).AsSplitQuery()
                                  .ToListAsync();
            var a = await _context.TodoLists
                                  .Select(l => new TodoList
                                   {
                                       Id = l.Id,
                                       Title = l.Title,
                                       Items = l.Items.Select(i => new TodoItem
                                       {
                                           Id = i.Id,
                                           ListId = i.ListId,
                                           Title = i.Title,
                                           Done = i.Done,
                                           Priority = i.Priority,
                                           Note = i.Note,
                                       }).ToList(),
                                   }).ToListAsync();

            return a;
        }

        // PUT: api/TodoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTodoList(int id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return BadRequest();
            }

            _context.TodoLists.Update(todoList);
            //_context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/TodoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostTodoList(TodoList todoList)
        {
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return todoList.Id;
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoListExists(int id)
        {
            return _context.TodoLists.Any(e => e.Id == id);
        }
    }
}