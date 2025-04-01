namespace TimeTable.Models
{
    public class TimeTableModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }        
        public List<Subject> Subjects { get; set; }
    }

}
