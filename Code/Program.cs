using System;
using System.Collections.Immutable;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Spectre.Console;

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

public class Book
{
    public string Isbn { set; get; }
    public string Category { set; get; }
    public string Name { set; get; }
    public string Author { set; get; }
    public DateOnly ReleaseDate { set; get; }
    public decimal Price { set; get; }
    public int Quantity { set; get; }
    public string Description { set; get; }

    public void bookInfo(Book c)
    {
        string query = @$"SELECT * FROM book 
                        join categories on book.category_id = categories.category_id 
                        join author on book.author_id = author.author_id
                        WHERE isbn = '{c.Isbn}';";

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            if (reader.Read())
            {
                c.Name = reader.GetString("book_name");
                c.Category = reader.GetString("category_name");
                c.Author = reader.GetString("author_name");
                c.ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("release_date"));
                c.Price = reader.GetDecimal("price");
                c.Quantity = reader.GetInt32("quantity");
            }
        }

        DBHelper.CloseConnection();

        AnsiConsole.MarkupLine("[green]---Book Info---[/]");
        Console.WriteLine($"ISBN: {c.Isbn}");
        Console.WriteLine($"Category: {c.Category}");
        Console.WriteLine($"Name: {c.Name}");
        Console.WriteLine($"Author: {c.Author}");
        Console.WriteLine($"Release Date: {c.ReleaseDate}");
        Console.WriteLine($"Price: {c.Price}");
        Console.WriteLine($"Quantity: {c.Quantity}");
        Console.WriteLine("----------------------------");
    }

    public int checkAuthor(Book c)
    {
        int aID = -1;

        string query = $"SELECT * FROM author WHERE author_name = '{c.Author}'";
        using (var cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    aID = reader.GetInt32("author_id");
                }
            }
        }
        DBHelper.CloseConnection();

        if (aID == -1)
        {
            query = $"INSERT INTO author (author_name) VALUES ('{c.Author}'); SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
            {
                aID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        DBHelper.CloseConnection();

        return aID;
    }

    public int checkCategory(Book c)
    {
        int cID = -1;

        string query = $"SELECT * FROM categories WHERE category_name = '{c.Category}'";
        using (var cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    cID = reader.GetInt32("category_id");
                }
            }
        }
        DBHelper.CloseConnection();

        if (cID == -1)
        {
            query = $"INSERT INTO categories (category_name) VALUES ('{c.Category}'); SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
            {
                cID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        DBHelper.CloseConnection();

        return cID;
    }

    public bool bookExist(string isbn)
    {
        string query = $"SELECT COUNT(*) FROM book WHERE isbn = '{isbn}';";

        using (MySqlCommand command = new MySqlCommand(query, DBHelper.OpenConnection()))
        {
            int count = Convert.ToInt32(command.ExecuteScalar());
            DBHelper.CloseConnection();

            return count > 0;
        }
    }

    public void addBook(Book c, string date)
    {
        int aID = checkAuthor(c);
        int cID = checkCategory(c);


        string query = @$"INSERT INTO book (isbn, category_id, author_id, book_name, release_date, price, quantity) 
                VALUES ('{c.Isbn}', {cID}, {aID} , '{c.Name}', '{date}', {c.Price}, {c.Quantity});";
        using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
        {
            cmd.ExecuteNonQuery();
        }
        DBHelper.CloseConnection();

        Console.WriteLine("Book Added Success!");
        Console.ReadLine();
    }

    public void updateBook(Book c)
    {

        while (true)
        {
            AnsiConsole.Clear();

            if (c == null) return;
            c.bookInfo(c);

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n-Select What to Change:")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Category", "Name", "Author", "Release Date", "Price", "Quantity", "Exit"
                    }));

            switch (option)
            {
                case "Category":
                    c.Category = AnsiConsole.Ask<string>($"\nEnter new [green]Category[/]:");
                    c.infoChange(c);
                    break;
                case "Name":
                    c.Name = AnsiConsole.Ask<string>($"\nEnter new [green]Name[/]:");
                    c.infoChange(c);
                    break;
                case "Author":
                    c.Author = AnsiConsole.Ask<string>($"\nEnter new [green]Author[/]:");
                    c.infoChange(c);
                    break;
                case "Release Date":
                    c.ReleaseDate = AnsiConsole.Ask<DateOnly>($"\nEnter new [green]Release Date(yyyy/mm/dd)[/]:");
                    c.infoChange(c);
                    break;
                case "Price":
                    c.Price = AnsiConsole.Ask<Decimal>($"\nEnter new [green]Price[/]:");
                    while (c.Price < 0) c.Price = AnsiConsole.Ask<Decimal>($"\nEnter new [green]Price[/]:");
                    c.infoChange(c);
                    break;
                case "Quantity":
                    c.Quantity = AnsiConsole.Ask<int>($"\nEnter new [green]Quantity[/]:");
                    while (c.Quantity < 0) c.Quantity = AnsiConsole.Ask<int>($"\nEnter new [green]Quantity[/]:");
                    c.infoChange(c);
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public void infoChange(Book c)
    {
        try
        {
            char opt = 'n';
            opt = 'n';
            opt = AnsiConsole.Ask<char>("[yellow]Confirm(y/n)[/]:");

            if (opt == 'y')
            {
                int aID = checkAuthor(c);
                int cID = checkCategory(c);
                string b = c.ReleaseDate.ToString("yyyy-MM-dd");

                string query = @$"update book set book_name = '{c.Name}', release_date = '{b}', price = {c.Price}, 
                                quantity = {c.Quantity}, author_id = {aID}, category_id = {cID} where isbn = '{c.Isbn}';";
                using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Update Success!");
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
        DBHelper.CloseConnection();
    }

    public Book bookSelector(List<Book> books)
    {
        int selectedIndex = 0;
        int rowsPerPage = 10;
        int totalPages = (int)Math.Ceiling((double)books.Count / rowsPerPage);
        int currentPage = 0;

        while (true)
        {
            var table = new Table();
            table.AddColumn("ISBN");
            table.AddColumn("Name");
            table.AddColumn("Author");
            table.AddColumn("Category");
            table.AddColumn("Release Date");
            table.AddColumn("Price");
            table.AddColumn("Quantity");
            table.AddColumn("Description");

            var data = books.Skip(currentPage * rowsPerPage).Take(rowsPerPage).ToList();

            for (int i = 0; i < data.Count; i++)
            {
                var b = data[i];
                var rowStyle = i == selectedIndex % rowsPerPage ? "green" : "";

                table.AddRow(
                    new Markup($"[{rowStyle}]{b.Isbn}[/]"),
                    new Markup($"[{rowStyle}]{b.Name}[/]"),
                    new Markup($"[{rowStyle}]{b.Author}[/]"),
                    new Markup($"[{rowStyle}]{b.Category}[/]"),
                    new Markup($"[{rowStyle}]{b.ReleaseDate}[/]"),
                    new Markup($"[{rowStyle}]{b.Price}[/]"),
                    new Markup($"[{rowStyle}]{b.Quantity}[/]"),
                    new Markup($"[{rowStyle}]{b.Description}[/]")
                );
            }


            AnsiConsole.Clear();
            AnsiConsole.Write(table);

            AnsiConsole.WriteLine($"\nPage {currentPage + 1} of {totalPages} (Select <-/-> to move page)");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                if (selectedIndex % rowsPerPage == 0) currentPage = currentPage - 1;
                if (currentPage < 0) currentPage = totalPages - 1;

                selectedIndex = (selectedIndex - 1 + books.Count) % books.Count;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % books.Count;
                if (selectedIndex % rowsPerPage == 0) currentPage = (currentPage + 1) % totalPages;
            }
            else if (key == ConsoleKey.Escape)
            {
                throw new ArgumentException("Exit!");
            }
            else if (key == ConsoleKey.RightArrow)
            {
                currentPage = (currentPage + 1) % totalPages;
                selectedIndex = currentPage * rowsPerPage;
                if (selectedIndex > books.Count) selectedIndex = selectedIndex % books.Count;

            }
            else if (key == ConsoleKey.LeftArrow)
            {
                currentPage = (currentPage == 0) ? totalPages - 1 : currentPage - 1;
                selectedIndex = currentPage * rowsPerPage;
                if (selectedIndex > books.Count) selectedIndex = selectedIndex % books.Count;

            }
            else if (key == ConsoleKey.Enter)
            {
                var selectedBook = books[selectedIndex];
                AnsiConsole.MarkupLine($"[bold]You selected:[/] [bold]{selectedBook.Isbn}[/] - [bold]{selectedBook.Name}[/]");
                return selectedBook;
            }
        }
    }

    public Book SearchByName()
    {
        string query, input;
        var books = new List<Book>();

        Console.Write("\nEnter book name:");
        input = Console.ReadLine();
        query = @$"SELECT* FROM book JOIN author ON book.author_id = author.author_id 
                    JOIN categories ON book.category_id = categories.category_id 
                    where book_name like '%{input}%' order by book_name;";

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            while (reader.Read())
            {
                var b = new Book
                {
                    Isbn = reader.GetString("isbn"),
                    Name = reader.GetString("book_name"),
                    Category = reader.GetString("category_name"),
                    Author = reader.GetString("author_name"),
                    ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("release_date")),
                    Price = reader.GetDecimal("price"),
                    Quantity = reader.GetInt32("quantity"),
                    Description = reader.GetString("product_des")
                };
                books.Add(b);
            }
            reader.Close();
        }

        DBHelper.CloseConnection();

        if (books.Count == 0) return null;

        return bookSelector(books);
    }

    public Book SearchByCategory()
    {
        string query, input;
        var books = new List<Book>();

        Console.Write("\nEnter category name:");
        input = Console.ReadLine();
        query = @$"SELECT* FROM book JOIN author ON book.author_id = author.author_id 
                JOIN categories ON book.category_id = categories.category_id
                where category_name like '%{input}%' order by book_name;";

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            while (reader.Read())
            {
                var b = new Book
                {
                    Isbn = reader.GetString("isbn"),
                    Name = reader.GetString("book_name"),
                    Category = reader.GetString("category_name"),
                    Author = reader.GetString("author_name"),
                    ReleaseDate = DateOnly.FromDateTime(reader.GetDateTime("release_date")),
                    Price = reader.GetDecimal("price"),
                    Quantity = reader.GetInt32("quantity"),
                    Description = reader.GetString("product_des")
                };
                books.Add(b);
            }
            reader.Close();
        }

        DBHelper.CloseConnection();

        if (books.Count == 0) return null;

        return bookSelector(books);
    }
}

