namespace KNU_Schedule.Logic
{
    public class KSTeacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middle_Name { get; set; }
        public override string ToString()
        {
            return Surname + ' ' + Name[0] + ". " + Middle_Name[0] + ".";
        }
    }
}