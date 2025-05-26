import React, { useState, useEffect } from "react";
   import { fetchProductions } from "../controllers/OrderController";
   import "../styles/ProductionList.css";

   const ProductionList = ({ email }) => {
     const [productions, setProductions] = useState([]);
     const [error, setError] = useState(null);

     useEffect(() => {
       if (email) {
         const loadProductions = async () => {
           try {
             console.log("Fetching productions for email:", email);
             const data = await fetchProductions(email);
             console.log("Fetched productions:", data);
             setProductions(data);
             setError(null);
           } catch (err) {
             console.error("Error:", err);
             setError("Failed to load production history");
             setProductions([]);
           }
         };
         loadProductions();
       } else {
         setProductions([]);
         setError(null);
       }
     }, [email]);

     return (
       <div className="production-list">
         <h2>Production History</h2>
         {error && <p className="error">{error}</p>}
         {productions.length === 0 && !error ? (
           <p>No productions found for this email.</p>
         ) : (
           <table>
             <thead>
               <tr>
                 <th>Order</th>
                 <th>Date</th>
                 <th>Quantity</th>
                 <th>Material Code</th>
                 <th>Cycle Time (s)</th>
               </tr>
             </thead>
             <tbody>
               {productions.map((prod, index) => (
                 <tr key={index}>
                   <td>{prod.order}</td>
                   <td>{prod.date}</td>
                   <td>{prod.quantity}</td>
                   <td>{prod.materialCode}</td>
                   <td>{prod.cycleTime}</td>
                 </tr>
               ))}
             </tbody>
           </table>
         )}
       </div>
     );
   };

   export default ProductionList;