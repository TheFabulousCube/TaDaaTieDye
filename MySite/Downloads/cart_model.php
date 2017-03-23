

<?php
/*** Marquardt 'Cube' Snell -->
/*** www.tadaatiedye.com ***/
/*** Cart_model.php ***/
/*** Shopping cart functions ***/


 abstract class ShoppingCart
{
	public $cart;
	public $message;
	public $cartTotal;
	public $itemCount; 
	abstract function addtocart($itemID, $qty);	
	abstract function updatecart($itemID, $qty);	
	abstract function removefromcart($item);	
	abstract function emptycart();
	
	abstract function gettotal();
	
	abstract function getcart();
	
	public function adjustQtyToInventory($itemID, $qty)
	{
		$db = read_only_connection();
		$query = "SELECT ProdQty FROM `Products` WHERE ProdId = '$itemID'";	
		$statement = $db->prepare($query);
		$statement->execute();
		$result = $statement->get_result(); 
		$row = $result->fetch_assoc();
		if ($qty > $row['ProdQty'])
		{
			$qty = $row['ProdQty'];
			switch (true)
			{ // Just to print plural/singular nicely
				case ($qty <= 0):
					$this->message = 'Sorry, there are none in stock.';
					break;
				
				case ($qty == 1):
					$this->message = 'Sorry, there is only 1 in stock.';
					break;
					
				case ($qty >= 2):
					$this->message = 'Sorry, there are only '.$qty.' in stock.';
					break;
			}
		}
		return $qty;		
	}
}

class SessionCart extends ShoppingCart
{ 
	public $cart;
	public $message;
	public $cartTotal;
	public $itemCount; 
	public function __construct()
	{

		$lifetime = 60 * 60 * 24 * 14;
		if (empty($_SESSION['cart'])) $_SESSION['cart'] = array();
		$this->cart = $_SESSION['cart'];
		$this->message = '';
	}
	
	public function addtocart($itemID, $qty)
	{
		// check availability
		$qty = $this->adjustQtyToInventory($itemID, $qty);
		if ($qty > 0)
		{/******  CHECK FOR QTY == 0 ****/
	
			//Item exists in cart, add quantity
			if (isset($_SESSION['cart'][$itemID])) 
			{
				$qty += $_SESSION['cart'][$itemId]['qty'];
				$_SESSION['cart'][$itemID]['qty'] += $qty;	
				$this->message = "Added another $itemName to your cart.";
			}
			else
			{
				// new item, add all data.
				// pull from db!
				$db = read_only_connection();
				/*** This is a really long query string!  Try it in a stored procedure? ***/
				$query = "SELECT p.ProdId, p.Catagory, p.ProdPrice,  m.name, p.ProdPicture, cat.Type
				FROM
						TaDaaTieDyeTPT.Catagory_Lookup AS cat,
						TaDaaTieDyeTPT.Products AS p
							LEFT JOIN
						TaDaaTieDyeTPT.Magnet_Details AS m ON p.ProdId = m.stateId
							LEFT JOIN
						TaDaaTieDyeTPT.Clothing_Details AS l ON p.ProdId = l.ClothingId
					WHERE p.ProdId = ?  AND p.Catagory = cat.CatId";
		$statement = $db->prepare($query);
		$statement->bind_param("s", $itemID);
		$success = $statement->execute();
		if ($success)
		{
			$result = $statement->get_result();
			if ($result->num_rows > 0)
			{
				while ($row = $result->fetch_assoc())
				{ // There are rows in the cart!
					echo var_dump($row);
					$_SESSION['cart'][$itemID]['itemName'] = ($row['name'] == null) ? $row['Type'] : $row['name'] ;	
					$_SESSION['cart'][$itemID]['catagory'] = $row['Catagory'];
					$_SESSION['cart'][$itemID]['picture'] = $row['ProdPicture'];
					$_SESSION['cart'][$itemID]['price'] = $row['ProdPrice'];
					$_SESSION['cart'][$itemID]['qty'] = $qty;	
					$this->message = "Added a(n) $itemName to your cart.";	
					}
				}
				else
				{echo "Didn't execute";}
			}}
		}
	} // end addtocart
	
	function updatecart($itemID, $qty)
	{
	
		$qty = $this->adjustQtyToInventory($itemID, $qty);

		$name = $_SESSION['cart'][$itemID]['itemName'];
		if ($qty <= 0)
		{
			unset($_SESSION['cart'][$itemID]);
		}
		else
		{
		$_SESSION['cart'][$itemID]['qty'] = $qty;
		}
		$this->message .= ' Updated '.$name.' to '.$qty;
		if (empty($_SESSION['cart'])) $this->message .= ' Your cart is empty';
	}
	
	function removefromcart($item)
	{
		
		$name = $_SESSION['cart'][$item]['itemName'];
		$qty = $_SESSION['cart'][$item]['qty'];
		$name = ($qty > 1) ? $name.'s' : $name ;
		if (isset($_SESSION['cart'][$item])) unset($_SESSION['cart'][$item]);
		$this->message = 'Removed '.$qty.' '.$name.' from your cart';
		if (empty($_SESSION['cart'])) 
		{
			$this->message .= ' Your cart is empty';
			unset($_SESSION['cart']);
		}
	}	
	
	function emptycart()
	{
		$_SESSION['cart'] = array();
		include './cart_view.php';
	}
	
