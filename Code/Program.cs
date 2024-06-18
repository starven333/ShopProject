using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DBHelper
{
    private static MySqlConnection connection;
    public static MySqlConnection GetConnection()
    {
        if (connection == null){
            connection = new MySqlConnection {
                ConnectionString = @"server=localhost;user id=root;password=root;port=3306;database=SaleDB;"   
            };
        }
        return connection;
    }
    public static MySqlConnection OpenConnection()
    {
        if (connection == null){
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

public class Book{
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
}

public class User
{
    public int ID { get; private set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Role { get; set; }

    public void Save()
    {
        using (MySqlConnection conn = DBHelper.OpenConnection())
        {
            string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.ExecuteNonQuery();
                ID = (int)cmd.LastInsertedId;
            }
        }
        DBHelper.CloseConnection();
    }

    public static User GetUser(string username, string password)
    {
        User user = new User();
        using (MySqlConnection conn = DBHelper.OpenConnection())
        {
            string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.ID = reader.GetInt32("user_id");
                        user.Name = reader.GetString("user_name");
                        user.Phone = reader.GetString("phone");
                        user.Address = reader.GetString("address");
                        user.Role = reader.GetString("role");
                    }
                }
            }
        }

        return user;
    }

    public bool IsAdmin()
    {
        return Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }
}

class Program{
    static public void SearchProduct(){
        bool again = true;
        int option = 0;
        string query, input;

        while(again){
            
            Console.WriteLine("\n--- Search Product ---");
            Console.WriteLine("1. By Name");
            Console.WriteLine("2. By Category");
            Console.WriteLine("3. By Price Range");
            Console.WriteLine("0. back");
            Console.Write("#Input option: ");
            option = Convert.ToInt32(Console.ReadLine());

        switch (option) {
            case 0:
                again = false;
                break;
            case 1:
                Console.Write("- Enter name: ");
                input = Console.ReadLine();
                query = $"select * from products where product_name like '{input}%';";
                Console.WriteLine("Product id\t\tProduct Name\t\tSize\t\tColor\t\tPrice\t\tQuantity");

                DBHelper.OpenConnection();
                using (MySqlDataReader reader = DBHelper.ExecQuery(query))
                {
                    while (reader.Read())
                    {
                        string row = $"{reader["product_id"]}\t\t{reader["product_name"]}\t\t{reader["size"]}\t\t{reader["color"]}\t\t{reader["price"]}\t\t{reader["quantity"]}";
                        Console.WriteLine(row);
                    }
                    reader.Close();
                }
                DBHelper.CloseConnection();
                break;

            case 2:
                Console.Write("- Enter category: ");
                input = Console.ReadLine();
                query = $"select * from products p join categories c on p.category_id=c.category_id where category_name like '%{input}%';";
                Console.WriteLine("Product id\t\tCategory\t\tProduct Name\t\tSize\t\tColor\t\tPrice\t\tQuantity");

                DBHelper.OpenConnection();
                using (MySqlDataReader reader = DBHelper.ExecQuery(query))
                {
                    while (reader.Read())
                    {
                        string row = $"{reader["product_id"]}\t\t{reader["category_name"]}\t\t{reader["product_name"]}\t\t{reader["size"]}\t\t{reader["color"]}\t\t{reader["price"]}\t\t{reader["quantity"]}";
                        Console.WriteLine(row);
                    }
                    reader.Close();
                }
                DBHelper.CloseConnection();
                break;

            case 3:
                string input1;

                Console.Write("- Price from: ");
                input = Console.ReadLine();
                Console.Write("To: ");
                input1 = Console.ReadLine();
                query = $"select * from products where {input} < price and price < {input1};";
                Console.WriteLine("Product id\t\tProduct Name\t\tSize\t\tColor\t\tPrice\t\tQuantity");

                DBHelper.OpenConnection();
                using (MySqlDataReader reader = DBHelper.ExecQuery(query))
                {
                    while (reader.Read())
                    {
                        string row = $"{reader["product_id"]}\t\t{reader["product_name"]}\t\t{reader["size"]}\t\t{reader["color"]}\t\t{reader["price"]}\t\t{reader["quantity"]}";
                        Console.WriteLine(row);
                    }
                    reader.Close();
                }
                DBHelper.CloseConnection();
                break;

            default:
                break;
        }
        }  
    }
    static public void InsertProduct(){
        Product c = new Product();

        try{
        Console.Write("- Category: ");
        c.Category = Console.ReadLine();

        Console.Write("- Name: ");
        c.Name = Console.ReadLine();

        Console.Write("- Size: ");
        c.Size = Console.ReadLine();

        Console.Write("- Color: ");
        c.Color = Console.ReadLine();

        Console.Write("- Price: ");
        c.Price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("- Quantity: ");
        c.Quantity = Convert.ToInt32(Console.ReadLine());

        c.add_product(c);
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }
    static void UpdateProduct(){
        Product c = new Product();

        Console.Write("- Enter Id for update: ");
        int id = Convert.ToInt32(Console.ReadLine());
        if(c.exist_product(id)){
            try{
            c.Id = id;

            Console.Write("- New Category: ");
            c.Category = Console.ReadLine();

            Console.Write("- New Name: ");
            c.Name = Console.ReadLine();

            Console.Write("- New Size: ");
            c.Size = Console.ReadLine();

            Console.Write("- New Color: ");
            c.Color = Console.ReadLine();

            Console.Write("- New Price: ");
            c.Price = Convert.ToDecimal(Console.ReadLine());

            Console.Write("- New Quantity: ");
            c.Quantity = Convert.ToInt32(Console.ReadLine());

            c.update_product(c);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }
        else Console.WriteLine("ID don't exist");
    }

    static void Main(string[] args){

        int option = 0;
        bool exit = false;

        while(!exit){
            Console.WriteLine("\n--- Sale database ---");
            Console.WriteLine("1. Insert Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Search Product");
            Console.WriteLine("0. exit");
            Console.Write("#Input option: ");
            option = Convert.ToInt32(Console.ReadLine());

            switch (option) {
            case 0:
                Console.WriteLine("\nExit Program.");
                exit = true;
                break;
            case 1:
                InsertProduct();
                break;
            case 2:
                UpdateProduct();
                break;
            case 3:
                SearchProduct();
                break;
            default:
                break;
            }  
        }
    }
}
//comment