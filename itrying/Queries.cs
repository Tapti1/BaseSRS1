using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using Bogus;
using MySqlX.XDevAPI.Relational;

namespace itrying
{
    internal class Queries
    {

        Faker faker = new Faker("ru");
        MySqlConnection conn;

        int max_cat,max_breed,max_pride,max_family,max_owner, max_nickname, max_age;
        

        public Queries(MySqlConnection conn)
        {
            this.conn = conn;
        }

        public void DELECTE_ALL()
        {
            string sql = $"drop table if exists cat_owner,owner_family,cat,breed,pride,owner,family";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            string line;sql = "";
            StreamReader sr = new StreamReader("CreateTablesNew.sql");
            line = sr.ReadLine();    //читаем запрос из файла
            while (line != null)
            {
                sql = sql + "\n" + line;
                line = sr.ReadLine();
            }
            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            sr.Close();
        }     
        
        public void SetMax()
        {
            max_cat = faker.Random.Number(30, 50);
            max_owner = faker.Random.Number(40, 50);
            max_family = faker.Random.Number(15, 30);
            max_breed = faker.Random.Number(30,50);
            max_pride = faker.Random.Number(15,30);
            max_nickname = faker.Random.Number(max_cat, 60);
            max_age= faker.Random.Number(20, 30);
        }

