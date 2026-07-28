using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
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

        /// ////////////////////////////////////////////////////////////////

        // Case 4: Add New Product
        static void AddProduct()
        {

            Console.WriteLine("\n--- Add New Product ---");


            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();


            Console.Write("Enter Product Price: ");
            double price = double.Parse(Console.ReadLine());


            // Show categories
            var categories = context.categories.ToList();


            if (categories.Count == 0)
            {
                Console.WriteLine("No categories available. Add a category first.");
                return;
            }


            Console.WriteLine("\nAvailable Categories:");

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.Id}. {category.Name}");
            }


            Console.Write("Choose Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());


            Category selectedCategory = context.categories
                .FirstOrDefault(c => c.Id == categoryId);


            if (selectedCategory == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }


            Product product = new Product
            {
                Name = name,
                Price = price,
                CategoryId = categoryId
            };


            context.products.Add(product);
            context.SaveChanges();


            Console.WriteLine("Product added successfully!");
        }


        /// /////////////////////////////////////////////////////////////

        // Case 5: View All Products
        static void ViewAllProducts()
        {
            Console.WriteLine("\n--- View All Products ---");


            Console.Write("Filter by category? (y/n): ");
            string answer = Console.ReadLine();


            var products = context.products
                .Include(p => p.Category)
                .AsQueryable();


            if (answer.ToLower() == "y")
            {
                Console.Write("Enter Category Name: ");
                string categoryName = Console.ReadLine();


                products = products.Where(
                    p => p.Category.Name == categoryName
                );
            }


            var productList = products.ToList();


            if (productList.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }


            foreach (var product in productList)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Category: {product.Category.Name}");
            }
        }


        /// ///////////////////////////////////////////////////////////////

        // Case 6: Place an Order
        static void PlaceOrder()
        {
            if (loggedInUserId == 0)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            var products = context.products.ToList();

            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

            Console.WriteLine("\nAvailable Products:");

            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}. {p.Name} - {p.Price}");
            }

            Order order = new Order
            {
                UserId = loggedInUserId,
                OrderDate = DateTime.Now
            };

            context.orders.Add(order);
            context.SaveChanges();

            string choice;

            do
            {
                Console.Write("Enter Product ID: ");
                int productId = int.Parse(Console.ReadLine());

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                OrderProduct orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                context.Set<OrderProduct>().Add(orderProduct);

                Console.Write("Add another product? (y/n): ");
                choice = Console.ReadLine().ToLower();

            } while (choice == "y");

            context.SaveChanges();

            Console.WriteLine("Order placed successfully!");
        }


        /// /////////////////////////////////////////////////////
        // Case 7: View My Orders
        static void ViewMyOrders()
        {
            if (loggedInUserId == 0)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            var orders = context.orders
                .Where(o => o.UserId == loggedInUserId)
                .ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Date: {order.OrderDate}");
            }
        }

        /// ///////////////////////////////////////////////////////////////
        // Case 8: View Order Details

        static void ViewOrderDetails()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            var order = context.orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.Review)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            double total = 0;

            Console.WriteLine("\nProducts:");

            foreach (var item in order.OrderProducts)
            {
                double subtotal = item.Product.Price * item.Quantity;

                Console.WriteLine($"{item.Product.Name}  Qty:{item.Quantity}  Price:{item.Product.Price}");

                total += subtotal;
            }

            Console.WriteLine($"Total = {total}");

            if (order.Review != null)
            {
                Console.WriteLine($"Rating: {order.Review.Rating}");
                Console.WriteLine($"Comment: {order.Review.Comment}");
            }
            else
            {
                Console.WriteLine("No review.");
            }
        }
        
        /// ///////////////////////////////////////////////////////////////
        // Case 9: Add a Review for an Order

        static void AddReview()
        {
            if (loggedInUserId == 0)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            var order = context.orders
                .Include(o => o.Review)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            if (order.UserId != loggedInUserId)
            {
                Console.WriteLine("This order does not belong to you.");
                return;
            }

            if (order.Review != null)
            {
                Console.WriteLine("Review already exists.");
                return;
            }

            Console.Write("Rating (1-5): ");
            int rating = int.Parse(Console.ReadLine());

            Console.Write("Comment: ");
            string comment = Console.ReadLine();

            Review review = new Review
            {
                Rating = rating,
                Comment = comment,
                OrderId = orderId
            };

            context.reviews.Add(review);
            context.SaveChanges();

            Console.WriteLine("Review added successfully.");
        }

        /// ///////////////////////////////////////////////////////////////
        // Case 10: View All Reviews for a Product

        static void ViewReviewsForProduct()
        {
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            var orderProducts = context.Set<OrderProduct>()
                .Include(op => op.Order)
                .ThenInclude(o => o.Review)
                .Where(op => op.ProductId == productId)
                .ToList();

            if (orderProducts.Count == 0)
            {
                Console.WriteLine("No orders found for this product.");
                return;
            }

            foreach (var item in orderProducts)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine($"Order ID: {item.OrderId}");

                if (item.Order.Review != null)
                {
                    Console.WriteLine($"Rating: {item.Order.Review.Rating}");
                    Console.WriteLine($"Comment: {item.Order.Review.Comment}");
                }
                else
                {
                    Console.WriteLine("No review.");
                }
            }
        }

        /// ///////////////////////////////////////////////////////////////
        // Case 11: Logout
        static void Logout()
        {
            static void Logout()
            {
                if (loggedInUserId == 0)
                {
                    Console.WriteLine("No user is currently logged in.");
                    return;
                }

                loggedInUserId = 0;
                Console.WriteLine("Logged out successfully.");
            }
        }
    }
}