public class User
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int Role { get; set; }
    public int Status { get; set; }

    public void userSave()
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
                Status = reader.GetInt32("user_status");
            }
        }

        DBHelper.CloseConnection();
        //return user;
    }

    public void displayOrders()
    {
        string query = @$"select * from orderdetails 
                        join book on book.isbn = orderdetails.isbn 
                        join users on users.user_id = orderdetails.user_id
                        order by order_date desc;";

        var table = new Table();
        table.AddColumn("Order ID");
        table.AddColumn("User ID");
        table.AddColumn("User Name");
        table.AddColumn("ISBN");
        table.AddColumn("Book Name");
        table.AddColumn("Order Date");
        table.AddColumn("Quantity");
        table.AddColumn("Total Price");

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            while (reader.Read())
            {
                {
                    table.AddRow(reader.GetInt32("order_detail_id").ToString(), reader.GetInt32("user_id").ToString(),
                                reader.GetString("user_name"), reader.GetString("isbn"), reader.GetString("book_name"),
                                reader.GetDateTime("order_date").ToString(), reader.GetInt32("quantity").ToString(),
                                reader.GetDecimal("total_price").ToString());
                };

            }
            reader.Close();
        }
        DBHelper.CloseConnection();

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    public void GetOrders(User u)
    {
        string query = @$"select * from orderdetails join book on book.isbn = orderdetails.isbn 
                        where user_id = {u.ID} order by order_date desc;";

        var table = new Table();
        table.AddColumn("Order ID");
        table.AddColumn("ISBN");
        table.AddColumn("Book Name");
        table.AddColumn("Order Date");
        table.AddColumn("Quantity");
        table.AddColumn("Total Price");

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            while (reader.Read())
            {
                {
                    table.AddRow(reader.GetInt32("order_detail_id").ToString(), reader.GetString("isbn"), reader.GetString("book_name"),
                                reader.GetDateTime("order_date").ToString(), reader.GetInt32("quantity").ToString(),
                                reader.GetDecimal("total_price").ToString());
                };

            }
            reader.Close();
        }
        DBHelper.CloseConnection();

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    public void userInfo(User u)
    {
        AnsiConsole.MarkupLine("[green]---Account Info---[/]");
        Console.WriteLine($"User ID: {u.ID}");
        Console.WriteLine($"Fullname: {u.Name}");
        Console.WriteLine($"Phone Number: {u.Phone}");
        Console.WriteLine($"Address: {u.Address}");
        Console.WriteLine("----------------------------");
    }

    public void updateInfo(User u, string iName, string i)
    {
        try
        {
            char opt = 'n';

            string a = AnsiConsole.Ask<string>($"\nEnter new [green]{i}[/]:");
            opt = AnsiConsole.Ask<char>("[yellow]Confirm(y/n)[/]:");

            if (opt == 'y')
            {
                if (iName == "user_name") u.Name = a;
                else if (iName == "phone") u.Phone = a;
                else if (iName == "address") u.Address = a;

                string query = $"update users set {iName} = '{a}' where user_id = {u.ID};";
                using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
                {
                    cmd.ExecuteNonQuery();
                }
                DBHelper.CloseConnection();

                Console.WriteLine("Update Success!");
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public User userSelector(List<User> users)
    {
        int selectedIndex = 0;

        while (true)
        {
            var table = new Table();
            table.AddColumn("User ID");
            table.AddColumn("Username");
            table.AddColumn("Password");
            table.AddColumn("Full Name");
            table.AddColumn("Phone");
            table.AddColumn("Address");
            table.AddColumn("Status");

            for (int i = 0; i < users.Count; i++)
            {
                var b = users[i];
                var rowStyle = i == selectedIndex ? "green" : "";
                table.AddRow(
                    new Markup($"[{rowStyle}]{b.ID}[/]"),
                    new Markup($"[{rowStyle}]{b.Username}[/]"),
                    new Markup($"[{rowStyle}]{b.Password}[/]"),
                    new Markup($"[{rowStyle}]{b.Name}[/]"),
                    new Markup($"[{rowStyle}]{b.Phone}[/]"),
                    new Markup($"[{rowStyle}]{b.Address}[/]"),
                    new Markup($"[{rowStyle}]{b.Status}[/]")
                );
            }

            AnsiConsole.Clear();
            AnsiConsole.Write(table);

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + users.Count) % users.Count;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % users.Count;
            }
            else if (key == ConsoleKey.Escape)
            {
                throw new ArgumentException("Exit!");
            }
            else if (key == ConsoleKey.Enter)
            {
                var selectedUser = users[selectedIndex];
                AnsiConsole.MarkupLine($"[bold]You selected:[/] [bold]{selectedUser.ID}[/] - [bold]{selectedUser.Name}[/]");
                return selectedUser;
            }
        }
    }

    public User SearchUserByName()
    {
        string query, input;
        var users = new List<User>();

        Console.Write("\nEnter User's name:");
        input = Console.ReadLine();
        query = $"SELECT* FROM users where user_name like '%{input}%';";

        DBHelper.OpenConnection();

        using (MySqlDataReader reader = DBHelper.ExecQuery(query))
        {
            while (reader.Read())
            {
                var b = new User
                {
                    ID = reader.GetInt32("user_id"),
                    Username = reader.GetString("login_name"),
                    Password = reader.GetString("login_password"),
                    Name = reader.GetString("user_name"),
                    Phone = reader.GetString("phone"),
                    Address = reader.GetString("address"),
                    Role = reader.GetInt32("user_role"),
                    Status = reader.GetInt32("user_status"),
                };

                if (!b.IsAdmin())
                    users.Add(b);
            }
            reader.Close();
        }

        DBHelper.CloseConnection();

        if (users.Count == 0) throw new ArgumentException("No User Found!");

        return userSelector(users);
    }

    public bool IsAdmin()
    {
        if (Role == 0) return false;
        else return true;
    }

    public void SignIn()
    {
        int count = 1;

        while (count > 0)
        {
            Username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");

            string query = $"SELECT COUNT(*) FROM users WHERE login_name = '{Username}';";

            using (MySqlCommand command = new MySqlCommand(query, DBHelper.OpenConnection()))
            {
                count = Convert.ToInt32(command.ExecuteScalar());
                DBHelper.CloseConnection();
            }

            if (count > 0)
            {
                AnsiConsole.MarkupLine("[red]Username exist![/]");
            }
        }

        Password = AnsiConsole.Ask<string>("Enter your [green]password[/]:");
        Name = AnsiConsole.Ask<string>("Enter your [green]full name[/]:");
        Phone = AnsiConsole.Ask<string>("Enter your [green]phone number[/]:");
        Address = AnsiConsole.Ask<string>("Enter your [green]address[/]:");

        char opt = AnsiConsole.Ask<char>("[yellow]Confirm(y/n)[/]:");

        if (opt == 'y')
        {
            userSave();
            Console.WriteLine("Sign In Success!");
            Console.ReadLine();
        }
    }

    public int Login(string loginName, string loginPass)
    {
        GetUser(loginName, loginPass);

        if (Name != null && Status != 0)
        {
            if (IsAdmin()) return 1;
            else return 2;
        }
        else
        {
            return 0;
        }
    }

    public void updateUserStatus(User u)
    {
        int i = 0;

        try
        {
            u = u.SearchUserByName();

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[green]--Select an Option--[/]")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Activate User", "Deactivate User", "Exit"
                    }));

            switch (option)
            {
                case "Activate User":
                    i = 1;
                    break;
                case "Deactivate User":
                    i = 0;
                    break;
                case "Exit":
                    return;
            }

            char opt = AnsiConsole.Ask<char>("[yellow]Confirm(y/n)[/]:");

            if (opt == 'y')
            {
                string query = $"update users set user_status = {i} where user_id = {u.ID};";
                using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Update Success!");
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
        DBHelper.CloseConnection();
    }

}

