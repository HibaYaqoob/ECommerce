using ECommerce.Models;
namespace ECommerce
{
    public class Program
    {
        // Shared DbContext - created ONCE, here, so every function below reuses
        static ProjectContext context = new ProjectContext();
        // the exact same instance instead of each function opening its own.
        //static AppDbContext context = new AppDbContext();
        // Shared login state - 0 means "nobody is logged in".
        // Set by Login(), read by any function that requires a logged-in user,
        // reset back to 0 by Logout()

        static int loggedInUserId = 0;


        static void Main(string[] args)
        { 

            bool exitApp = false;
            while (!exitApp)
            {
                Console.WriteLine("\n===== E-Commerce Console App =====");
                Console.WriteLine(" 1. Register New User");
                Console.WriteLine(" 2. Login");
                Console.WriteLine(" 3. Add New Category");
                Console.WriteLine(" 4. Add New Product");
                Console.WriteLine(" 5. View All Products");
                Console.WriteLine(" 6. Place an Order");
                Console.WriteLine(" 7. View My Orders");
                Console.WriteLine(" 8. View Order Details");
                Console.WriteLine(" 9. Add a Review for an Order");
                Console.WriteLine("10. View All Reviews for a Product");
                Console.WriteLine("11. Logout");
                Console.WriteLine(" 0. Exit");
                Console.Write("Enter your choice: ");
                int choice;
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                switch (choice)
                {
                    case 1: RegisterUser(); break;
                    case 2: Login(); break;
                    case 3: AddCategory(); break;
                    case 4: AddProduct(); break;
                    case 5: ViewAllProducts(); break;
                    case 6: PlaceOrder(); break;
                    case 7: ViewMyOrders(); break;
                    case 8: ViewOrderDetails(); break;
                    case 9: AddReview(); break;
                    case 10: ViewReviewsForProduct(); break;
                    case 11: Logout(); break;
                    case 0:
                        exitApp = true;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // ===================== FUNCTIONS =====================
        // Every function below talks to the console itself AND uses the
        // shared "context" field declared above - never create a new
        // AppDbContext() inside any of these functions.


        /// //////////////////////////////////////////////////////////////
        // Case 1: Register New User
        static void RegisterUser()
        {
            
            try
            {
                Console.WriteLine("\n--- Register New User ---");

                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();


                User user = new User
                {
                    Name = name,
                    Email = email,
                    Password = password
                };


                context.users.Add(user);

                int result = context.SaveChanges();


                if (result > 0)
                {
                    Console.WriteLine("User registered successfully!");
                }
                else
                {
                    Console.WriteLine("User was not saved.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("DETAILS:");
                    Console.WriteLine(ex.InnerException.Message);
                }
            }


            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        //// //////////////////////////////////////////////////////////////

        // Case 2: Login

        static void Login()
        {
            Console.WriteLine("\n--- Login ---");

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();


            User user = context.users
                .FirstOrDefault(u => u.Email == email && u.Password == password);


            if (user != null)
            {
                loggedInUserId = user.Id;
                Console.WriteLine($"Welcome {user.Name}!");
                Console.WriteLine("Login successful.");
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
            }
        }

        /// //////////////////////////////////////////////////////////////
        
        // Case 3: Add New Category
        static void AddCategory()
        {
            Console.WriteLine("\n--- Add New Category ---");

            Console.Write("Enter Category Name: ");
            string name = Console.ReadLine();


            Category category = new Category
            {
                Name = name
            };


            context.categories.Add(category);
            context.SaveChanges();


            Console.WriteLine("Category added successfully!");
        }
        static void AddProduct()
        {
            // TODO: implement
        }
        static void ViewAllProducts()
        {
            // TODO: implement
        }
        static void PlaceOrder()
        {
            // TODO: implement - check loggedInUserId != 0 first
        }
        static void ViewMyOrders()
        {
            // TODO: implement - check loggedInUserId != 0 first
        }
        static void ViewOrderDetails()
        {
            // TODO: implement
        }
        static void AddReview()
        {
            // TODO: implement - check loggedInUserId != 0 first
        }
        static void ViewReviewsForProduct()
        {
            // TODO: implement
        }
        
        static void Logout()
        {
            // TODO: implement - reset loggedInUserId back to 0
        }
    }
}
