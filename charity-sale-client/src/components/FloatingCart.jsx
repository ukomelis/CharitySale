import React, { useState } from 'react';

const FloatingCart = ({ cart, totalAmount, onCheckout, onReset }) => {
  const [expanded, setExpanded] = useState(false);

  if (cart.length === 0) {
    return null;
  }

  return (
    <div 
      className="position-fixed bottom-0 start-0 end-0" 
      style={{ 
        backgroundColor: '#fff', 
        boxShadow: '0 -2px 10px rgba(0,0,0,0.1)',
        zIndex: 1000,
        maxHeight: expanded ? '50vh' : '80px',
        transition: 'max-height 0.3s ease-in-out',
        overflow: 'hidden'
      }}
    >
      <div className="container py-3">
        <div className="d-flex justify-content-between align-items-center">
          <div>
            <button 
              className="btn btn-sm btn-outline-secondary" 
              onClick={() => setExpanded(!expanded)}
            >
              {expanded ? 'Hide Details' : 'Show Details'}
            </button>
            <span className="ms-3">
              Cart: {cart.length} {cart.length === 1 ? 'item' : 'items'} | 
              Total: <strong>{totalAmount.toFixed(2)} €</strong>
            </span>
          </div>
          <div>
            <button 
              className="btn btn-danger me-2" 
              onClick={onReset}
            >
              Clear
            </button>
            <button 
              className="btn btn-primary" 
              onClick={onCheckout}
            >
              Checkout
            </button>
          </div>
        </div>
        
        {expanded && (
          <div className="mt-3" style={{ maxHeight: '40vh', overflowY: 'auto' }}>
            <table className="table table-sm">
              <thead>
                <tr>
                  <th>Item</th>
                  <th>Price</th>
                  <th>Quantity</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tbody>
                {cart.map((item, index) => (
                  <tr key={index}>
                    <td>{item.name}</td>
                    <td>{item.price.toFixed(2)} €</td>
                    <td>{item.quantity}</td>
                    <td>{(item.price * item.quantity).toFixed(2)} €</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default FloatingCart;