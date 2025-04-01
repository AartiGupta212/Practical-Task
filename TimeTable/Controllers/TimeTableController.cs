using Microsoft.AspNetCore.Mvc;
using TimeTable.Models;

namespace TimeTable.Controllers
{
    public class TimeTableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateTimeTable([FromBody] TimeTableModel model)
        {
            int totalHours = model.WorkingDays * model.SubjectsPerDay;
            if (totalHours != model.Subjects.Sum(x => x.Hours))
            {
                return BadRequest(new { ErrorMessage = "Total hours of subject must be equal to Total hours of week" });
            }
            var subjectList = new List<string>();

            foreach (var subject in model.Subjects)
            {
                for (int i = 0; i < subject.Hours; i++)
                {
                    subjectList.Add(subject.Name);
                }
            }

            var timetable = new string[model.SubjectsPerDay, model.WorkingDays];
            int index = 0;

            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                for (int j = 0; j < model.WorkingDays; j++)
                {
                    if (index < subjectList.Count)
                    {
                        timetable[i, j] = subjectList[index++];
                    }
                }
            }

            var serializedTimetable = new List<List<string>>();
            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < model.WorkingDays; j++)
                {
                    row.Add(timetable[i, j]);
                }
                serializedTimetable.Add(row);
            }

            return Json(new { TimeTable = serializedTimetable });
        }
    }
}
