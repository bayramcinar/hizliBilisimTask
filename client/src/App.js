import "./App.css";
import { Route, Routes, Navigate } from "react-router-dom";
import LoginPage from "./Pages/LoginPage";
import TablePage from "./Pages/TablePage";
import ProtectedRoute from "./Components/ProtectedRoute";
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route path="/login/*" element={<LoginPage />} />
        <Route
          path="/panel/*"
          element={
            <ProtectedRoute>
              <TablePage />
            </ProtectedRoute>
          }
        />
      </Routes>
    </>
  );
}

export default App;
