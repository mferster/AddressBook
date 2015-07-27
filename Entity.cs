using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Unit1
{
    class Entity<T> where T : Entity<T>
    {
        public void Save()
        { 
            Dictionary<string, object> properties = new Dictionary<string, object>();

            this.id = ++MaxId; 

            foreach (var property in this.GetType().GetProperties())
                properties.Add(property.Name, property.GetValue(this, null));

            WriteToFile(JsonConvert.SerializeObject(properties, Formatting.None));
        }

        public void Delete()
        {
            string line = null;

            if (this.id == 0)
                return;

            using (StreamReader sr = new StreamReader(FullPath))
            {
                using (StreamWriter sw = new StreamWriter(FullPath + TEMP))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (JsonConvert.DeserializeObject<T>(line).Id == this.id.ToString())
                            continue;

                        sw.WriteLine(line);
                    }
                }
            }

            File.Delete(FullPath);
            File.Move(FullPath + TEMP, FullPath);

            if (this.id == MaxId)   
                MaxId = GetMaxId();

            this.id = 0;          
        }

        public static T Find(string id)
        {
            if (!System.IO.Directory.Exists(LOCAL_FILE_PATH) ||
                !System.IO.File.Exists(FullPath) ||
                id == "")
                return null;

            using (StreamReader sr = new StreamReader(FullPath))
            {
                string nextId = "0";
                
                T deserialize;

                while (sr.Peek() >= 0 && int.Parse(nextId) <= int.Parse(id))
                {
                    deserialize = JsonConvert.DeserializeObject<T>(sr.ReadLine());
                    nextId = deserialize.Id; 

                    if (nextId == id)
                        return deserialize;     
                }
            }
            return null;
        }

        public string Id
        {
            get {return id == 0? "": id.ToString();}
            set { id = int.Parse(value); }
        }

        private void WriteToFile(string data)
        {
            if (!System.IO.Directory.Exists(LOCAL_FILE_PATH))
                System.IO.Directory.CreateDirectory(LOCAL_FILE_PATH);

            using(StreamWriter sw = new StreamWriter(FullPath, true))
            {
                sw.WriteLine(data);
            }
        }

        private static int GetMaxId()
        {
            int maxid = 0;

            if (!System.IO.Directory.Exists(LOCAL_FILE_PATH) ||
                !System.IO.File.Exists(FullPath))
                return maxid;
            try
            {
                using (StreamReader sr = new StreamReader(FullPath))
                {
                    int nextId;
                    string line = null;
                    T deserialize;

                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();

                        if (line == "")
                            continue;

                        deserialize = JsonConvert.DeserializeObject<T>(line);

                        nextId = int.Parse(deserialize.Id);
                        maxid = nextId > maxid ? nextId : maxid;
                    }
                }

                return maxid;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        private static int MaxId
        {
            get { return maxId == 0 ? GetMaxId() : maxId; }
            set { maxId = value; }
        }

        private static string FullPath { get { return LOCAL_FILE_PATH + entityName + FILE_TYPE; } }

        private int id;
        private static int maxId;

        private const string LOCAL_FILE_PATH = "c:\\storage\\";
        private const string FILE_TYPE = ".txt";
        private const string TEMP = "_TEMP";
        private static string entityName = typeof(T).Name;
    }
}
