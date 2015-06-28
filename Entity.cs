using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unit1
{
    class Entity<T> where T : Entity<T>
    {
        public void Save()
        {
            string data;
            var entity = this as T;

            entity.id = ++MaxId;

            if (this is Company)
            {
                var company = this as Company;
                data = String.Format("{1}{0}{2}{0}{3}",
                    PIPE, MaxId, company.Name, company.Address.ToString());
            }
            else if (this is Customer)
            {
                var customer = this as Customer;
                data = String.Format("{1}{0}{2}{0}{3}{0}{4}", PIPE, MaxId,
                    customer.FirstName, customer.LastName, customer.Address.ToString());
            }
            else
                return;

            WriteToFile(data);
        }

        public void Delete()
        {
            var entity = this as T;
            string line = null;

            if (entity.id == 0 || entity.id == null)
                return;

            using (StreamReader sr = new StreamReader(FullPath))
            {
                using (StreamWriter sw = new StreamWriter(FullPath + TEMP))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Split(PIPE)[ID] == entity.id.ToString())
                            continue;

                        sw.WriteLine(line);
                    }
                }
            }

            File.Delete(FullPath);
            File.Move(FullPath + TEMP, FullPath);

            if (entity.id == MaxId)
                MaxId = GetMaxId();

            entity.id = 0;
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
                string[] arrEntity, arrAddress;

                while (sr.Peek() >= 0 && int.Parse(nextId) <= int.Parse(id))
                {
                    arrEntity = sr.ReadLine().Split(PIPE);
                    nextId = arrEntity[ID];
                    arrAddress = arrEntity[arrEntity.Length-1].Split(USCORE);

                    if (nextId == id)
                    {
                        if (typeof(T) == typeof(Company))
                            return new Company(arrEntity[(int)COMPANY.NAME],
                                new Address(arrAddress[(int)ADDRESS.STREET],
                                    arrAddress[(int)ADDRESS.CITY], 
                                    arrAddress[(int)ADDRESS.STATE],
                                    arrAddress[(int)ADDRESS.ZIP]), id) as T;

                        else if (typeof(T) == typeof(Customer))
                            return new Customer(arrEntity[(int)CUSTOMER.FIRST_NAME], 
                                                arrEntity[(int)CUSTOMER.LASTNAME],
                                                new Address(arrAddress[(int)ADDRESS.STREET],
                                                    arrAddress[(int)ADDRESS.CITY], 
                                                    arrAddress[(int)ADDRESS.STATE],
                                                    arrAddress[(int)ADDRESS.ZIP]), id) as T;
                    }
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

        private int GetMaxId()
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

                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();

                        if (line == "" || !line.Contains(PIPE))
                            continue;

                        nextId = int.Parse(line.Split(PIPE)[ID]);
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

        private int MaxId
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

        private const char PIPE = '|';
        private const char USCORE = '_';
        private const int ID = 0;
        private enum CUSTOMER { ID, FIRST_NAME, LASTNAME, ADDRESS };
        private enum COMPANY { ID, NAME, ADDRESS };
        private enum ADDRESS { STREET, CITY, STATE, ZIP };
    }
}
