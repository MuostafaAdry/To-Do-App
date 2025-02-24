using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List.DataAccess;
using To_Do_List.Models;

namespace To_Do_List.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }


        public IActionResult AllToDo()
        {
            var ALLToDos = dbContext.ToDos;
            return View(ALLToDos.ToList());
        }

        [HttpGet]
        public IActionResult Create(ToDo toDo)
        {
            return View(new ToDo());
        }
        [HttpPost]
        public IActionResult Create(ToDo toDo, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ToDoFiles", fileName);
                //copy img in the wwwroot
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                //save img path in db

                toDo.File = fileName;
                dbContext.ToDos.Add(toDo);
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Task created successfully!";
                return RedirectToAction("AllToDo");
            }
            return View(toDo);


        }

        [HttpGet]
        public IActionResult Edit( int id)
        {
            var EditToDo = dbContext.ToDos.FirstOrDefault(e => e.Id == id);
            if (EditToDo!=null)
            {
                return View(EditToDo);
            }
            return RedirectToAction("NotFoundPage");
        }
        [HttpPost]
        public IActionResult Edit(ToDo toDo, IFormFile file)
        {
            if (toDo!=null)
            {
                var OldFileInWWWRoot = dbContext.ToDos.AsNoTracking().FirstOrDefault(e => e.Id == toDo.Id).File;
                //لو عدل كل حاجه و  الصوره
                if (file!=null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ToDoFiles", fileName);
                    //copy img in the wwwroot
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // wwwroot كده بيعدل الصوره عاوزين بقه نمسح لصوره القديمه من 
                    var OldPathFileInWWWRoot= Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ToDoFiles", OldFileInWWWRoot);
                    if (System.IO.File.Exists(OldPathFileInWWWRoot))
                    {
                        System.IO.File.Delete(OldPathFileInWWWRoot);
                    }
                    //save img path in db
                    toDo.File = fileName;
                }else
                    //لو عدل كل حاجه ومعدلش الصوره هخليه يرجع الصوره القديمه الل هى ف داتا بيز اصلا 
                    toDo.File = dbContext.ToDos.AsNoTracking().FirstOrDefault(e => e.Id == toDo.Id).File;

                 dbContext.ToDos.Update(toDo);
                dbContext.SaveChanges();
                return RedirectToAction("AllToDo");
            }
            return RedirectToAction("NotFoundPage");
        }



        public IActionResult Delete(int id)
        {
            var todo = dbContext.ToDos.FirstOrDefault(e => e.Id == id);
            if (todo != null)
            {
                //Delete old img from wwwroot
                if (todo.File != null)
                {
                    var OldPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ToDoFiles", todo.File);
                    if (System.IO.File.Exists(OldPath))
                    {
                        System.IO.File.Delete(OldPath);
                    }
                }

                dbContext.ToDos.Remove(todo);
                dbContext.SaveChanges();
                return RedirectToAction("AllToDo");

            }
            return RedirectToAction("NotFoundPage");
        }

    }
}