public class Order
{
    public string BookID { get; set; }
    public int UserID { get; set; }
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public Order(User u, Book b)
    {
        BookID = b.Isbn;
        UserID = u.ID;
        UnitPrice = b.Price;
        Stock = b.Quantity;
    }
    public void Transaction()
    {
        int quantity = AnsiConsole.Ask<int>("\n[yellow]Enter quantity to buy[/]: ");

        while (quantity > Stock || quantity < 0)
        {
            if (quantity > Stock)
                AnsiConsole.MarkupLine("\n[red]Not Enough In Stock![/]");
            if (quantity < 0)
                AnsiConsole.MarkupLine("\n[red]Invalid![/]");

            quantity = AnsiConsole.Ask<int>("\n[yellow]Enter quantity to buy[/]: ");
        }

        if (quantity == 0) return;

        TotalPrice = UnitPrice * quantity;
        Stock -= quantity;

        AnsiConsole.MarkupLine($"\n[yellow]Total Price[/]: {TotalPrice} ");
        char opt = AnsiConsole.Ask<char>("\n[yellow]Confirm your order(y/n)[/]: ");

        if (opt == 'y')
        {
            string query = $"update book set quantity = {Stock} where isbn = '{BookID}'";
            using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
            {
                cmd.ExecuteNonQuery();
            }
            DBHelper.CloseConnection();

            query = "INSERT INTO orderdetails (user_id, isbn, quantity, total_price) VALUES (@User, @Isbn, @Quantity, @Price)";
            using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection()))
            {
                cmd.Parameters.AddWithValue("@User", UserID);
                cmd.Parameters.AddWithValue("@Isbn", BookID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Price", TotalPrice);
                cmd.ExecuteNonQuery();
            }
            DBHelper.CloseConnection();

            Console.WriteLine("Order Created!");
            Console.ReadLine();
        }
    }
}

