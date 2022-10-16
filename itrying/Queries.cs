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
            while (d.Count() <= max_owner)
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
                    if (nick.Count() <= max_nickname)
                    {
                        nick.Add(s);
                    }
                }
            }
            sr.Close();

            string non_pride = "", non_breed = "";
            int random_pride_del = faker.Random.Number(3, 5); //рандомные прайды без котов
            for(int i=0;i< random_pride_del; i++)   
            {
                int x= faker.Random.Number(1, max_pride);
                non_pride=non_pride + "(" + x + ")";
            }

            int random_breed_del=faker.Random.Number(2, 5); //рандомные породы без котов

            for (int i = 0; i < random_breed_del; i++)
            {
                int x = faker.Random.Number(1, max_breed);
                non_breed = non_breed + "(" + x + ")";
            }
            r = r + "INSERT INTO cat(nickname,breed_id,pride_id,age) VALUES ";    
            for(int i = 0; i <= max_cat; i++)
            {
                int random_breed=faker.Random.Number(1, max_breed); //пока нужный не найдём
                while(non_breed.IndexOf($"({random_breed})") != -1)
                {
                    random_breed = faker.Random.Number(1, max_breed);   //на рандоме мы берём
                }

                int random_pride=faker.Random.Number(1, max_pride);
                while (non_pride.IndexOf($"({random_pride})") != -1)
                {
                    random_pride = faker.Random.Number(1, max_pride);   
                }

                r = r + ($"('{nick[i]}','{random_breed}','{random_pride}','{faker.Random.Number(max_age)+1}')");
                if (i != max_cat) r = r + ",";
            }
            return r;            
        }
        public string cat_owner()
        {
            int rand2 = faker.Random.Number(2, 3);
            int rand3 = faker.Random.Number(5, 10);
            
            string r = "INSERT INTO cat_owner(cat_id,owner_id) VALUES "; 
            for(int i = 0; i < rand2; i++)  
            {
                int random_cat=faker.Random.Number(1,max_cat);
                
                int random_owner1=faker.Random.Number(1,max_owner), random_owner2 = faker.Random.Number(1,max_owner);
                
                while (random_owner2 == random_owner1) //хозяева разные
                {
                    random_owner2 = faker.Random.Number(1,max_owner);
                }
                
                if (r.IndexOf($"('{random_cat}','{random_owner1}')") != -1 || r.IndexOf($"('{random_cat}','{random_owner2}')") != -1) //этой запис
                {
                    i--;
                    continue;
                }                
                r = r + ($"('{random_cat}','{random_owner1}'),");
                r = r + ($"('{random_cat}','{random_owner2}'),");
            }

            for (int i = 0; i < rand3; i++)
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
                    i--;
                    continue;
                }
                r = r + ($"('{random_cat}','{random_owner1}'),");
                r = r + ($"('{random_cat}','{random_owner2}'),");
                r = r + ($"('{random_cat}','{random_owner3}'),");
            }
            int max_con = faker.Random.Number(40, 60);

            for (int i = 0; i <= max_con; i++)  
            {
                int random_owner = faker.Random.Number(1, max_owner);
                int random_cat = faker.Random.Number(1, max_cat);
                
                if (r.IndexOf($"('{random_cat}','{random_owner}')") != -1)
                {
                    i--;
                    continue;
                }
                r = r + ($"('{random_cat}','{random_owner}')");
                if (i != max_con) r = r + ',';
            }
            
            return r;

        }
        public string owner_family()
        {
            int max_con = faker.Random.Number(40, 60);
           
            int max_family_cat = faker.Random.Number(2, 5);

            //находим котов с неколькими хозяевами
            string sql = $"SELECT cat_id,count(owner_id) as num FROM catbd.cat_owner group by cat_id having num > 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            List<int> Cats = new List<int>();
            for (int i = 0; i < max_family_cat && reader.Read(); i++)
            {                
                int x = reader.GetInt32(0);
                Cats.Add(x);               
            }            
            reader.Close();


            string r = "INSERT INTO owner_family(family_id,owner_id) VALUES ";
            for (int i = 0; i < max_family_cat; i++)
            {
                //хозяева этого кота
                sql = $"SELECT owner_id FROM cat_owner where cat_id={Cats[i]}";

                //помещаем их в одну семью
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                int random_family = faker.Random.Number(1, max_family);

               while(reader.Read())
                {
                    int x = reader.GetInt32(0);
                    if (r.IndexOf($"('{random_family}','{x}')") == -1)    //одинаковые записи не добавляем
                    {
                        r = r + ($"('{random_family}','{x}'),");
                        max_con--;
                    }
                }
                reader.Close();
            }

            //остальные связи
            int random_family_del=faker.Random.Number(2,3);
            string non_family = "";

            for (int i = 0; i < random_family_del; i++) //семьи без хозяев
            {
                int x = faker.Random.Number(1, max_family);
                non_family = non_family + "(" + x + ")";
            }
            for (int i = 1; i <= max_con; i++)
            {
                int random_family = faker.Random.Number(1, max_family);

                int random_owner = faker.Random.Number(1, max_owner);
                while (non_family.IndexOf($"({random_family})") != -1)
                {
                    random_family = faker.Random.Number(1,max_family);
                }
                if (r.IndexOf($"('{random_family}','{random_owner}')") == -1)
                {
                    r = r + ($"('{random_family}','{random_owner}')");
                    if (i != max_con) r = r + ',';
                }
                else
                {
                    i--;
                }
            }
            return r;
        }
        public bool QuareryOut(MySqlDataReader reader)
        {
            int Count = reader.FieldCount;
            
            if (!reader.HasRows) {
                reader.Close();
                return false;
                }
            
            if (reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                if (Count == 1)
                {                    
                    Console.WriteLine(String.Format("|{0,35}|", reader.GetName(0)));
                }
                if (Count == 2)
                {
                    Console.WriteLine(String.Format("|{0,35}|{1,20}|", reader.GetName(0), reader.GetName(1)));
                }
                if (Count == 3)
                {
                    Console.WriteLine(String.Format("|{0,35}|{1,20}|{2,20}|", reader.GetName(0), reader.GetName(1), reader.GetName(2)));
                }
                while (reader.Read()) // построчно считываем данные
                {
                    if (Count == 1)
                    {
                        if (reader.IsDBNull(0))
                        {
                            reader.Close();
                            return false;
                        }
                        Console.WriteLine(String.Format("|{0,35}|", reader.GetString(0)));
                    }
                    if (Count == 2)
                    {
                        Console.WriteLine(String.Format("|{0,35}|{1,20}|", reader.GetString(0), reader.GetString(1)));
                    }
                    if (Count == 3)
                    {
                        Console.WriteLine(String.Format("|{0,35}|{1,20}|{2,20}|", reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
                Console.WriteLine();
            }
            reader.Close();
            return true;
        }

        public void DoUnique(string from_table,string find_col)
        {
            Console.WriteLine(from_table);
            string sql = $"SELECT {find_col},count({find_col}) FROM catbd.{from_table} group by {find_col}";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            QuareryOut(reader);
        }
        
        public void CheckUnique()
        {
            //from_table    find_col  
            DoUnique("breed", "title");
            DoUnique("pride", "title");
            DoUnique("family", "title");
            DoUnique("cat", "nickname");

        }
        public void CheackCats()
        {
            string sql = $"SELECT cat_id,count(cat_id) as num from cat_owner group by cat_id having num=2";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            QuareryOut(reader);

            sql = $"SELECT cat_id,count(cat_id) as num from cat_owner group by cat_id having num=3";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader);

            Console.WriteLine("Коты с неколькими хозяевами из одной семьи:");
            string line;sql = "";
            StreamReader sr = new StreamReader("CheackCats.sql");
            line = sr.ReadLine();    //читаем запрос из файла
            while (line != null)
            {
                sql = sql + " " + line;
                line = sr.ReadLine();
            }            
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader);            
        }
        public void CheackWithout()
        {
            Console.WriteLine("Прайды без котов");
            string sql = $"SELECT title from pride where pride_id not in (Select pride_id From cat)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            QuareryOut(reader);

            Console.WriteLine("Породы без котов");
            sql = $"SELECT title from breed where breed_id not in (Select breed_id From cat)";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader);

            Console.WriteLine("Семьи без хозяев");
            sql = $"SELECT title from family where family_id not in (Select family_id From owner_family)";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            QuareryOut(reader);
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
            MySqlDataReader reader = cmd.ExecuteReader();            
            bool is_out=QuareryOut(reader);
            sr.Close();
            if(x==9 && !is_out)
            {
                Console.WriteLine("У Карла Маркса нет котов");
            }
            if (x == 4 && !is_out)
            {
                Console.WriteLine("Нет котов породы сфинкс");
            }
        }
    }
}

