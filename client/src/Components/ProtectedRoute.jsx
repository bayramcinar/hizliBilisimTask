import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import axios from "axios";

function ProtectedRoute({ children }) {
  const [isValid, setIsValid] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("authToken");

    if (!token) {
      setIsValid(false);
      setLoading(false);
      return;
    }

    axios
      .get("http://localhost:5281/api/Auth/verify-token", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        if (response.data.message === "Token geçerli.") {
          setIsValid(true);
        } else {
          setIsValid(false);
        }
        setLoading(false);
      })
      .catch(() => {
        setIsValid(false);
        setLoading(false);
      });
  }, []);

  if (loading) {
    return <div>Yükleniyor...</div>;
  }

  if (!isValid) {
    return <Navigate to="/login" replace />;
  }

  return children;
}

export default ProtectedRoute;
