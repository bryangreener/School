#Name: Bryan Greener
#Date: 2018-02-23
#Homework: 4

class Customer:
    def __init__(self):
        self.first_name = ""
        self.last_name = ""
        self.phone = 0
        self.email = ""
        self.date = ""
        self.history = []
    def __str__(self):
        output = ""
        output = (("\n%s, %s - Phone Number: %s - Email Address(es): %s - Date: %s - Purchase history: {") %
                (self.last_name, self.first_name, self.phone, self.email, self.date))
        for i in customers_db:
            if(i.first_name == self.first_name and i.last_name == self.last_name):
                for j in i.history:
                    output += str(j) + ", "
                output += "}\n"
        return output
    def add_contact(self):
        self.first_name = input("Enter customer's first name:\n")
        self.last_name = input("Enter customer's last name:\n")
        self.phone = input("Enter customer's phone no.:\n")
        self.email = input("Enter customer's email address(es):\n")
        self.date = input("Enter today date:\n")
    def look_contact(self, last_name):
        return
class ItemToPurchase:
    def __init__(self):
        self.item_name = input("Enter the item name:\n")
        self.item_description = input("Enter the item description:\n")
        self.item_price = float(input("Enter the item price:\n"))
        self.item_quantity = int(input("Enter the item quantity:\n"))
    def __str__(self):
        return ("%s: %s") % (self.item_name, self.item_description)
    def print_item_cost(self):
        try:
            if(self.item_price <= 0):
                raise ValueError('Price cannot be <= 0.')
            else:
                print(("%s %d @ $%f = $%f") %
                      (self.item_name, self.item_quantity, self.item_price,
                       (self.item_price * self.item_quantity)))
        except ValueError as e:
            print(e)
class ShoppingCart:
    def __init__(self, customer_name="", current_date="", cart_items=[]):
        self.customer_name = customer_name
        self.current_date = current_date
        self.cart_items = cart_items
    def add_item(self, ItemToPurchase):
        self.cart_items.append(ItemToPurchase)
    def remove_item(self, ItemToRemove):
        try:
            for i in self.cart_items:
                if(i.item_name == ItemToRemove):
                    self.cart_items.remove(i)
                    return
            raise ValueError
        except ValueError:
            print("Item not found in cart. Nothing removed.")
    def modify_item(self, ItemToPurchase):
        try:
            for i in self.cart_items:
                if(i.item_name == ItemToPurchase):
                    i.item_quantity = int(input("Enter the new quantity:\n"))
                    return
            raise ValueError
        except ValueError:
            print("Item not found in cart. Nothing modified.")
    def return_item(self, Cust, ItemToRemove):
        try:
            for i in customers_db:
                if(i.first_name == Cust):
                    for j in i.history:
                        if(j.item_name == ItemToRemove):
                            i.history.remove(j)
                            print("The item is found and returned successfully")
                            return
            raise ValueError
        except ValueError:
            print("The item is not found")
                
    def get_num_items_in_cart(self):
        total_items = 0
        for i in self.cart_items:
            total_items += i.item_quantity
        return total_items
    def get_cost_of_cart(self):
        total_cost = 0.0
        for i in self.cart_items:
            total_cost += (i.item_price * i.item_quantity)
        return total_cost
    def print_total(self):
        try:
            print("Total: $", ShoppingCart.get_cost_of_cart(self))
        except ValueError:
            print("SHOPPING CART IS EMPTY.")
    def print_descriptions(self):
        for i in self.cart_items:
            print(i) #should use __str__ from ItemToPurchase
                  
def print_menu():
    options = ['a','r','c','u','i','o','q']
    while(1):
        print(  "\nMENU\n"
                "a - Add item to cart\n"
                "r - Remove item from cart\n"
                "c - Change item quantity\n"
                "u - Return items\n"
                "i - Output item descriptions\n"
                "o - Output shopping cart\n"
                "q - Quit\n")
        choice = input("\nChoose an option:\n")
        if(choice in options):
            return choice
        else:
            print("INVALID MENU OPTION")

customers_db = []
shopping_cart = []

try:
    num_customers = int(input("Enter the number of customers:\n"))
except ValueError as e:
    print(e)

for i in range(0, num_customers):
    customers_db.append(Customer())
    print("Enter customer info. #%d" % (i + 1))
    customers_db[i].add_contact()
    print(customers_db[i])
for i in range(0, num_customers):
    print(("Customer name: %s %s") % (customers_db[i].first_name, customers_db[i].last_name))
    print("Today's date:", customers_db[i].date)
    shopping_cart.append(ShoppingCart(customers_db[i].first_name, customers_db[i].date))
    shopping_cart[i].cart_items = []
    menu_choice = 'z'
    while(menu_choice != 'q'):
        menu_choice = print_menu()
        if(menu_choice == 'a'):
            print("\nADD ITEM TO CART")
            ShoppingCart.add_item(shopping_cart[i], ItemToPurchase())
            ShoppingCart.print_descriptions(shopping_cart[i])
        elif(menu_choice == 'r'):
            print("\nREMOVE ITEM FROM CART")
            ShoppingCart.remove_item(shopping_cart[i], input("Enter name of item to remove:\n"))
        elif(menu_choice == 'c'):
            print("\nCHANGE ITEM QUANTITY")
            ShoppingCart.modify_item(shopping_cart[i], input("Enter the item name:\n"))
        elif(menu_choice == 'u'):
            print("\nRETURN ITEM")
            cnametoreturn = input("Enter the customer name:\n")
            for j in customers_db:
                if(j.first_name == cnametoreturn):
                    print(j)
                    ShoppingCart.return_item(shopping_cart[i], cnametoreturn,
                                             input("Enter name of item to return:\n"))
                    print(j)
        elif(menu_choice == 'i'):
            print(("\nOUTPUT ITEM'S DESCRIPTIONS"
                  "%s %s's Shopping Cart - %s\n"
                  "Item Descriptions\n") %
                  (customers_db[i].first_name, customers_db[i].last_name, customers_db[i].date))
            ShoppingCart.print_descriptions(shopping_cart[i])
        elif(menu_choice == 'o'):
            print(("\nOUTPUT SHOPPING CART"
                  "%s's Shopping Cart - %s\n"
                  "Number of Items: %d\n") %
                  (customers_db[i].first_name, shopping_cart[i].current_date,
                  ShoppingCart.get_num_items_in_cart(shopping_cart[i])))
            for j in shopping_cart[i].cart_items:
                ItemToPurchase.print_item_cost(j)
            print("\n")
            ShoppingCart.print_total(shopping_cart[i])
        elif(menu_choice == 'q'):
            for j in shopping_cart[i].cart_items:
                customers_db[i].history.append(j)





























        
