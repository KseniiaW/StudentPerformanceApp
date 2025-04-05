using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Data;
using StudentPerformanceApp.Models;
using System.Text;

namespace StudentPerformanceApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();

            if (students.Any())
            {
                ViewBag.AverageScore = students.Average(s => s.Score);
                ViewBag.MaxScore = students.Max(s => s.Score);
                ViewBag.MinScore = students.Min(s => s.Score);
            }
            else
            {
                ViewBag.AverageScore = 0;
                ViewBag.MaxScore = 0;
                ViewBag.MinScore = 0;
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Group,Score")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Студент успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Group,Score")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Данные студента успешно обновлены";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Студент успешно удален";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/TopStudents
        public async Task<IActionResult> TopStudents()
        {
            var students = await _context.Students
                .OrderByDescending(s => s.Score)
                .Take(5)
                .ToListAsync();

            if (!students.Any())
            {
                ViewBag.Message = "Нет данных о студентах";
            }

            return View(students);
        }

        // GET: Students/WorstStudents
        public async Task<IActionResult> WorstStudents()
        {
            var worstStudents = await _context.Students
                .OrderBy(s => s.Score)
                .Take(5)
                .ToListAsync();

            return View(worstStudents);
        }

        // GET: Students/ExportToFile
        public async Task<IActionResult> ExportToFile()
        {
            var students = await _context.Students.ToListAsync();
            var sb = new StringBuilder();

            sb.AppendLine("Фамилия;Имя;Группа;Баллы");

            foreach (var student in students)
            {
                sb.AppendLine($"{student.LastName};{student.FirstName};{student.Group};{student.Score}");
            }

            var fileName = $"students_{DateTime.Now:yyyyMMddHHmmss}.csv";
            var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());

            return File(fileBytes, "text/csv", fileName);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}