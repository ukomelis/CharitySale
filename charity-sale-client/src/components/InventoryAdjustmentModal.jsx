// components/InventoryAdjustmentModal.jsx
import React, { useState, useEffect } from 'react';
import './InventoryAdjustmentModal.css';
import { CATEGORY_MAPPING } from '../constants/constants';


const InventoryAdjustmentModal = ({ show, onClose, items, onSave }) => {
  const [adjustedItems, setAdjustedItems] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    if (show && items) {
      // Make a deep copy of the items for editing
      setAdjustedItems(items.map(item => ({ ...item })));
    }
  }, [show, items]);

  const handleQuantityChange = (id, newQuantity) => {
    // Ensure quantity is at least 0
    const quantity = Math.max(0, parseInt(newQuantity) || 0);
    
    setAdjustedItems(prevItems => 
      prevItems.map(item => 
        item.id === id ? { ...item, quantity } : item
      )
    );
  };

  const handleSave = () => {
    onSave(adjustedItems);
    onClose();
  };

  // Filter items based on search
  const filteredItems = searchQuery
    ? adjustedItems.filter(item => 
        item.name.toLowerCase().includes(searchQuery.toLowerCase()))
    : adjustedItems;

  // Group items by category
  const groupedItems = filteredItems.reduce((acc, item) => {
    const categoryId = item.categoryId || item.category;
    if (!acc[categoryId]) {
      acc[categoryId] = [];
    }
    acc[categoryId].push(item);
    return acc;
  }, {});

  if (!show) return null;

  return (
    <div className="modal-backdrop">
      <div className="inventory-modal">
        <div className="inventory-modal-header">
          <h2>Adjust Inventory Quantities</h2>
          <div className="inventory-search-container">
            <input
              type="text"
              className="inventory-search-input"
              placeholder="Search items..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
            <i className="fas fa-search inventory-search-icon"></i>
          </div>
          <button className="close-button" onClick={onClose}>
            <i className="fas fa-times"></i>
          </button>
        </div>
        
        <div className="inventory-modal-content">
          {Object.keys(groupedItems).length === 0 ? (
            <div className="no-items-message">
              <i className="fas fa-search"></i>
              <p>No items found matching your search.</p>
            </div>
          ) : (
            Object.keys(groupedItems).sort().map(categoryId => (
              <div key={categoryId} className="inventory-category-section">
                <h3 className="inventory-category-header">
                  {CATEGORY_MAPPING[categoryId] || `Category ${categoryId}`}
                </h3>
                <div className="inventory-items-list">
                  {groupedItems[categoryId].map(item => (
                    <div key={item.id} className="inventory-item">
                      <div className="inventory-item-details">
                        <div className="inventory-item-image-container">
                          {item.imageUrl ? (
                            <img src={item.imageUrl} alt={item.name} className="inventory-item-image" />
                          ) : (
                            <div className="inventory-item-no-image">
                              <i className="fas fa-image"></i>
                            </div>
                          )}
                        </div>
                        <div className="inventory-item-info">
                          <h4 className="inventory-item-name">{item.name}</h4>
                          <p className="inventory-item-price">
                            {new Intl.NumberFormat('en-US', {
                              style: 'currency',
                              currency: 'USD'
                            }).format(item.price)}
                          </p>
                        </div>
                      </div>
                      
                      <div className="inventory-quantity-control">
                        <button 
                          className="quantity-btn"
                          onClick={() => handleQuantityChange(item.id, Math.max(0, item.quantity - 1))}
                        >
                          <i className="fas fa-minus"></i>
                        </button>
                        <input
                          type="number"
                          min="0"
                          value={item.quantity}
                          onChange={(e) => handleQuantityChange(item.id, e.target.value)}
                          className="quantity-input"
                        />
                        <button 
                          className="quantity-btn"
                          onClick={() => handleQuantityChange(item.id, item.quantity + 1)}
                        >
                          <i className="fas fa-plus"></i>
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            ))
          )}
        </div>
        
        <div className="inventory-modal-footer">
          <button className="cancel-btn" onClick={onClose}>Cancel</button>
          <button className="save-btn" onClick={handleSave}>
            <i className="fas fa-save"></i> Save Changes
          </button>
        </div>
      </div>
    </div>
  );
};

export default InventoryAdjustmentModal;