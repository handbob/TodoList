namespace TodoList.Controllers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase {
    private readonly TodoContext context;
    public TodoItemsController(TodoContext context) {
        this.context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() {
        return await this.context.TodoItems.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id) {
        var todoItem = await this.context.TodoItems.FindAsync(id);
        if (todoItem == null) {
            return NotFound();
        }
        return todoItem;
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem) {
        if (id != todoItem.Id) {
            return BadRequest();
        }

        this.context.Entry(todoItem).State = EntityState.Modified;

        try {
            await this.context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!TodoItemExists(id)) {
                return NotFound();
            }
            else {
                throw;
            }
        }
        return NoContent();
    }
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) {
        this.context.TodoItems.Add(todoItem);
        await this.context.SaveChangesAsync();

        return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id) {
        var todoItem = await this.context.TodoItems.FindAsync(id);
        if (todoItem == null) {
            return NotFound();
        }
        this.context.TodoItems.Remove(todoItem);
        await this.context.SaveChangesAsync();
        return NoContent();
    }
    private bool TodoItemExists(long id) {
        return this.context.TodoItems.Any(e => e.Id == id);
    }
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO {
        Id = todoItem.Id,
        Name = todoItem.Name,
        IsComplete = todoItem.IsComplete
    };
};
