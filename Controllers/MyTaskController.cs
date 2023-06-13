
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Task.Services;
using Task.Interfaces;

namespace Task.Controller
{
    using Microsoft.Net.Http.Headers;
    using Task.Models;
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;

        public TaskController(ITaskService taskService)
        {
            this.TaskService = taskService;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<Task>> Get()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return TaskService.GetAll(TokenService.decode(token));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<Task> Get(int id)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var task = TaskService.Get(id, TokenService.decode(token));
            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        [Authorize(Policy = ("User"))]
        public ActionResult Post(Task t)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            t.UserId = TokenService.decode(token);
            TaskService.Post(t);
            return CreatedAtAction(nameof(Post), new { Id = t.Id }, t);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult Put(int id, [FromBody] Task task)
        {
            if (id != task.Id)
                return BadRequest("id <> task.Id");
            var res = TaskService.Update(task);
            if (!res)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            int userId = TokenService.decode(token);
            var task = TaskService.Get(id, userId);
            if (task == null)
                return NotFound();
            TaskService.Delete(id, userId);
            return NoContent();
        }

    }
}