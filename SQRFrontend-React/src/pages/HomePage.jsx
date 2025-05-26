import React from "react";
   import { Link } from "react-router-dom";
   import ProductionForm from "../components/ProductionForm";

   const HomePage = () => {
     return (
       <div>
         <nav>
           <Link to="/">Production Reporting</Link> | <Link to="/history">Production History</Link>
         </nav>
         <ProductionForm onSubmission={() => {}} />
       </div>
     );
   };

   export default HomePage;