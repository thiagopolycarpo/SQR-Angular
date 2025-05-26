import { createBrowserRouter } from "react-router-dom";
   import HomePage from "./pages/HomePage";
   import HistoryPage from "./pages/HistoryPage";

   export const router = createBrowserRouter([
     {
       path: "/",
       element: <HomePage />,
     },
     {
       path: "/history",
       element: <HistoryPage />,
     },
   ]);