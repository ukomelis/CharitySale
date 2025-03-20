import React from 'react';
import ChangeDisplay from './ChangeDisplay';

const ReceiptModal = ({ sale, show, onClose }) => {
  if (!show || !sale) return null;
  
  return (
    <div className="modal d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Sale Receipt</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <div className="modal-body">
            <div className="container">
              <div className="row mb-3">
                <div className="col">
                  <h5>Receipt Details</h5>
                  <p>
                    <strong>Sale ID:</strong> {sale.id}<br />
                    <strong>Date:</strong> {new Date(sale.createdAt).toLocaleString()}<br />
                    <strong>Total Amount:</strong> {sale.totalAmount.toFixed(2)} €<br />
                    <strong>Amount Paid:</strong> {sale.amountPaid.toFixed(2)} €<br />
                  </p>
                </div>
              </div>
              
              <div className="row">
                <div className="col">
                  <h5>Items Purchased</h5>
                  <table className="table table-striped">
                    <thead>
                      <tr>
                        <th>Item</th>
                        <th>Price</th>
                        <th>Quantity</th>
                        <th>Subtotal</th>
                      </tr>
                    </thead>
                    <tbody>
                      {sale.items.map((item, index) => (
                        <tr key={index}>
                          <td>{item.itemName}</td>
                          <td>{item.unitPrice.toFixed(2)} €</td>
                          <td>{item.quantity}</td>
                          <td>{(item.unitPrice * item.quantity).toFixed(2)} €</td>
                        </tr>
                      ))}
                    </tbody>
                    <tfoot>
                      <tr>
                        <td colSpan="3" className="text-end"><strong>Total:</strong></td>
                        <td><strong>{sale.totalAmount.toFixed(2)} €</strong></td>
                      </tr>
                    </tfoot>
                  </table>
                </div>
              </div>
              
              {sale.changeAmount > 0 && (
                <div className="row">
                  <div className="col">
                    <ChangeDisplay change={sale.change} totalChange={sale.changeAmount} />
                  </div>
                </div>
              )}
            </div>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-primary" onClick={onClose}>Close</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ReceiptModal;