// App.jsx
import React, { useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
import SalePage from './pages/SalePage';
import signalRService from './services/signalRService';

function App() {
  useEffect(() => {
    // Initialize SignalR connection when the app loads
    signalRService.initialize();
    
    // Clean up the connection when the app unmounts
    return () => {
      signalRService.disconnect();
    };
  }, []);
  
  return (
    <div className="container-fluid">
      <SalePage />
    </div>
  );
}

export default App;