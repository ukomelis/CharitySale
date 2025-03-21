// pages/SalePage.jsx
import React, {useEffect, useState} from 'react';
import {createSale, getItems, updateItemQuantity} from '../api/apiClient';
import ItemCard from '../components/ItemCard';
import CheckoutModal from '../components/CheckoutModal';
import FloatingCart from '../components/FloatingCart';
import ReceiptModal from '../components/ReceiptModal';
import InventoryAdjustmentModal from '../components/InventoryAdjustmentModal';
import signalRService from '../services/signalRService';
import { CATEGORY_MAPPING, CATEGORY_COLORS } from '../constants/constants';

import './SalePage.css';

const SalePage = () => {
  const [items, setItems] = useState([]);
  const [cart, setCart] = useState([]);
  const [totalAmount, setTotalAmount] = useState(0);
  const [showCheckout, setShowCheckout] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [completedSale, setCompletedSale] = useState(null);
  const [showReceipt, setShowReceipt] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [compactView, setCompactView] = useState(false);
  const [showInventoryModal, setShowInventoryModal] = useState(false);

  // Fetch items on component mount
  useEffect(() => {
    fetchItems();

    // Show inventory adjustment modal automatically on first load
    setShowInventoryModal(true);
  }, []);

    // Set up SignalR event listeners
    useEffect(() => {
      // Listen for item quantity updates from other clients
      const unsubscribeQuantity = signalRService.onItemQuantityUpdated((itemId, newQuantity) => {
        setItems(prevItems => 
          prevItems.map(item => 
            item.id === itemId 
              ? { ...item, quantity: newQuantity } 
              : item
          )
        );
      });
      
      // Listen for new sales created by other clients
      const unsubscribeSale = signalRService.onSaleCreated((saleId) => {
        //refresh the items to get updated inventory
        fetchItems();
      });
      
      // Clean up listeners when component unmounts
      return () => {
        unsubscribeQuantity();
        unsubscribeSale();
      };
    }, []);
  

  // Calculate total when cart changes
  useEffect(() => {
    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    setTotalAmount(total);
  }, [cart]);

  // Handle item selection (adding to cart)
  const handleItemSelect = (selectedItem) => {
    // Check if item has enough stock
    const itemInStock = items.find(i => i.id === selectedItem.id);
    if (!itemInStock || itemInStock.quantity <= 0) {
      return;
    }

    // Update local items stock immediately
    setItems(prevItems => 
      prevItems.map(item => 
        item.id === selectedItem.id 
          ? { ...item, quantity: item.quantity - 1 } 
          : item
      )
    );

    // Add item to cart or increase quantity if already in cart
    setCart(prevCart => {
      const existingItem = prevCart.find(item => item.id === selectedItem.id);
      if (existingItem) {
        return prevCart.map(item => 
          item.id === selectedItem.id 
            ? { ...item, quantity: item.quantity + 1 } 
            : item
        );
      } else {
        return [...prevCart, { ...selectedItem, quantity: 1 }];
      }
    });
  };

  const fetchItems = async () => {
    try {
      const data = await getItems();
      setItems(data);
      setLoading(false);

    } catch (err) {
      setError('Failed to load items. Please refresh the page.');
      setLoading(false);
    }
  };

  // Reset cart and completed sale
  const handleReset = () => {
    // Restore item quantities in the UI
    if (cart.length > 0) {
      setItems(prevItems => {
        const updatedItems = [...prevItems];
        cart.forEach(cartItem => {
          const itemIndex = updatedItems.findIndex(item => item.id === cartItem.id);
          if (itemIndex !== -1) {
            updatedItems[itemIndex] = {
              ...updatedItems[itemIndex],
              quantity: updatedItems[itemIndex].quantity + cartItem.quantity
            };
          }
        });
        return updatedItems;
      });
    }
    
    setCart([]);
    setCompletedSale(null);
  };

  // Handle checkout confirmation
  const handleCheckout = () => {
    if (cart.length > 0) {
      setShowCheckout(true);
    }
  };

  // Handle payment confirmation
  const handleConfirmPayment = async (amountPaid) => {
    try {
      const saleData = {
        items: cart.map(item => ({
          itemId: item.id,
          itemName: item.name,
          quantity: item.quantity
        })),
        amountPaid: parseFloat(amountPaid)
      };

      // Send sale to API
      const response = await createSale(saleData);
      setCompletedSale(response);

      // Close checkout modal and show receipt
      setShowCheckout(false);
      setShowReceipt(true);
    } catch (err) {
      // Instead of setting error in SalePage
      return err?.response?.data || err?.message || 'Failed to process sale. Please try again.'; // Return the error message to be handled by CheckoutModal
    }
  };

  // Handle receipt modal close
  const handleReceiptClose = () => {
    setShowReceipt(false);
    // Reset cart when closing receipt
    setCart([]);
    setCompletedSale(null);
  };

  // Handle inventory adjustments
  const handleSaveInventory = async (adjustedItems) => {
    try {
      // Update quantities in the database
      for (const item of adjustedItems) {
        // Only send API requests for items whose quantities have changed
        const originalItem = items.find(i => i.id === item.id);
        if (originalItem && originalItem.quantity !== item.quantity) {
          await updateItemQuantity(item.id, item.quantity);
        }
      }
      
      // Update local state with the adjusted quantities
      setItems(adjustedItems);
      
    } catch (err) {
      setError('Failed to update inventory. Please try again.');
      console.error(err);
    }
  };

  // Toggle view mode
  const toggleViewMode = () => {
    setCompactView(!compactView);
  };
  
  // Show inventory adjustment modal
  const openInventoryAdjustment = () => {
    setShowInventoryModal(true);
  };

  // Filter items based on search query
  const filteredItems = searchQuery 
    ? items.filter(item => 
        item.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        item.description?.toLowerCase().includes(searchQuery.toLowerCase())
      )
    : items;

  // Group items by category for display, using the numeric category value
  const groupedItems = filteredItems.reduce((acc, item) => {
    // Use the category number as the key
    const categoryId = item.categoryId || item.category;
    if (!acc[categoryId]) {
      acc[categoryId] = [];
    }
    acc[categoryId].push(item);
    return acc;
  }, {});

  // Sort categories according to enum order (1: Food, 2: Clothes, 3: Other)
  const sortedCategories = Object.keys(groupedItems).sort((a, b) => parseInt(a) - parseInt(b));

  return (
    <div className="sale-page-container">
      {error && (
        <div className="alert-container">
          <div className="alert-error">
            <i className="fas fa-exclamation-circle"></i>
            {error}
          </div>
        </div>
      )}
      
      <header className="sale-page-header">
        <div className="header-content">
          <h1>Charity Sale</h1>
          <div className="header-controls">
            <button 
              className="inventory-btn"
              onClick={openInventoryAdjustment}
              title="Adjust inventory quantities"
            >
              <i className="fas fa-boxes"></i> Inventory
            </button>
            <div className="search-container">
              <input
                type="text"
                className="search-input"
                placeholder="Search items..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
              <i className="fas fa-search search-icon"></i>
            </div>
            <button 
              className={`view-toggle-btn ${compactView ? 'compact-active' : ''}`}
              onClick={toggleViewMode}
              title={compactView ? "Switch to normal view" : "Switch to compact view"}
            >
              <i className={`fas fa-${compactView ? 'th' : 'th-large'}`}></i>
            </button>
          </div>
        </div>
      </header>

      {loading ? (
        <div className="loader-container">
          <div className="loader"></div>
          <p>Loading items...</p>
        </div>
      ) : (
        <div className={`sale-page-content ${compactView ? 'compact-view' : ''}`}>
          {sortedCategories.length === 0 ? (
            <div className="no-items-message">
              <i className="fas fa-search"></i>
              <p>No items found matching your search.</p>
            </div>
          ) : (
            sortedCategories.map(categoryId => {
              const categoryColor = CATEGORY_COLORS[categoryId] || { bg: '#f9f9f9', border: '#d9d9d9', icon: '📦' };
              
              return (
                <div key={categoryId} className="category-section" style={{ backgroundColor: categoryColor.bg, borderColor: categoryColor.border }}>
                  <div className="category-header">
                    <span className="category-icon">{categoryColor.icon}</span>
                    <h2>{CATEGORY_MAPPING[categoryId] || `Category ${categoryId}`}</h2>
                    <span className="item-count">{groupedItems[categoryId].length} items</span>
                  </div>
                  <div className={`items-grid ${compactView ? 'compact-grid' : ''}`}>
                    {groupedItems[categoryId].map(item => (
                      <ItemCard 
                        key={item.id} 
                        item={item} 
                        onItemSelect={handleItemSelect}
                        isOutOfStock={item.quantity <= 0}
                        categoryColor={categoryColor.border}
                        compact={compactView}
                      />
                    ))}
                  </div>
                </div>
              );
            })
          )}
        </div>
      )}
      
      {/* Floating Cart at bottom */}
      <FloatingCart 
        cart={cart}
        totalAmount={totalAmount}
        onCheckout={handleCheckout}
        onReset={handleReset}
      />
      
      {/* Checkout Modal */}
      <CheckoutModal 
        show={showCheckout}
        onClose={() => setShowCheckout(false)}
        totalAmount={totalAmount}
        onConfirmPayment={handleConfirmPayment}
      />
      
      {/* Receipt Modal */}
      <ReceiptModal
        show={showReceipt}
        sale={completedSale}
        onClose={handleReceiptClose}
      />
      
      {/* Inventory Adjustment Modal */}
      <InventoryAdjustmentModal
        show={showInventoryModal}
        onClose={() => setShowInventoryModal(false)}
        items={items}
        onSave={handleSaveInventory}
      />
    </div>
  );
};

export default SalePage;