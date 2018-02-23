class Customer:
    def __init__(self):
        self.first_name = ""
        self.last_name = ""
        self.phone = 0
        self.email = []
        self.date = 0
        self.history = {}
    def __str__(self):
        return
    def add_contact(self):
        self.first_name = input("Enter customer's first name:")
        self.last_name = input("Enter customer's last name:")
        self.phone = input("Enter customer's phone no.:")
        self.email.append(input("Enter customer's email address(es):"))
        self.date = input("Enter today date:")
    def look_contact(self, last_name):
        return
class ItemToPurchase:
    def __init__(self):
        self.item_name = input("Enter item name:")
        self.item_price = input("Enter item price:")
        self.item_quantity = input("Enter item quantity:")
        self.item_description = input("Enter item description:")
    def __str__(self):
        return ("%s: %s") % (self.name, self.description)
    def print_item_cost(self):
        try:
            if(self.price <= 0):
                raise ValueError('Price cannot be <= 0.')
            else:
                print(("%s %d @ $%f = $%f") %
                      (self.name, self.quantity, self.price,
                       (self.price * self.quantity)))
        except ValueError as e:
            print(e)
class ShoppingCart:
    def __init__(self, customer_name="", current_date="", cart_items=[]):
        self.customer_name = customer_name
        self.current_date = current_date
    def add_item(self, ItemToPurchase):
        self.cart_items.append(ItemToPurchase)
    def remove_item(self, ItemToRemove):
        try:
            self.cart_items.remove(ItemToRemove)
        except ValueError:
            print("Item not found in cart. Nothing removed.")
    def modify_item(self, ItemToPurchase):
        try:
            self.cart_items[ItemToPurchase].quantity = input("Enter new quantity:")
        except ValueError:
            print("Item not found in card. Nothing modified.")
    def return_item(self, Cust, ItemToRemove):
        return
    def get_num_items_in_cart():
        total_items = 0
        for i in shopping_cart:
            total_items += shopping_cart[i].quantity
        return total_items
    def get_cost_of_cart():
        total_cost = 0.0
        for i in shopping_cart:
            total_cost += (shopping_cart[i].cost * shopping_cart[i].quantity)
        return total_cost
    def print_total():
        try:
            print("Total:" % get_cost_of_cart())
        except ValueError:
            print("SHOPPING CART IS EMPTY.")
    def print_descriptions():
        for i in shopping_cart:
            print(shopping_cart[i]) #should use __str__ from ItemToPurchase
                  
def print_menu():
    options = ['a','r','c','u','i','o','q']
    while(1):
        print(  "MENU\n"
                "a - Add item to cart\n"
                "r - Remove item from cart\n"
                "c - Change item quantity\n"
                "u - Return items\n"
                "i - Output item descriptions\n"
                "o - Output shopping cart\n"
                "q - Quit\n")
        choice = input("Choose an option::")
        if(choice in options):
            return choice
        else:
            print("INVALID MENU OPTION")

customers_db = []
shopping_cart = []

try:
    num_customers = int(input("Enter the number of customers:"))
except ValueError as e:
    print(e)

for i in range(0, num_customers):
    customers_db.append(Customer())
    shopping_cart.append(ShoppingCart())

    menu_choice = print_menu()
    if(menu_choice == 'a'):
        new_item = ItemToPurchase()
        ShoppingCart.add_item(new_item)
    elif(menu_choice == 'r'):
        ShoppingCart.remove_item(shopping_cart[i])
    elif(menu_choice == 'c'):
        print("")
    elif(menu_choice == 'u'):
        print("")
    elif(menu_choice == 'i'):
        print("")
    elif(menu_choice == 'o'):
        print("")
    elif(menu_chocie == 'q'):
        print("")
    else:
        print("Somehow no menu option")





























        
