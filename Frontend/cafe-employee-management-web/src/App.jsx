import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import CafeList from "./components/cafe/CafeList";
import EmployeeList from "./components/employee/EmployeeList";
import EmployeeForm from "./components/employee/EmployeeForm";
import CafeForm from "./components/cafe/CafeForm";
import Navbar from "./components/common/Navbar";

function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<CafeList />} />
        <Route path="/employees" element={<EmployeeList />} />
        <Route path="/employees/add" element={<EmployeeForm />} />
        <Route path="/employees/edit/:id" element={<EmployeeForm />} />
        <Route path="/cafes/add" element={<CafeForm />} />
        <Route path="/cafes/edit/:id" element={<CafeForm />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
