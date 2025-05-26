import axios from "axios";

   const API_BASE_URL = "http://localhost:5267/api/orders";

   export const fetchOrders = async () => {
     try {
       const response = await axios.get(`${API_BASE_URL}/GetOrders`);
       return response.data.orders;
     } catch (error) {
       console.error("Error fetching orders:", error);
       return [];
     }
   };

   export const submitProduction = async (productionData) => {
     try {
       const response = await axios.post(`${API_BASE_URL}/SetProduction`, productionData);
       return response.data;
     } catch (error) {
       console.error("Error submitting production:", error);
       return { status: 500, type: "E", description: "Failed to submit production" };
     }
   };

   export const fetchProductions = async (email) => {
     try {
       const response = await axios.get(`${API_BASE_URL}/GetProduction?email=${encodeURIComponent(email)}`);
       return response.data.productions;
     } catch (error) {
       console.error("Error fetching productions:", error);
       return [];
     }
   };