class Program
{
    public static void loginMenu(User u)
    {
        try
        {
            char opt = 'y';
            while (opt != 'n')
            {
                AnsiConsole.Clear();

                var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");

                var password = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter your [green]password[/]:")
                        .PromptStyle("red")
                        .Secret());

                int i = u.Login(username, password);

                if (i == 2) { userMenu(u); return; }
                else if (i == 1) { adminMenu(u); return; }
                else AnsiConsole.Markup("[red]User Not Found![/]\n");

                opt = AnsiConsole.Ask<char>("[yellow]Continue(y/n)[/]:");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
    }

    public static void userMenu(User u)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(@"  _    _               __  __                  
 | |  | |             |  \/  |                 
 | |  | |___  ___ _ __| \  / | ___ _ __  _   _ 
 | |  | / __|/ _ \ '__| |\/| |/ _ \ '_ \| | | |
 | |__| \__ \  __/ |  | |  | |  __/ | | | |_| |
  \____/|___/\___|_|  |_|  |_|\___|_| |_|\__,_|
                                               
                                               ")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Purchase", "Account Info","Exit"
                    }));

            switch (option)
            {
                case "Purchase":
                    PurchaseMenu(u);
                    break;
                case "Account Info":
                    UserInfoMenu(u);
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public static void PurchaseMenu(User u)
    {
        while (true)
        {
            try
            {
                Book b = new Book();


                AnsiConsole.Clear();

                var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[green]--Search Book To Purchase--[/]")
                    .PageSize(1000)
                    .AddChoices(new[] {
                    "Search by Name", "Search by Category", "Exit"
                        }));

                switch (option)
                {
                    case "Search by Name":
                        b = b.SearchByName();
                        Order d = new Order(u, b);
                        d.Transaction();
                        break;
                    case "Search by Category":
                        b = b.SearchByCategory();
                        Order e = new Order(u, b);
                        e.Transaction();
                        break;
                    case "Exit":
                        return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nNo book found!");
                Console.ReadLine();
            }
        }
    }

    public static void UserInfoMenu(User u)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[green]---User Info---[/]")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Orders History", "Update Account Info","Exit"
                    }));

            switch (option)
            {
                case "Orders History":
                    u.GetOrders(u);
                    break;
                case "Update Account Info":
                    updateInfoMenu(u);
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public static void updateInfoMenu(User u)
    {
        while (true)
        {
            AnsiConsole.Clear();

            u.userInfo(u);

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n-Select What to Change:")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Fullname", "Phone Number", "Address","Exit"
                    }));

            switch (option)
            {
                case "Fullname":
                    u.updateInfo(u, "user_name", "Fullname");
                    break;
                case "Phone Number":
                    u.updateInfo(u, "phone", "Phone Number");
                    break;
                case "Address":
                    u.updateInfo(u, "address", "Address");
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public static void adminMenu(User u)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(@"              _           _       __  __                  
     /\      | |         (_)     |  \/  |                 
    /  \   __| |_ __ ___  _ _ __ | \  / | ___ _ __  _   _ 
   / /\ \ / _` | '_ ` _ \| | '_ \| |\/| |/ _ \ '_ \| | | |
  / ____ \ (_| | | | | | | | | | | |  | |  __/ | | | |_| |
 /_/    \_\__,_|_| |_| |_|_|_| |_|_|  |_|\___|_| |_|\__,_|
                                                          
                                                          ")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Manage Book", "Manage Users", "Orders List", "Exit"
                    }));

            switch (option)
            {
                case "Manage Book":
                    manageBookMenu();
                    break;
                case "Manage Users":
                    manageUserMenu();
                    break;
                case "Orders List":
                    u.displayOrders();
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public static void manageBookMenu()
    {
        try
        {
            AnsiConsole.Clear();

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[green]---Manage Books---[/]")
                .PageSize(1000)
                .AddChoices(new[] {
                    "Add Book", "Update Book Info", "Exit"
                    }));

            switch (option)
            {
                case "Add Book":
                    Book c = new Book();

                    c.Isbn = AnsiConsole.Ask<string>("Enter book's [green]ISBN[/]:");
                    while (c.bookExist(c.Isbn))
                    {
                        AnsiConsole.MarkupLine("[red]Book Exist![/]");
                        c.Isbn = AnsiConsole.Ask<string>("Enter book's [green]ISBN[/]:");
                    }

                    c.Name = AnsiConsole.Ask<string>("Enter book's [green]Title[/]:");
                    c.Author = AnsiConsole.Ask<string>("Enter book's [green]Author[/]:");
                    c.Category = AnsiConsole.Ask<string>("Enter book's [green]Category[/]:");
                    while (c.Price <= 0) c.Price = AnsiConsole.Ask<decimal>("Enter book's [green]Price[/]:");
                    while (c.Quantity <= 0) c.Quantity = AnsiConsole.Ask<int>("Enter book's [green]Quantity[/]:");
                    c.ReleaseDate = AnsiConsole.Ask<DateOnly>("Enter book's [green]Release Date(yyyy/mm/dd)[/]:");
                    string date = c.ReleaseDate.ToString("yyyy-MM-dd");

                    char opt = AnsiConsole.Ask<char>("[yellow]Confirm(y/n)[/]:");
                    if (opt == 'y') c.addBook(c, date);

                    break;
                case "Update Book Info":
                    c = new Book();
                    c = c.SearchByName();
                    c.updateBook(c);
                    break;
                case "Exit":
                    return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
    }

    public static void manageUserMenu()
    {
        try
        {
            User u = new User();
            u.updateUserStatus(u);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadLine();
        }
    }

    static void Main(string[] args)
    {
        while (true)
        {
            AnsiConsole.Clear();

            User u = new User();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(@"  ______      _      _____ ____  _____            _______     __
 |  ____|    | |    |_   _|  _ \|  __ \     /\   |  __ \ \   / /
 | |__ ______| |      | | | |_) | |__) |   /  \  | |__) \ \_/ / 
 |  __|______| |      | | |  _ <|  _  /   / /\ \ |  _  / \   /  
 | |____     | |____ _| |_| |_) | | \ \  / ____ \| | \ \  | |   
 |______|    |______|_____|____/|_|  \_\/_/    \_\_|  \_\ |_|   
                                                                
                                                                ")
                    .PageSize(1000)
                    .AddChoices(new[] {
                        "Login", "Sign In", "Exit"
                    }));

            switch (option)
            {
                case "Login":
                    loginMenu(u);
                    break;
                case "Sign In":
                    u.SignIn();
                    break;
                case "Exit":
                    return;
            }
        }
    }
}
