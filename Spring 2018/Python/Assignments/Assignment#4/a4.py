class Customer:
    def __init__(self):
        self.first_name = ""
        self.last_name = ""
        self.phone = 0
        self.email = []
        self.date = 0
        self.history = {}
    def __str__(self):
    def add_contact(self):
        self.first_name = input("Enter customer's first name:")
        self.last_name = input("Enter customer's last name:")
        self.phone = input("Enter customer's phone no.:")
        self.email.append(input("Enter customer's email address(es):"))
        self.date = input("Enter today date:")
    def look_contact(self, last_name):
        
class ItemToPurchase:
    def __init__(self, item_name, item_price, item_quantity, item_description):
        self.item_name = item_name
        self.item_price = item_price
        self.item_quantity = item_quantity
        self.item_description = item_description
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
    def __init__(self, customer_name, current_date, cart_items=[]):
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
                  

customers_db = []
shopping_cart = []






























        
