// services/signalRService.js
import * as signalR from "@microsoft/signalr";

class SignalRService {
  constructor() {
    this.connection = null;
    this.callbacks = {
      itemQuantityUpdated: [],
      saleCreated: []
    };
  }

  // Initialize connection to the SignalR hub
  async initialize() {
    try {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:8080/charitySaleHub")
        .withAutomaticReconnect()
        .build();

      // Register handlers to receive messages from the server
      this.connection.on("ItemQuantityUpdated", (itemId, newQuantity) => {
        this.callbacks.itemQuantityUpdated.forEach(callback => 
          callback(itemId, newQuantity)
        );
      });

      this.connection.on("SaleCreated", (saleId) => {
        this.callbacks.saleCreated.forEach(callback => 
          callback(saleId)
        );
      });

      await this.connection.start();
      console.log("SignalR connected.");
      return true;
    } catch (error) {
      console.error("SignalR connection error:", error);
      return false;
    }
  }

  // Add event listeners
  onItemQuantityUpdated(callback) {
    this.callbacks.itemQuantityUpdated.push(callback);
    return () => {
      this.callbacks.itemQuantityUpdated = this.callbacks.itemQuantityUpdated
        .filter(cb => cb !== callback);
    };
  }

  onSaleCreated(callback) {
    this.callbacks.saleCreated.push(callback);
    return () => {
      this.callbacks.saleCreated = this.callbacks.saleCreated
        .filter(cb => cb !== callback);
    };
  }

  // Close connection
  async disconnect() {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }
}

// Create and export a singleton instance
const signalRService = new SignalRService();
export default signalRService;