        public string breed()
        {
            string s,r="";      
            Dictionary<string,int> d = new Dictionary<string, int>();//чтобы без повторов
            StreamReader sr = new StreamReader("breeds.txt");
            while (d.Count()<max_breed)
            {                
                s = sr.ReadLine();
                d[s] = 1;
            }
            sr.Close();
            r =r+"INSERT INTO breed(title) VALUES ";    //сам запрос
            r = r + ($"('сфинкс')");
            foreach (var person in d)
            {
                s = person.Key;
                r = r + ($",('{s}')");
            }           
            
            return r;
        }
        public string pride()
        {
            string s, r = "";      //чтобы без повторов
            Dictionary<string, int> d = new Dictionary<string, int>();
            
            while (d.Count() < max_pride)
            {
                s = faker.Name.LastName();
                d[s] = 1;
            }

            r = r + "INSERT INTO pride(title) VALUES ";    //сам запрос
            
            r = r + ($"('Бесславные ублюдки')");
            foreach (var person in d)
            {
                s = person.Key;
                r = r + ($",('{s}')");
            }

            
            return r;
        }
        public string family()
        {
            string s, r = "";      //чтобы без повторов
            Dictionary<string, int> d = new Dictionary<string, int>();
            
            while (d.Count() < max_family)
            {
                s = faker.Name.LastName();
                d[s] = 1;
            }

            r = r + "INSERT INTO family(title) VALUES ";    //сам запрос

            r = r + ($"('Энгельс'),");
            r = r + ($"('Рабинович')");
            foreach (var person in d)
            {
                s = person.Key;
                r = r + ($",('{s}')");
            }


            return r;
        }
        public string owner()
        {
            string s, r = "";      //чтобы без повторов
            Dictionary<string, int> d = new Dictionary<string, int>();
            while (d.Count() < max_owner)
            {
                s = faker.Name.FullName();
                d[s] = 1;
            }

            r = r + "INSERT INTO owner(name) VALUES ";    //сам запрос
            r = r + ($"('Карл Маркс')");
            foreach (var person in d)
            {
                s = person.Key;
                r = r + ($",('{s}')");
            }

            
            return r;
        }
        public string cat()
        {
            string s, r = "";      
            List<string> nick = new List<string>();            
            StreamReader sr = new StreamReader("nicknames.txt");
            while (nick.Count() < max_nickname)
            {                
                s = sr.ReadLine();
                nick.Add(s);
                for(int i = 0; i < faker.Random.Number(4); i++)
                {
                    if (nick.Count() < max_nickname)
                    {
                        nick.Add(s);
                    }
                }
            }
            sr.Close();
            r = r + "INSERT INTO cat(nickname,breed_id,pride_id,age) VALUES ";    //сам запрос
            for(int i = 0; i <= max_cat; i++)
            {
                r = r + ($"('{nick[i]}','{i % max_breed+1}','{ i % max_pride+1}','{faker.Random.Number(max_age)+1}')");
                if (i != max_cat) r = r + ",";
            }
            return r;            
        }
        public string cat_owner()
        {
            int rand2 = faker.Random.Number(2, 5);
            int rand3 = faker.Random.Number(5, 10);

            string r = "";
            
            r = r + "INSERT INTO cat_owner(cat_id,owner_id) VALUES "; 
            for(int i = 0; i < rand2; i++)  
            {
                int random_cat=faker.Random.Number(1,max_cat);
                int random_owner1=faker.Random.Number(max_owner), random_owner2 = faker.Random.Number(max_owner);
                
                while (random_owner2 == random_owner1)
                {
                    random_owner2 = faker.Random.Number(1,max_owner);
                }
                
                if (r.IndexOf($"('{random_cat}','{random_owner1}')") != -1 || r.IndexOf($"('{random_cat}','{random_owner2}')") != -1)
                {
                    i++;
                    continue;
                }                
                r = r + ($"('{random_cat}','{random_owner1}'),");
                r = r + ($"('{random_cat}','{random_owner2}'),");
            }

            for (int i = 0; i < rand2; i++)
            {
                int random_cat = faker.Random.Number(1,max_cat);
                int random_owner1 = faker.Random.Number(1,max_owner), random_owner2 = faker.Random.Number(1, max_owner);
                
                while (random_owner2 == random_owner1)
                {
                    random_owner2 = faker.Random.Number(1, max_owner);
                }
                
                int random_owner3 = faker.Random.Number(1, max_owner);
                while (random_owner3 == random_owner1 || random_owner2 == random_owner3)
                {
                    random_owner3 = faker.Random.Number(1, max_owner);
                }
                
                if (r.IndexOf($"('{random_cat}','{random_owner1}')") != -1 || r.IndexOf($"('{random_cat}','{random_owner2}')") != -1 || r.IndexOf($"('{random_cat}','{random_owner3}')") != -1)
                {
                    i++;
                    continue;
                }
                r = r + ($"('{random_cat}','{random_owner1}'),");
                r = r + ($"('{random_cat}','{random_owner2}'),");
                r = r + ($"('{random_cat}','{random_owner3}')");
                if (rand2 + rand3 < max_cat)
                {
                    r = r + ",";
                }
            }

            for (int i = rand2+rand3; i <= max_cat; i++)  
            {
                if (r.IndexOf($"('{i%max_cat+1}','{i% max_owner+1}')") != -1)
                {
                    i++;
                    continue;
                }
                r = r + ($"('{i % max_cat+1}','{i % max_owner+1}')");
                if (i != max_cat) r = r + ',';
            }
            
            return r;

        }
        public string owner_family()
        {
            int max_con = faker.Random.Number(40, 60);
            string r = "";
            int max_family_cat = faker.Random.Number(2, 5);

            string sql = $"SELECT cat_id,count(owner_id) as num FROM catbd.cat_owner group by cat_id having num > 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            List<int> Cats = new List<int>(max_family_cat);
            for(int i = 0; i < max_family_cat; i++)
            {
                Cats[i] = reader.GetValue(i);
            }
            //находим котов с неколькими хозяевами

            r = r + "INSERT INTO owner_family(family_id,owner_id) VALUES ";
            Console.WriteLine(reader.GetValue(0));
            for (int i = 0; i < max_con; i++)
            {
                /*int rand_cat = r[0];
                string sql1 = $"SELECT owner_id FROM catbd.cat_owner where cat_id ={rand_cat}";
                MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                MySqlDataReader reader1 = cmd.ExecuteReader();
                IDataReader dataRecord1 = reader;
                //и этих хозяев

                int rand_family= faker.Random.Number(1,max_family);

                r = r + ($"('{rand_family}','{i % max_owner + 1}')");
                //в одну семью
                */
            }            
            for (int i = 1; i <= max_con; i++)
            {
                if (r.IndexOf($"('{i % max_family + 1}','{ i % max_owner + 1}')") == -1)
                {
                    r = r + ($"('{ i % max_family + 1}','{ i % max_owner + 1}')");
                    if (i != max_con) r = r + ',';
                }
                else
                {
                    i++;
                }
            }
            return r;
        }
        public void QuareryOut(IDataReader r,int num)
        {
            //num количество столбцов
            while (r.Read())
            {
                if (num == 1)
                {
                    Console.WriteLine(r[0]);
                }
                if(num == 2)
                {
                    Console.WriteLine("{0}\t{1}", r[0], r[1]);
                }
                if (num == 3)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", r[0], r[1], r[2]);
                }
            }
            r.Close();
        }

