// components/ItemCard.jsx
import React from 'react';
import './ItemCard.css';

const ItemCard = ({ item, onItemSelect, isOutOfStock, categoryColor, compact = false }) => {
  const handleClick = () => {
    if (!isOutOfStock) {
      onItemSelect(item);
    }
  };
  
  // Format price with 2 decimal places
  const formattedPrice = new Intl.NumberFormat('en-ET', {
    style: 'currency',
    currency: 'EUR'
  }).format(item.price);
  
  return (
    <div 
      className={`item-card ${isOutOfStock ? 'out-of-stock' : ''} ${compact ? 'compact' : ''}`} 
      onClick={handleClick}
      style={{ borderColor: isOutOfStock ? '#d9d9d9' : categoryColor }}
    >
      {item.imageUrl && (
        <div className="item-image-container">
          <img src={item.imageUrl} alt={item.name} className="item-image" />
        </div>
      )}
      
      <div className="item-content">
        <h3 className="item-name">{item.name}</h3>
        
        {!compact && item.description && (
          <p className="item-description">{item.description}</p>
        )}
        
        <div className="item-details">
          <div className="item-price" style={{ color: categoryColor }}>
            {formattedPrice}
          </div>
          
          {!compact && (
            <div className="item-stock">
              {isOutOfStock ? (
                <span className="out-of-stock-label">Out of stock</span>
              ) : (
                <span className="stock-count">
                  <i className="fas fa-cubes"></i> {item.quantity} left
                </span>
              )}
            </div>
          )}
        </div>
        
        {!compact && (
          <button 
            className={`add-to-cart-btn ${isOutOfStock ? 'disabled' : ''}`}
            disabled={isOutOfStock}
            style={{ backgroundColor: isOutOfStock ? '#d9d9d9' : categoryColor }}
          >
            {isOutOfStock ? 'Out of stock' : 'Add to cart'}
          </button>
        )}
        
        {compact && !isOutOfStock && (
          <button 
            className="compact-add-btn"
            style={{ color: categoryColor }}
          >
            <i className="fas fa-plus-circle"></i>
          </button>
        )}
        
        {compact && isOutOfStock && (
          <span className="compact-out-label">Out</span>
        )}
      </div>
    </div>
  );
};

export default ItemCard;