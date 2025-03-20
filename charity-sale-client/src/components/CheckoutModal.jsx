import React, { useState } from 'react';

const CheckoutModal = ({ show, onClose, totalAmount, onConfirmPayment }) => {
  const [amountPaid, setAmountPaid] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    const paid = parseFloat(amountPaid);
    
    if (isNaN(paid) || paid < totalAmount) {
      setError(`Amount must be at least ${totalAmount.toFixed(2)} €`);
      return;
    }
    
    onConfirmPayment(paid);
  };

  if (!show) return null;

  return (
    <div className="modal d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Checkout</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body">
              <div className="mb-3">
                <label className="form-label">Total Amount</label>
                <input 
                  type="text" 
                  className="form-control" 
                  value={`${totalAmount.toFixed(2)} €`} 
                  disabled 
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Cash Amount Paid</label>
                <input 
                  type="number" 
                  className="form-control" 
                  step="0.01" 
                  min={totalAmount} 
                  value={amountPaid} 
                  onChange={(e) => {
                    setAmountPaid(e.target.value);
                    setError('');
                  }} 
                  required 
                />
                {error && <div className="text-danger">{error}</div>}
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>Cancel</button>
              <button type="submit" className="btn btn-primary">Complete Payment</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CheckoutModal;