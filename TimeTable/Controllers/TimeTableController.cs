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
            //Calculate total hours of the week
            int totalHours = model.WorkingDays * model.SubjectsPerDay;

            //Validation total hours of the week eqals to sum of subject hours
            if (totalHours != model.Subjects.Sum(x => x.Hours))
            {
                return BadRequest(new { ErrorMessage = "Total hours of subject must be equal to Total hours of week" });
            }
            var subjectList = new List<string>();

            //based on subject and hours create subject list
            foreach (var subject in model.Subjects)
            {
                for (int i = 0; i < subject.Hours; i++)
                {
                    subjectList.Add(subject.Name);
                }
            }

            //creating two dimensional array using subjects per day and working days
            var timetable = new string[model.SubjectsPerDay, model.WorkingDays];
            int index = 0;

            //looping subjects per day
            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                //after working days loop
                for (int j = 0; j < model.WorkingDays; j++)
                {
                    //checking index and count
                    if (index < subjectList.Count)
                    {
                        //maaping subject name to two dimensional array
                        timetable[i, j] = subjectList[index++];
                    }
                }
            }

            //serializing data of two dimensional array for returning data
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
