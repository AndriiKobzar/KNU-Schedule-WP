using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNU_Schedule.Logic
{
    public class KSGroup
    {
            private string id;
            public string Id
            {
                get
                {
                    return this.id;
                }
                set
                {
                    this.id = value;
                }
            }
            private string name;
            public string Name { 
                get
                {
                    return this.name;

                }
                set
                {
                    this.name = value;
                }
            }
            private string facultyId;
            public string FacultyId 
            { 
                get
                {
                    return this.facultyId;
                }
                set
                {
                    this.facultyId = value;
                }
            }
    }
}
