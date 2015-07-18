using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;

namespace KNU_Schedule.Logic
{
    public class KSTimetable
    {
        KSSubject[,] timetable = new KSSubject[5, 4];
        public KSTimetable()
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if(isf.FileExists("Myfile.store")) 
            {
                readFromStorage();
            }
            isf.Dispose();
        }
        public KSSubject this[int day, int period]
        {
            get { return this.timetable[day, period]; }
            set { this.timetable[day, period] = value; }
        }
        public void Save()
        {
            saveDataToIsolatedStorage();
        }
        private void saveDataToIsolatedStorage()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile("MyFile.store"))
                {
                    XmlWriter writer = XmlWriter.Create(rawStream);
                    writer.WriteStartElement("timetable");
                    for (int i = 0; i < timetable.GetLength(0); i++ )
                    {
                        if (dayIsNotNull(i))
                        {
                            writer.WriteStartElement("day");
                            writer.WriteElementString("number", i.ToString());
                            for (int j = 0; j < timetable.GetLength(1); j++)
                            {
                                if (timetable[i, j] != null)
                                {
                                    writer.WriteStartElement("subject");
                                    writer.WriteElementString("period", j.ToString());
                                    writer.WriteElementString("name", timetable[i, j].Name);
                                    writer.WriteElementString("teacher", timetable[i, j].Teacher);
                                    writer.WriteElementString("room", timetable[i, j].Room.ToString());
                                    writer.WriteEndElement();
                                }
                            }
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                    writer.Close();
                }
            }
        }
        private bool dayIsNotNull(int day)
        {
            for (int i = 0; i < timetable.GetLength(1); i++ )
            {
                if (timetable[day, i] != null) return true;
            }
            return false;
        }
        private void readFromStorage()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists("Myfile.store"))
                {
                    using (IsolatedStorageFileStream rawStream = isf.OpenFile("Myfile.store", System.IO.FileMode.Open)) 
                    {
                        XDocument reader = XDocument.Load(rawStream);
                        foreach(XElement day in reader.Root.Elements())
                        {      
                            int i = 0;                    
                            foreach(XElement el in day.Elements())
                            {
                                
                                if (el.Name == "number") i = int.Parse(el.Value);
                                else if (el.Name == "subject")
                                {
                                    string name = "", teacher = "", room = "0"; int period = 0;
                                    foreach (XElement field in el.Elements())
                                    {                                        
                                        if (field.Name == "period") period = int.Parse(field.Value); else
                                        if (field.Name == "name") name = field.Value; else
                                        if(field.Name == "teacher") teacher = field.Value; else
                                        if(field.Name == "room") room = field.Value;
                                    }
                                    timetable[i, period] = new KSSubject(name, teacher, room);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}