        public void DoUnique(string from_table,string find_col,string out_name,bool full_out)
        {
            string sql = $"SELECT count(distinct {find_col}) FROM catbd.{from_table}";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            string countU = cmd.ExecuteScalar().ToString();

            sql = $"SELECT count({find_col}) FROM catbd.{from_table}";
            cmd = new MySqlCommand(sql, conn);
            string countAll = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"Уникальные {out_name}: {countU}  Общее число: {countAll}");
            if (full_out)
            {
                sql = $"SELECT {find_col} FROM catbd.{from_table}";
                cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                IDataReader dataRecord = reader;
                Console.WriteLine($"{out_name}:");
                QuareryOut(reader, 1);
            }
            Console.WriteLine();
        }
        
        public void CheckUnique()
        {
            //from_table    find_col   out_name  full_out
            DoUnique("breed", "breed_id",  "Породы", false);
            DoUnique("pride", "pride_id", "Прайды", false);
            DoUnique("family", "family_id", "Семьи", false);
            DoUnique("cat", "nickname", "Клички", true);

        }
        public void CheackCats()
        {
            string sql = $"SELECT count(distinct cat_id) FROM catbd.cat_owner AS b1 where (select count(owner_id) from cat_owner AS b2 where b1.cat_id=b2.cat_id)=2";
            MySqlCommand cmd = new MySqlCommand(sql, conn);            
            string countU = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"Котов с двумя хозяевами: {countU}");

            Console.WriteLine("Коты\tИх хозяева");
            sql = $"SELECT distinct cat_id,name FROM cat_owner as b1 inner join owner on b1.owner_id=owner.owner_id where (select count(owner_id) from cat_owner AS b2 where b1.cat_id=b2.cat_id)=2";
            cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            QuareryOut(reader, 2);



            sql = $"SELECT count(distinct cat_id) FROM catbd.cat_owner AS b1 where (select count(owner_id) from cat_owner AS b2 where b1.cat_id=b2.cat_id)=3";
            cmd = new MySqlCommand(sql, conn);
            countU = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"Котов с тремя хозяевами: {countU}");

            Console.WriteLine("Коты\tИх хозяева");
            sql = $"SELECT distinct cat_id,name FROM cat_owner as b1 inner join owner on b1.owner_id=owner.owner_id where (select count(owner_id) from cat_owner AS b2 where b1.cat_id=b2.cat_id)=3";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader, 2);



            Console.WriteLine("Коты с неколькими хозяевами из одной семьи:");
            sql = $"SELECT cat_id,count(owner.name),family.title from cat_owner inner JOIN owner \r\non owner.owner_id = cat_owner.owner_id inner join owner_family \r\non cat_owner.owner_id = owner_family.owner_id inner join family \r\non family.family_id = owner_family.family_id\r\ngroup by cat_id,family.title\r\nhaving count(owner.name)>1";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader, 3);

            Console.WriteLine($"\n По итогу их:");
            sql = $"with t as(SELECT cat_id,count(owner.name),family.title from cat_owner inner JOIN owner \r\non owner.owner_id = cat_owner.owner_id inner join owner_family \r\non cat_owner.owner_id = owner_family.owner_id inner join family \r\non family.family_id = owner_family.family_id\r\ngroup by cat_id,family.title\r\nhaving count(owner.name)>1)\r\nselect count(cat_id) from t";
            cmd= new MySqlCommand(sql, conn);
            countU=cmd.ExecuteScalar().ToString();
            Console.WriteLine(countU);
        }
        public void CheackWithout()
        {
            Console.WriteLine("Прайды без котов");
            string sql = $"SELECT title from pride where pride_id not in (Select pride_id From cat)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            QuareryOut(reader, 1);
            Console.WriteLine();

            Console.WriteLine("Семьи без хозяев");
            sql = $"SELECT title from family where family_id not in (Select family_id From owner_family)";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader, 1);
        }
        public void DoQuareries(int x)
        {
            string sql="",line;
            StreamReader sr = new StreamReader("Q"+x+".sql");
            line = sr.ReadLine();    //читаем запрос из файла
            while (line != null)
            {
                sql=sql+" "+line;
                line = sr.ReadLine();
            }
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            if (x == 1 || x==2 || x==3 || x==5 || x==8 || x==9 || x==10)
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                IDataReader dataRecord = reader;
                Console.WriteLine($"title");
                QuareryOut(reader, 1);
                Console.WriteLine();
            }
            if (x == 4)
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                IDataReader dataRecord = reader;
                Console.WriteLine($"AVG(cat.age)");
                QuareryOut(reader, 1);
                Console.WriteLine();
            }
            if (x == 6)
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                IDataReader dataRecord = reader;
                Console.WriteLine($"Количество людей в семье Рабинович");
                QuareryOut(reader, 1);
                Console.WriteLine();
            }
            if (x == 7)
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                IDataReader dataRecord = reader;
                Console.WriteLine($"Количество котов в семье Энгельс");
                QuareryOut(reader, 1);
                Console.WriteLine();
            }
            sr.Close();
        }
    }
}

