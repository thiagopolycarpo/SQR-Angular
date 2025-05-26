import React, { useState, useEffect } from "react";
   import { fetchOrders, submitProduction } from "../controllers/OrderController";
   import "../styles/ProductionForm.css";

   const ProductionForm = ({ onSubmission }) => {
     const [email, setEmail] = useState("");
     const [orders, setOrders] = useState([]);
     const [selectedOrder, setSelectedOrder] = useState(null);
     const [productionDate, setProductionDate] = useState("");
     const [quantity, setQuantity] = useState("");
     const [materialCode, setMaterialCode] = useState("");
     const [cycleTime, setCycleTime] = useState(0);
     const [startTime, setStartTime] = useState(null);
     const [response, setResponse] = useState(null);

     useEffect(() => {
       const loadOrders = async () => {
         const data = await fetchOrders();
         console.log("Fetched orders:", data);
         setOrders(data);
       };
       loadOrders();
     }, []);

     useEffect(() => {
       let timer;
       if (selectedOrder && startTime) {
         console.log("Starting timer, startTime:", startTime);
         timer = setInterval(() => {
           const currentTime = Date.now();
           const calculatedCycleTime = ((currentTime - startTime) / 1000).toFixed(1);
           console.log("Updating cycleTime:", calculatedCycleTime);
           setCycleTime(parseFloat(calculatedCycleTime));
         }, 50);
       }
       return () => {
         console.log("Clearing timer");
         clearInterval(timer);
       };
     }, [selectedOrder, startTime]);

     const handleOrderChange = (e) => {
       const orderId = e.target.value;
       const order = orders.find((o) => o.order === orderId);
       console.log("Selected order:", order);
       setSelectedOrder(order);
       setMaterialCode("");
       setCycleTime(0);
       setStartTime(order ? Date.now() : null);
     };

     const handleSubmit = async (e) => {
       e.preventDefault();
       const endTime = Date.now();
       const calculatedCycleTime = ((endTime - startTime) / 1000).toFixed(1);
       console.log("Submitting with cycleTime:", calculatedCycleTime);

       const productionData = {
         email,
         order: selectedOrder?.order,
         productionDate: productionDate,
         productionTime: new Date().toLocaleTimeString("en-US", { hour12: false }),
         quantity: parseFloat(quantity),
         materialCode,
         cycleTime: parseFloat(calculatedCycleTime),
       };

       console.log("Production data:", productionData);
       const res = await submitProduction(productionData);
       console.log("Submission response:", res);
       setResponse(res);
       onSubmission({ ...res, email });

       if (res.status === 200) {
         setEmail("");
         setSelectedOrder(null);
         setProductionDate("");
         setQuantity("");
         setMaterialCode("");
         setCycleTime(0);
         setStartTime(null);
       }
     };

     const isSubmitDisabled = !(
       selectedOrder &&
       email &&
       productionDate &&
       quantity &&
       materialCode &&
       cycleTime >= (selectedOrder?.cycleTime || 0)
     );
     console.log("Submit disabled:", isSubmitDisabled, {
       selectedOrder: !!selectedOrder,
       email: !!email,
       productionDate: !!productionDate,
       quantity: !!quantity,
       materialCode: !!materialCode,
       cycleTime,
       requiredCycleTime: selectedOrder?.cycleTime,
     });

     return (
       <div className="production-form">
         <h2>Production Reporting</h2>
         <form onSubmit={handleSubmit}>
           <div>
             <label>Email:</label>
             <input
               type="email"
               value={email}
               onChange={(e) => setEmail(e.target.value)}
               required
             />
           </div>
           <div>
             <label>Order:</label>
             <select onChange={handleOrderChange} value={selectedOrder?.order || ""}>
               <option value="">Select an order</option>
               {orders.map((order) => (
                 <option key={order.order} value={order.order}>
                   {order.order} - {order.productDescription}
                 </option>
               ))}
             </select>
           </div>
           {selectedOrder && (
             <div className="order-details">
               <p>Product: {selectedOrder.productDescription}</p>
               <img src={selectedOrder.image || "https://via.placeholder.com/100"} alt="Product" className="product-image" />
             </div>
           )}
           <div>
             <label>Production Date:</label>
             <input
               type="date"
               value={productionDate}
               onChange={(e) => setProductionDate(e.target.value)}
               required
             />
           </div>
           <div>
             <label>Quantity:</label>
             <input
               type="number"
               value={quantity}
               onChange={(e) => setQuantity(e.target.value)}
               min="0"
               max={selectedOrder?.quantity || 1000}
               required
             />
           </div>
           <div>
             <label>Material:</label>
             <select
               value={materialCode}
               onChange={(e) => setMaterialCode(e.target.value)}
               disabled={!selectedOrder}
               required
             >
               <option value="">Select a material</option>
               {selectedOrder?.materials.map((material) => (
                 <option key={material.materialCode} value={material.materialCode}>
                   {material.materialDescription}
                 </option>
               ))}
             </select>
           </div>
           <div>
             <label>Cycle Time (seconds):</label>
             <input type="number" value={cycleTime} readOnly />
           </div>
           <button type="button" onClick={() => setCycleTime(40)}>
             Force Cycle Time to 40s
           </button>
           <button type="submit" disabled={isSubmitDisabled}>
             Submit Production
           </button>
         </form>
         {response && (
           <div className={`response ${response.type === "E" ? "error" : "success"}`}>
             <p>Status: {response.status}</p>
             <p>{response.description}</p>
           </div>
         )}
       </div>
     );
   };

   export default ProductionForm;