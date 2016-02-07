using System.Collections.Generic;

namespace KNU_Schedule.Logic
{
    public class KSFacultyResponse
    {
        public int Status { get; set; }
        public List<KSFaculty> Result { get; set; }
        public int Error { get; set; }
    }

    public class KSFaculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