	function gettotal()
	{
		//return $_SESSION['cart']['total'];
	}
	
	function getCart() 
	{
		$cart = $_SESSION['cart'];
		return $cart;
	}

}
 
class DBCart extends ShoppingCart
{
	public $cart;
	public $message;
	public $cartTotal;
	public $itemCount; 
		
	public function __construct()
	{
		$this->cart = array();
		$this->message = '';
	}
	
	
	public function addtocart($itemID, $qty)
	{
		$user = $_SESSION['user'];		
		/*************************************************************************/
				// check availability
		$qty = $this->adjustQtyToInventory($itemID, $qty);
		/**************************************************************************/
		if ($qty > 0)
		{
			 $db = crud_connection();
			 $query = "SELECT * FROM Cart WHERE UserId = ? AND ProdID = ?";
			 $statement = $db->prepare($query);
			 $statement->bind_param("ss", $user, $itemID);
			 $statement->execute();
			 $result = $statement->get_result();
			 
			 if($result->num_rows > 0)
			 {
				 // item alreadyexists, add to qty
				 $row = $result->fetch_assoc();
				 $qty += $row['qty'];
				 $query = "UPDATE Cart SET qty = ? WHERE UserId = ? AND ProdID = ?";
				 $statement = $db->prepare($query);
				 $statement->bind_param("iss", $qty, $user, $itemID);
				 $success = $statement->execute();
				 if ($db->affected_rows == 1)
				 {
					 $this->message = "Added $qty to your cart";
				 }
				 else
				 {
					 $errorMessage .= 'Somethings wrong.  It only worked on '.$db->affected_rows;
					 //include './cart_view.php';
				 }			 
			 }
			 else
			 {
				 // new item, add all data.
				 $query = "INSERT INTO Cart (UserId, ProdID, Qty)
				 VALUES (?, ?, ?)";
				 $statement = $db->prepare($query);
				 
				 $statement->bind_param("ssi", $user, $itemID, $qty);
				 $success = $statement->execute();
				 if (!$success || $db->affected_rows !=1)
				 {
					 $errorMessage .= 'Dang.  I couldn\'t find any '.$itemID.'s in your cart.';
				 }			 
			 }
			 $statement->close();
			 $this->message .= " Added $qty to your cart";
		} 
	} // end addtocart
	
	function updatecart($itemID, $qty)
	{
	
		$qty = $this->adjustQtyToInventory($itemID, $qty);
		if ($qty <= 0) 
		{ 
			$this->removefromcart($itemID);
		}
		else
		{
		$user = $_SESSION['user'];
		$db = crud_connection();
		$query = "UPDATE Cart SET Qty = ? WHERE UserId = ? and ProdID = ?";
		$statement = $db->prepare($query);
		$statement->bind_param("iss", $qty, $user, $itemID);
		$success = $statement->execute();
		if (!$success)
		{
			$errorMessage .= 'Dang.  I couldn\'t find any in your cart.';
		}
		$statement->close();
		$this->message .= ' Updated '.$itemID.' to '.$qty;
		}
	}
	
	function removefromcart($itemID)
	{
		$user = $_SESSION['user'];
		
		$db = crud_connection();
		$query = "DELETE FROM Cart WHERE UserId = ? AND ProdID = ?";
		$statement = $db->prepare($query);
		$statement->bind_param("ss", $user, $itemID);
		$statement->execute();
		$result = $statement->get_result();
		$this->message = 'Removed '.$itemID.' from your cart.';
	}
	
	function emptyCart()
	{
		
	}
	
	function gettotal()
	{
		
	}
	
	function getCart() 
	{
		$user = $_SESSION['user'];
		$cart = array ();
		$db = read_only_connection();
		$query = "SELECT 
    p.ProdId, m.name, p.ProdPicture, p.ProdPrice, c.Qty, cl.Type
FROM
    TaDaaTieDyeTPT.Cart AS c
        LEFT JOIN
    TaDaaTieDyeTPT.Products AS p ON c.ProdID = p.ProdId
        LEFT JOIN
    TaDaaTieDyeTPT.Magnet_Details AS m ON p.ProdId = m.stateId
        LEFT JOIN
    TaDaaTieDyeTPT.Clothing_Details AS l ON p.ProdId = l.ClothingId
        LEFT JOIN
    TaDaaTieDyeTPT.Catagory_Lookup AS cl ON p.Catagory = cl.CatId
WHERE
    c.UserId = ?";
		$statement = $db->prepare($query);
		$statement->bind_param("s", $user);
		$success = $statement->execute();
		if ($success)
		{
			$result = $statement->get_result();
			if ($result->num_rows > 0)
			{
				while ($row = $result->fetch_assoc())
				{ // There are rows in the cart!
			$name = ($row['name'] == null) ? $row['Type'] : $row['name'];
					$cart[$row['ProdId']] = array('itemName' => $name, 'catagory' => $row['Type'], 'price' => $row['ProdPrice'], 'qty' => $row['Qty'], 'picture' => $row['ProdPicture']);
				}
				
				$statement->close();
				$db->close();
			}
			else
			{
				$errorMessage .= 'No rows returned, cart must be empty?';
			}
		}
		else
		{
			$errorMessage .= 'Statement didn\'t execute.';
		}
		return $cart;
	}
	

}
?>
	