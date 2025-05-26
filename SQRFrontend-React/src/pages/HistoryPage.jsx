import React, { useState } from "react";
   import { Link } from "react-router-dom";
   import ProductionList from "../components/ProductionList";
   import "../styles/HistoryPage.css";

   const HistoryPage = () => {
     const [email, setEmail] = useState("");

     return (
       <div className="history-page">
         <nav>
           <Link to="/">Production Reporting</Link> | <Link to="/history">Production History</Link>
         </nav>
         <div className="history-content">
           <div className="email-input">
             <label>Email:</label>
             <input
               type="email"
               value={email}
               onChange={(e) => setEmail(e.target.value)}
               placeholder="Enter email to view history"
             />
           </div>
           <ProductionList email={email} />
         </div>
       </div>
     );
   };

   export default HistoryPage;