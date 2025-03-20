import React from 'react';

const ChangeDisplay = ({ change, totalChange }) => {
  return (
    <div className="mt-4">
      <h5>Change: {totalChange.toFixed(2)} €</h5>
      <div className="row">
        {change.map((item, index) => (
          item.count > 0 && (
            <div key={index} className="col-md-4 mb-2">
              <div className="d-flex align-items-center">
                <span className="badge bg-success me-2">{item.count}×</span>
                <span>{item.name}</span>
              </div>
            </div>
          )
        ))}
      </div>
    </div>
  );
};

export default ChangeDisplay;