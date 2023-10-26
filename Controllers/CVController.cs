using CVApp.Data;
using CVApp.Models;
using CVApp.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CVApp.Controllers
{
    public class CVController : Controller
    {
        private readonly CVDbContext _db;
        private readonly ViewRenderService _viewRenderService;
        private const int _RECORDS_PER_PAGE = 10;

        public CVController(CVDbContext db, ViewRenderService viewRenderService)
        {
            _db = db;
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int skipRecords = (pageNumber - 1) * _RECORDS_PER_PAGE;

            var objCVList = _db.CVs
                               .Skip(skipRecords)
                               .Take(_RECORDS_PER_PAGE)
                               .ToList();

            ViewBag.TotalPages = Math.Ceiling((double)_db.CVs.Count() / _RECORDS_PER_PAGE);
            ViewBag.CurrentPage = pageNumber;

            return View(objCVList);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateCVViewModel
            {
                CV = new CV(),
                Schools = new List<School>(),
                Jobs = new List<Job>(),
                Skills = new List<Skill>(),
                Achievements = new List<Achievement>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCVViewModel viewModel)
        {
            if (viewModel.Schools.Count == 0 || viewModel.Jobs.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please add at least one school and one job.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.CVs.Add(viewModel.CV);
                        _db.SaveChanges();

                        foreach (var school in viewModel.Schools)
                        {
                            school.CVId = viewModel.CV.CVId;
                            _db.Schools.Add(school);
                        }

                        foreach (var job in viewModel.Jobs)
                        {
                            job.CVId = viewModel.CV.CVId;

                            foreach (var skill in viewModel.Skills)
                            {
                                skill.JobId = job.JobId;
                                _db.Skills.Add(skill);
                            }

                            foreach (var achievement in viewModel.Achievements)
                            {
                                achievement.JobId = job.JobId;
                                _db.Achievements.Add(achievement);
                            }

                            _db.Jobs.Add(job);
                        }

                        _db.SaveChanges();
                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        string errorMessage = "An error occurred while saving the data. " + ex.Message;
                        if (ex.InnerException != null)
                        {
                            errorMessage += " Inner Exception: " + ex.InnerException.Message;
                        }
                        ModelState.AddModelError("", errorMessage);
                    }
                }
            }

            return View(viewModel);
        }

        public IActionResult Edit(int? id)
        {
            var cvFromDb = _db.CVs
                .Include(cv => cv.Education)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Skills)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Achievements)
                .Where(cv => cv.CVId == id)
                .FirstOrDefault();

            CreateCVViewModel viewModel = new CreateCVViewModel
            {
                CV = cvFromDb,
                Schools = cvFromDb.Education.ToList(),
                Jobs = cvFromDb.Experience.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateCVViewModel obj)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.CVs.Update(obj.CV);
                        _db.SaveChanges();

                        foreach (var school in obj.Schools)
                        {
                            school.CVId = obj.CV.CVId;
                            _db.Schools.Update(school);
                        }

                        foreach (var job in obj.Jobs)
                        {
                            job.CVId = obj.CV.CVId;

                            foreach (var skill in obj.Skills)
                            {
                                skill.JobId = job.JobId;
                                _db.Skills.Update(skill);
                            }

                            foreach (var achievement in obj.Achievements)
                            {
                                achievement.JobId = job.JobId;
                                _db.Achievements.Update(achievement);
                            }

                            _db.Jobs.Update(job);
                        }

                        _db.SaveChanges();
                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        ModelState.AddModelError("", "An error occurred while saving the data.");
                    }
                }
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cvFromDb = _db.CVs
                .Include(cv => cv.Education)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Skills)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Achievements)
                .Where(cv => cv.CVId == id)
                .FirstOrDefault();

            if (cvFromDb == null)
            {
                return NotFound();
            }

            CreateCVViewModel viewModel = new CreateCVViewModel
            {
                CV = cvFromDb,
                Schools = cvFromDb.Education.ToList(),
                Jobs = cvFromDb.Experience.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cvFromDb = _db.CVs
                .Include(cv => cv.Education)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Skills)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Achievements)
                .Where(cv => cv.CVId == id)
                .FirstOrDefault();

            if (cvFromDb == null)
            {
                return NotFound();
            }

            foreach (var job in cvFromDb.Experience)
            {
                _db.Skills.RemoveRange(job.Skills);
                _db.Achievements.RemoveRange(job.Achievements);
            }

            _db.Schools.RemoveRange(cvFromDb.Education);
            _db.Jobs.RemoveRange(cvFromDb.Experience);

            _db.CVs.Remove(cvFromDb);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeneratePdf(int? id)
        {
            var cvFromDb = _db.CVs
                .Include(cv => cv.Education)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Skills)
                .Include(cv => cv.Experience)
                    .ThenInclude(job => job.Achievements)
                .Where(cv => cv.CVId == id)
                .FirstOrDefault();

            var html = await _viewRenderService.RenderToStringAsync("CVTemplate", cvFromDb);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 20, Bottom = 15 },
                DocumentTitle = "CV"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var converter = HttpContext.RequestServices.GetService<IConverter>();
            var file = converter.Convert(pdf);

            return File(file, "application/pdf");
        }

        public IActionResult PreviewPdf(int? id)
        {
            return View(id);
        }

        public IActionResult DownloadPdf()
        {
            if (TempData["PDF"] is byte[] file)
            {
                return File(file, "application/pdf", "CV.pdf");
            }
            else
            {
                return NotFound();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}