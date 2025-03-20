import React, { useState } from 'react';
import './CheckoutModal.css'; // Make sure to import the CSS

const CheckoutModal = ({ show, onClose, totalAmount, onConfirmPayment }) => {
  const [amountPaid, setAmountPaid] = useState('');
  const [error, setError] = useState('');
  const [isProcessing, setIsProcessing] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const paid = parseFloat(amountPaid);

    if (isNaN(paid) || paid < totalAmount) {
      setError(`Amount must be at least ${totalAmount.toFixed(2)} €`);
      return;
    }

    setIsProcessing(true);
    const errorResult = await onConfirmPayment(paid);

    if (errorResult) {
      // If onConfirmPayment returns an error message, display it
      setError(errorResult);
      setIsProcessing(false);
    }
  };

  if (!show) return null;

  return (
      <div className="modal-backdrop">
        <div className="checkout-modal">
          <div className="modal-header">
            <h2>Checkout</h2>
            <button className="close-button" onClick={onClose}>&times;</button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Total Amount</label>
              <input
                  type="text"
                  value={`${totalAmount.toFixed(2)} €`}
                  disabled
              />
            </div>
            <div className="form-group">
              <label>Cash Amount Paid</label>
              <input
                  type="number"
                  step="0.01"
                  min={totalAmount}
                  value={amountPaid}
                  onChange={(e) => {
                    setAmountPaid(e.target.value);
                    setError('');
                  }}
                  required
              />
            </div>

            {/* Display error message in a prominent way */}
            {error && (
                <div className="error-message">
                  <span className="error-icon">⚠️</span>
                  {error}
                </div>
            )}

            {/* Show change due if amount paid > total */}
            {parseFloat(amountPaid) > totalAmount && (
                <div className="change-due">
                  Change due: {(parseFloat(amountPaid) - totalAmount).toFixed(2)} €
                </div>
            )}

            <div className="modal-actions">
              <button
                  type="button"
                  className="cancel-button"
                  onClick={onClose}
              >
                Cancel
              </button>
              <button
                  type="submit"
                  className="confirm-button"
                  disabled={isProcessing}
              >
                {isProcessing ? 'Processing...' : 'Complete Payment'}
              </button>
            </div>
          </form>
        </div>
      </div>
  );
};

export default CheckoutModal;