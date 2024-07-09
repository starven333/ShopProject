using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DBHelper
{
    private static MySqlConnection connection;
    public static MySqlConnection GetConnection()
    {
        if (connection == null)
        {
            connection = new MySqlConnection
            {
                ConnectionString = @"server=localhost;user id=root;password=root;port=3306;database=bookstore;"
            };
        }
        return connection;
    }
    public static MySqlConnection OpenConnection()
    {
        if (connection == null)
        {
            GetConnection();
        }
        connection.Open();
        return connection;
    }
    public static MySqlDataReader ExecQuery(string query)
    {
        MySqlCommand command = new MySqlCommand(query, connection);
        return command.ExecuteReader();
    }
    public static void CloseConnection()
    {
        if (connection != null) connection.Close();
    }
}

/*public class Book{
    public int Isbn { set; get; }
    public string Category { set; get; }
    public string Name { set; get; }
    public string Author { set; get; }
    public string ReleaseDate { set; get; }
    public decimal Price { set; get; }
    public int Quantity { set; get; }
    public string Description { set; get; }
    private string query;

    public void add_product(Book c){
        MySqlCommand cmd = new MySqlCommand("inputProduct", DBHelper.OpenConnection());
        try {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_category", c.Category);
            cmd.Parameters["@p_category"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_name", c.Name);
            cmd.Parameters["@p_name"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_size", c.Size);
            cmd.Parameters["@p_size"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_color", c.Color);
            cmd.Parameters["@p_color"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_price", c.Price);
            cmd.Parameters["@p_price"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_quantity", c.Quantity);
            cmd.Parameters["@p_quantity"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
        } catch (MySqlException e) {
            Console.WriteLine(e.Message);
        }
        finally {
            DBHelper.CloseConnection();
        }
    }
    
    public void update_product(Book c){
        MySqlCommand cmd = new MySqlCommand("updateProduct", DBHelper.OpenConnection());
        try {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id", c.Id);
            cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_category", c.Category);
            cmd.Parameters["@p_category"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_name", c.Name);
            cmd.Parameters["@p_name"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_size", c.Size);
            cmd.Parameters["@p_size"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_color", c.Color);
            cmd.Parameters["@p_color"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_price", c.Price);
            cmd.Parameters["@p_price"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@p_quantity", c.Quantity);
            cmd.Parameters["@p_quantity"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
        } catch (MySqlException e){
            Console.WriteLine(e.Message);     
        }
        finally {
            DBHelper.CloseConnection();
        }
    }

    public bool exist_product(int c){
        bool result = false;
        MySqlCommand cmd = new MySqlCommand("product_exist", DBHelper.OpenConnection());
        try {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_product_id", c);
            cmd.Parameters["@p_product_id"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@id", MySqlDbType.Bit);
            cmd.Parameters["@id"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            result = Convert.ToBoolean(cmd.Parameters["@id"].Value);

        } catch (MySqlException e) {
            Console.WriteLine(e.Message);
        }
        finally {
            DBHelper.CloseConnection();
        }
        return result;
    }
}*/
public class User
{
    private int ID { get; set; }
    private string Username { get; set; }
    private string Password { get; set; }
    private string Name { get; set; }
    private string Phone { get; set; }
    private string Address { get; set; }
    private int Role { get; set; }

    public void Save()
    {
        string query = "INSERT INTO Users (login_name, login_password, user_name, phone, address) VALUES (@Username, @Password, @Name, @Phone, @Address)";
        using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
        {
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.ExecuteNonQuery();
        }
        DBHelper.CloseConnection();
    }
    public void GetUser(string username, string password)
    {

        string query = $"SELECT * FROM users WHERE login_name = '{username}' AND login_password = '{password}';";

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            if (reader.Read())
            {
                ID = reader.GetInt32("user_id");
                Name = reader.GetString("user_name");
                Phone = reader.GetString("phone");
                Address = reader.GetString("address");
                Role = reader.GetInt32("user_role");
            }
        }

        DBHelper.CloseConnection();
        //return user;
    }
    public void userInfo(User u)
    {
        Console.WriteLine(u.ID);
        Console.WriteLine(u.Name);
        Console.WriteLine(u.Phone);
        Console.WriteLine(u.Address);
        Console.WriteLine(u.Role);
    }
    public bool IsAdmin()
    {
        if (Role == 0) return false;
        else return true;
    }
    public void SignIn()
    {
        Console.Write("- Enter Username: ");
        Username = Console.ReadLine();
        Console.Write("- Enter Password: ");
        Password = Console.ReadLine();
        Console.Write("- Enter Full Name: ");
        Name = Console.ReadLine();
        Console.Write("- Enter Phone Number: ");
        Phone = Console.ReadLine();
        Console.Write("- Enter Address: ");
        Address = Console.ReadLine();

        Save();
    }
    public void Login()
    {
        bool isLoggedIn = false;
        string loginName, loginPass;

        Console.Write("- Enter Username: ");
        loginName = Console.ReadLine();
        Console.Write("- Enter Password: ");
        loginPass = Console.ReadLine();

        GetUser(loginName, loginPass);
        
        if (Name != null)
        {
            Console.WriteLine("Login Success");
            userInfo(this);
            isLoggedIn = true;
        }
        else
        {
            Console.WriteLine("No User Found");
            isLoggedIn = false;
        }

        if (isLoggedIn)
        {
            if (this.IsAdmin())
            {
                adminMenu();
            }
            else
            {
                userMenu();
            }
        }
    }
    public void userMenu()
    {
        int option = 0;
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- User Menu ---");
            Console.WriteLine("1. Search Book");
            Console.WriteLine("2. Buy");
            Console.WriteLine("3. Account Info");
            Console.WriteLine("0. exit");
            Console.Write("#Input option: ");
            option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 0:
                    Console.WriteLine("\nExit Program.");
                    exit = true;
                    break;
                case 1:
                    //SearchBook();
                    break;
                case 2:
                    //BuyProduct();
                    break;
                case 3:
                    //AccountInfo();
                    break;
                default:
                    break;
            }
        }
    }
    public void adminMenu()
    {
        int option = 0;
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. Manage Book");
            Console.WriteLine("2. Manage User");
            Console.WriteLine("3. Orders Detail");
            Console.WriteLine("0. exit");
            Console.Write("#Input option: ");
            option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 0:
                    Console.WriteLine("\nExit Program.");
                    exit = true;
                    break;
                case 1:
                    //ManageBook();
                    break;
                case 2:
                    //ManageUser();
                    break;
                case 3:
                    //OrderDetail();
                    break;
                default:
                    break;
            }
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        int option = 0;
        bool exit = false;
        User u = new User();

        while (!exit)
        {
            Console.WriteLine("\n--- Book Store ---");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign in");
            Console.WriteLine("0. exit");
            Console.Write("#Input option: ");
            option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 0:
                    Console.WriteLine("\nExit Program.");
                    exit = true;
                    break;
                case 1:
                    u.Login();
                    break;
                case 2:
                    u.SignIn();
                    break;
                default:
                    break;
            }
        }
    }
}
