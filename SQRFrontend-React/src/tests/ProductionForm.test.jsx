import { render, screen, fireEvent, waitFor } from "@testing-library/react";
   import ProductionForm from "../components/ProductionForm";
   import * as OrderController from "../controllers/OrderController";

   jest.mock("../controllers/OrderController");

   describe("ProductionForm", () => {
     const mockOrders = [
       {
         order: "111",
         quantity: 100,
         productCode: "abc",
         productDescription: "xxx",
         image: "https://via.placeholder.com/100",
         cycleTime: 30.3,
         materials: [
           { materialCode: "aaa", materialDescription: "desc1" },
           { materialCode: "bbb", materialDescription: "desc2" },
         ],
       },
     ];

     beforeEach(() => {
       OrderController.fetchOrders.mockResolvedValue(mockOrders);
       OrderController.submitProduction.mockResolvedValue({
         status: 200,
         type: "S",
         description: "Production submitted successfully",
         email: "teste@sqr.com.br",
       });
       jest.useFakeTimers();
     });

     afterEach(() => {
       jest.useRealTimers();
       jest.clearAllMocks();
     });

     it("renders form fields", async () => {
       render(<ProductionForm onSubmission={jest.fn()} />);
       await waitFor(() => {
         expect(screen.getByLabelText(/Email/i)).toBeInTheDocument();
         expect(screen.getByLabelText(/Order/i)).toBeInTheDocument();
         expect(screen.getByLabelText(/Production Date/i)).toBeInTheDocument();
         expect(screen.getByLabelText(/Quantity/i)).toBeInTheDocument();
         expect(screen.getByLabelText(/Material/i)).toBeInTheDocument();
         expect(screen.getByLabelText(/Cycle Time/i)).toBeInTheDocument();
       });
     });

     it("disables submit button until cycle time is sufficient", async () => {
       render(<ProductionForm onSubmission={jest.fn()} />);
       const submitButton = screen.getByText(/Submit Production/i);
       expect(submitButton).toBeDisabled();

       fireEvent.change(screen.getByLabelText(/Order/i), { target: { value: "111" } });
       await waitFor(() => {
         expect(screen.getByText(/Product: xxx/i)).toBeInTheDocument();
       });

       fireEvent.change(screen.getByLabelText(/Email/i), { target: { value: "teste@sqr.com.br" } });
       fireEvent.change(screen.getByLabelText(/Production Date/i), { target: { value: "2021-01-01" } });
       fireEvent.change(screen.getByLabelText(/Quantity/i), { target: { value: "50" } });
       fireEvent.change(screen.getByLabelText(/Material/i), { target: { value: "aaa" } });

       jest.advanceTimersByTime(31000); // Advance past 30.3s
       await waitFor(() => {
         expect(submitButton).not.toBeDisabled();
       }, { timeout: 5000 });
     });
   });