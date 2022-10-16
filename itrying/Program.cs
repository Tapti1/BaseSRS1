using MySql.Data.MySqlClient;
using itrying;
using System.Data;
using System.Data.SqlClient;


MySqlConnection conn = BaseConnection.GetDBConnection();
conn.Open();                        //подключаем
Queries query = new Queries(conn);    //делаем запросы

Console.WriteLine("Заполнить таблицу данными?(Да/Нет)");
string CreateData=Console.ReadLine();
String sql="";

if (CreateData == "Да") {
    query.DELECTE_ALL();

    query.SetMax();

    sql = query.breed();
    MySqlCommand cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = query.pride();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = query.family();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = query.owner();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = query.cat();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();

    sql = query.cat_owner();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();
    
    sql = query.owner_family();
    cmd = new MySqlCommand(sql, conn);
    cmd.ExecuteNonQuery();
}
Console.WriteLine("Проверить уникальность?(Да/Нет)");
string Cheack = Console.ReadLine();
if (Cheack == "Да")
{
    query.CheckUnique();
}
Console.WriteLine("Проверить котов?(Да/Нет)");
Cheack = Console.ReadLine();
if (Cheack == "Да")
{
    query.CheackCats();
}
Console.WriteLine("Проверить на отсутвие?(Да/Нет)");
Cheack = Console.ReadLine();
if (Cheack == "Да")
{
    query.CheackWithout();
}
Console.WriteLine("Введите номер запроса(или 0 для выхода)");
Cheack = Console.ReadLine();
while (Cheack != "0"){
    
    query.DoQuareries(Convert.ToInt32(Cheack));
    Console.WriteLine("Введите номер запроса(или 0 для выхода)");
    Cheack = Console.ReadLine();
}

conn.Close();
